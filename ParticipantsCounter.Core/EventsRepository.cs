using ParticipantsCounter.Core.Entities;
using ParticipantsCounter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParticipantsCounter.Core.Infrastructure
{
    public class EventsRepository
    {
        private List<Event> _events;
        private readonly IStorage<List<Event>> _storage;

        public EventsRepository(IStorage<List<Event>> storage)
        {
            _storage = storage;
            LoadState();
        }

        public void AddParticipantsGroupToEvent(string eventName, ParticipantsGroup participantsGroup)
        {
            var eventData = GetEventByName(eventName);

            if (eventData == null)
            {
                eventData = new Event(eventName, DateTime.Now);
                _events.Add(eventData);
            }

            var existingParticipantGroup = eventData.Participants.FirstOrDefault(p => p.Name == participantsGroup.Name);

            if (existingParticipantGroup == null)
            {
                eventData.Participants.Add(participantsGroup);
            }
            else if (existingParticipantGroup.IsSure != participantsGroup.IsSure)
            {
                existingParticipantGroup.IsSure = participantsGroup.IsSure;
            }
            else if (participantsGroup.Count > 1)
            {
                existingParticipantGroup.Count += participantsGroup.Count;
            }

            _storage.Save(_events);            
        }

        public void RemoveParticipantGroupFromEvent(string eventName, ParticipantsGroup participantInfo)
        {
            var eventData = GetEventByName(eventName);

            if (eventData == null)
            {
                return;
            }

            var existingParticipant = eventData.Participants.FirstOrDefault(p => p.Name == participantInfo.Name);

            if (existingParticipant != null)
            {
                if (participantInfo.Count >= existingParticipant.Count)
                {
                    eventData.Participants.Remove(existingParticipant);
                }
                else
                {
                    existingParticipant.Count -= participantInfo.Count;
                }
            }

            _storage.Save(_events);
        }

        public List<ParticipantsGroup> GetAllParticipantsGroupsForEvent(string eventName)
        {
            var eventData = GetEventByName(eventName);
            return eventData?.Participants;
        }

        private Event GetEventByName(string name)
        {
            return _events.FirstOrDefault(x => x.Name == name);
        }

        private void LoadState()
        {            
            var allEvents = _storage.Load();
            if (allEvents != null)
            {
                _events = new List<Event>();

                foreach (var eventData in allEvents)
                {
                    if (eventData.Date.AddDays(3) > DateTime.Now)
                    {
                        _events.Add(eventData);
                    }
                }
            }
        }             
    }
}