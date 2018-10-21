using Newtonsoft.Json;
using ParticipantsCounter.Core.Interfaces;
using System.IO;

namespace ParticipantsCounter.Core.Infrastructure
{
    public class JsonStorage<T> : IStorage<T> where T: class
    {
        private readonly string _fileName;

        public JsonStorage(string fileName)
        {
            _fileName = fileName;
        }

        public T Load()
        {
            if (!File.Exists(_fileName))
            {
                File.CreateText(_fileName);
            }

            var stateJson = File.ReadAllText(_fileName);
            return JsonConvert.DeserializeObject<T>(stateJson);
        }

        public void Save(T data)
        {
            var serializer = new JsonSerializer();
            using (var writeStream = File.CreateText(_fileName))
            {
                serializer.Serialize(writeStream, data);
            }
        }
    }
}
