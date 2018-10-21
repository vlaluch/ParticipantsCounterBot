using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public class RemoveParticipantsGroupCommand : GetParticipantsCountCommand
    {
        public RemoveParticipantsGroupCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
            : base(commandMessage, eventsRepository)
        { }

        public override string Execute()
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(Message);
            participantsGroup.IsSure = true;
            EventsRepository.RemoveParticipantGroupFromEvent(Message.ChatName, participantsGroup);
            return base.Execute();
        }
    }
}
