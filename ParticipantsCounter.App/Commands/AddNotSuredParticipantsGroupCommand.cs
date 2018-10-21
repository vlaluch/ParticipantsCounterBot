using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;

namespace ParticipantsCounter.App.Commands
{
    public class AddNotSuredParticipantsGroupCommand : GetParticipantsCountCommand
    {
        public AddNotSuredParticipantsGroupCommand(
            ChatMessage commandMessage,
            EventsRepository participantsRepository)
            : base(commandMessage, participantsRepository)
        { }

        public override string Execute()
        {
            var participantsGroup = MessageParser.GetParticipantsGroupFromMessage(Message);
            participantsGroup.IsSure = false;
            EventsRepository.AddParticipantsGroupToEvent(Message.ChatName, participantsGroup);
            return base.Execute();
        }
    }
}
