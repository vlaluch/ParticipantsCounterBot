using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public class CleanParticipantsListCommand : BaseCommand
    {
        public CleanParticipantsListCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
            : base(commandMessage, eventsRepository)
        { }

        public override string Execute()
        {
            EventsRepository.RemoveAllParticipantsGroupsFromEvent(Message.ChatName);
            return "Вжух! И список пуст";
        }
    }
}
