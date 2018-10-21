namespace ParticipantsCounter.Core.Interfaces
{
    public interface IStorage<T> where T : class
    {
        void Save(T data);
        T Load();
    }
}
