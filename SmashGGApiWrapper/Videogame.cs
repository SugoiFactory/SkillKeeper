using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Videogame
    {
        public int id { get; set; }
        public string abbrev { get; set; }
        public string name { get; set; }
        public string displayName { get; set; }
        public int minPerEntry { get; set; }
        public int maxPerEntry { get; set; }
        public bool enabled { get; set; }
        public string slug { get; set; }
        public object isCardGame { get; set; }
        public object characterTerm { get; set; }
        public object stageTerm { get; set; }
        public object initialStocks { get; set; }
        public List<Image2> images { get; set; }
    }
}