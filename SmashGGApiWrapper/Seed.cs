using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmashGGApiWrapper
{
    class Seed
    {
        public int id { get; set; }
        public int phaseId { get; set; }
        public int phaseLinkId { get; set; }
        public int entrantId { get; set; }
        public object prereqProgressionId { get; set; }
        public int seedNum { get; set; }
        public int placement { get; set; }
        public int losses { get; set; }
        public bool isFinal { get; set; }
        public bool? isSeeded { get; set; }
        public bool disqualified { get; set; }
        public bool unverified { get; set; }
        public int phaseGroupId { get; set; }
        public int groupSeedNum { get; set; }
        public object progressionSeedId { get; set; }
        public object projectedEntrantId { get; set; }
        public List<object> expand { get; set; }
        public Mutations mutations { get; set; }
    }
}
