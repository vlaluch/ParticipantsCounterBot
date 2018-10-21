using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public abstract class BaseCommand
    {
        protected BaseCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
        {
            Message = commandMessage;
            EventsRepository = eventsRepository;
        }

        public abstract string Execute();

        protected ChatMessage Message { get; }

        protected EventsRepository EventsRepository { get; }
    }
}
