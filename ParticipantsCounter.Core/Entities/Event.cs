using System;
using System.Collections.Generic;

namespace ParticipantsCounter.Core.Entities
{
    public class Event
    {
        public Event(string name, DateTime date)
        {
            Name = name;
            Date = date;
            Participants = new List<ParticipantsGroup>();
        }

        public string Name { get; }

        public DateTime Date { get; }

        public List<ParticipantsGroup> Participants { get; }
    }
}