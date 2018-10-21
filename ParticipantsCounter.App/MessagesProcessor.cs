using ParticipantsCounter.App.Commands;
using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;
using ParticipantsCounter.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace ParticipantsCounter.App
{
    public class MessagesProcessor
    {
        private readonly EventsRepository _eventsRepository;

        public MessagesProcessor(IStorage<List<Event>> storage)
        {
            _eventsRepository = new EventsRepository(storage);
        }

        public string ProcessMessage(ChatMessage message)
        {
            var commandType = MessageParser.GetCommandTypeFromMessage(message);

            if (commandType == CommandType.NotCommand)
            {
                return string.Empty;
            }

            var command = CreateCommand(commandType, message);
            return command.Execute();
        }

        private BaseCommand CreateCommand(CommandType commandType, ChatMessage message)
        {
            switch (commandType)
            {
                case CommandType.Add:
                    return new AddParticipantsGroupCommand(message, _eventsRepository);
                case CommandType.NotSure:
                    return new AddNotSuredParticipantsGroupCommand(message, _eventsRepository);
                case CommandType.Remove:
                    return new RemoveParticipantsGroupCommand(message, _eventsRepository);
                case CommandType.Count:
                    return new GetParticipantsCountCommand(message, _eventsRepository);
                case CommandType.List:
                    return new GetListOfParticipantsCommand(message, _eventsRepository);
                default:
                    throw new NotImplementedException("Unexpected command");
            }
        }
    }
}
