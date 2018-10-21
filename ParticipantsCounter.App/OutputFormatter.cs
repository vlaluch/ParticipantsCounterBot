using ParticipantsCounter.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace ParticipantsCounter.App
{
    public class OutputFormatter
    {
        public static string FormatCountResponse(int suredCount, int notSuredCount)
        {
            if (suredCount > 0 && notSuredCount > 0)
            {
                return $"{suredCount} и {notSuredCount}±";
            }
            if (suredCount > 0)
            {
                return suredCount.ToString();
            }
            if (notSuredCount > 0)
            {
                return $"{notSuredCount}±";
            }

            return "0";
        }

        public static string FormatListResponse(List<ParticipantsGroup> participants)
        {
            if (participants == null || participants.Count == 0)
            {
                return "Пока пусто. Жду ваших плюсов!";
            }

            var output = "";

            var suredParticipants = participants.Where(x => x.IsSure).ToList();
            var notSuredParticipants = participants.Where(x => !x.IsSure).ToList();

            if (suredParticipants.Count > 0)
            {
                if (notSuredParticipants.Count > 0)
                {
                    output += $"Пойдут:\r\n";
                }

                output += OutputParticipantsGroups(suredParticipants);
            }

            if (notSuredParticipants.Count > 0)
            {
                output += $"Пока не уверены:\r\n";
                output += OutputParticipantsGroups(notSuredParticipants);
            }

            return output;
        }

        private static string OutputParticipantsGroups(List<ParticipantsGroup> participantsGroups)
        {
            var output = "";
            var counter = 0;

            for (int i = 0; i < participantsGroups.Count; i++)
            {
                if (participantsGroups[i].Count > 1)
                {
                    output += $"{counter + 1}-{counter + participantsGroups[i].Count}. {participantsGroups[i].Name} - {participantsGroups[i].Count}\r\n";
                    counter += participantsGroups[i].Count;
                }
                else
                {
                    output += $"{counter + 1}. {participantsGroups[i].Name}\r\n";
                    counter++;
                }
            }

            return output;
        }
    }
}
