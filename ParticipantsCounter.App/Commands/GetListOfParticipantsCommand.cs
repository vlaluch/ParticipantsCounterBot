using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public class GetListOfParticipantsCommand : BaseCommand
    {
        public GetListOfParticipantsCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
            : base(commandMessage, eventsRepository)
        { }

        public override string Execute()
        {
            return OutputFormatter.FormatListResponse(EventsRepository.GetAllParticipantsGroupsForEvent(Message.ChatName));
        }
    }
}
