using ParticipantsCounter.Core.Interfaces;
using System;

namespace ParticipantsCounter.Tests
{
    public class FictiveStorage<T> : IStorage<T> where T : class
    {
        public T Load()
        {
            return Activator.CreateInstance<T>();
        }

        public void Save(T data)
        {
            
        }
    }
}
