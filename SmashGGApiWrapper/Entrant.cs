namespace SmashGGApiWrapper
{
    public class Entrant
    {
        public object isPlaceholder { get; set; }
        public int id { get; set; }
        public int eventId { get; set; }
        public object participant1Id { get; set; }
        public object participant2Id { get; set; }
        public string name { get; set; }
        public int finalPlacement { get; set; }
        public int defaultSkill { get; set; }
        public object skill { get; set; }
        public int skillOrder { get; set; }
        public object unverified { get; set; }
        public int initialSeedNum { get; set; }
        public Mutations mutations { get; set; }
    }
}