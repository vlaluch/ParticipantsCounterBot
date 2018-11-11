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

            if (!ExecutionWillUpdateCurrentCount(participantsGroup))
            {
                return string.Empty;
            }
            
            EventsRepository.AddParticipantsGroupToEvent(Message.ChatName, participantsGroup);
            return base.Execute();
        }

        private bool ExecutionWillUpdateCurrentCount(ParticipantsGroup participantsGroup)
        {
            if (participantsGroup.Name != Message.AuthorName)
            {
                return true;
            }

            var existingParticipantsGroup = EventsRepository.GetParticipantsGroup(Message.ChatName, participantsGroup.Name);
            return existingParticipantsGroup == null || existingParticipantsGroup.IsSure;            
        }
    }
}
