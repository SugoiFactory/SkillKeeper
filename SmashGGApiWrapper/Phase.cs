using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Phase
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int typeId { get; set; }
        public int phaseOrder { get; set; }
        public string name { get; set; }
        public int state { get; set; }
        public int pendingSeeds { get; set; }
        public bool locked { get; set; }
        public bool isDefault { get; set; }
        public object isExhibition { get; set; }
        public int tier { get; set; }
        public bool playGF { get; set; }
        public object loserStartDirection { get; set; }
        public object defaultLoserStartDirection { get; set; }
        public object bracketInitializer { get; set; }
        public int groupCount { get; set; }
        public object bracketMap { get; set; }
        public List<object> groups { get; set; }
        public string preserveLosses { get; set; }
        public string shuffleWinners { get; set; }
        public string shuffleLosers { get; set; }
        public string sequentialOrdering { get; set; }
        public string playFullBrackets { get; set; }
        public string entrantsIn { get; set; }
        public string entrantsOut { get; set; }
        public string avoidRematch { get; set; }
    }
}