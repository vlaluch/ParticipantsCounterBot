using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace ParticipantsCounter.App.Commands
{
    public class GetParticipantsCountCommand : BaseCommand
    {
        public GetParticipantsCountCommand(
            ChatMessage commandMessage,
            EventsRepository eventsRepository)
            : base(commandMessage, eventsRepository)
        { }

        public override string Execute()
        {
            var participants = EventsRepository.GetAllParticipantsGroupsForEvent(Message.ChatName);

            if(participants == null)
            {
                return "0";
            }

            var suredParticipants = participants.Where(x => x.IsSure).ToList();
            var notSuredParticipants = participants.Where(x => !x.IsSure).ToList();

            return OutputFormatter.FormatCountResponse(
                CountParticipants(suredParticipants),
                CountParticipants(notSuredParticipants));
        }

        private int CountParticipants(List<ParticipantsGroup> participantsGroups)
        {
            var count = 0;

            foreach (var participantsGroup in participantsGroups)
            {
                count += participantsGroup.Count;
            }

            return count;
        }
    }
}
