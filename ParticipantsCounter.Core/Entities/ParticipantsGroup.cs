namespace ParticipantsCounter.Core.Entities
{
    public class ParticipantsGroup
    {
        public ParticipantsGroup() { }

        public ParticipantsGroup(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; set; }

        public int Count { get; set; }

        public bool IsSure { get; set; }
    }
}