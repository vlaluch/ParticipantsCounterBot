using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public class AddParticipantsGroupCommand : GetParticipantsCountCommand
    {
        public AddParticipantsGroupCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
            : base(commandMessage, eventsRepository)
        { }

        public override string Execute()
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(Message);
            participantsGroup.IsSure = true;
            EventsRepository.AddParticipantsGroupToEvent(Message.ChatName, participantsGroup);
            return base.Execute();
        }
    }
}
