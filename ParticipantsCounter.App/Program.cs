using ParticipantsCounter.Core.Entities;
using System;

namespace ParticipantsCounter.App
{
    public static class Program
    {
        private static ParticipantsCounterBotClient _bot;

        public static void Main()
        {
            _bot = new ParticipantsCounterBotClient();
            _bot.OnMessageReceived += BotOnMessageReceived;
            _bot.OnErrorOccured += BotOnErrorOccured;

            _bot.Run();
            WriteMessage("started");

            Console.ReadLine();
            _bot.Stop();
            WriteMessage("stopped");
        }

        private static void BotOnMessageReceived(ChatMessage message)
        {
            WriteMessage($"message received from {message.ChatName}");
        }

        private static void BotOnErrorOccured(string errorMessage)
        {
            WriteMessage("oops, error occured");
        }

        private static void WriteMessage(string message)
        {
            Console.WriteLine($"{DateTime.Now:yyyy.MM.dd HH:mm} {message}");
        }
    }
}