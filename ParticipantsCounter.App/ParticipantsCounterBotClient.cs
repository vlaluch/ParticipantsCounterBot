using ParticipantsCounter.App.Infrastructure;
using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace ParticipantsCounter.App
{
    public class ParticipantsCounterBotClient
    {
        private const string StateFileName = "state.json";
        private static TelegramBotClient _client;
        private static MessagesProcessor _messagesProcessor;

        public delegate void MessageReceivedHandler(ChatMessage message);
        public delegate void ErrorOccuredHandler(string errorMessage);

        public event MessageReceivedHandler OnMessageReceived;
        public event ErrorOccuredHandler OnErrorOccured;

        public ParticipantsCounterBotClient()
        {
            var storage = new JsonStorage<List<Event>>(StateFileName);
            _messagesProcessor = new MessagesProcessor(storage);

            _client = new TelegramBotClient(
                ApplicationSettingsManager.Token,
                CreateClient()
            );

            _client.OnMessage += ClientOnMessageReceived;
            _client.OnReceiveError += ClientOnReceiveError;
        }

        public void Run()
        {
            _client.StartReceiving(Array.Empty<UpdateType>());
        }

        public void Stop()
        {
            _client.StopReceiving();
        }

        private static HttpClient CreateClient()
        {
            var proxy = new WebProxy($"http://{ApplicationSettingsManager.ProxyAddress}:{ApplicationSettingsManager.ProxyPort}", false, new string[] { });

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                UseProxy = true
            };

            return new HttpClient(handler: httpClientHandler, disposeHandler: true);
        }

        private async void ClientOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            if (message == null || message.Type != MessageType.Text) return;

            var messageInfo = message.ToChatMessage();

            OnMessageReceived?.Invoke(messageInfo);

            try
            {
                var response = _messagesProcessor.ProcessMessage(messageInfo);
                if (!string.IsNullOrEmpty(response))
                {
                    await _client.SendTextMessageAsync(message.Chat.Id, response);
                }
            }
            catch (Exception e)
            {
                OnErrorOccured?.Invoke(e.Message);
            }
        }

        private void ClientOnReceiveError(object sender, ReceiveErrorEventArgs errorEventArgs)
        {
            OnErrorOccured?.Invoke(errorEventArgs.ApiRequestException.Message);
        }
    }
}
