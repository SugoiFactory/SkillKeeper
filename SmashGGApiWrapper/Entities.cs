using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Entities
    {
        public Tournament tournament { get; set; }
        public List<Event> @event { get; set; }
        public List<Videogame> videogame { get; set; }
        public List<Group> groups { get; set; }
        public List<Phase> phase { get; set; }
        public List<Entrant> entrants { get; set; }
        public List<Player> player { get; set; }
        public List<RankingSery> rankingSeries { get; set; }
        public List<RankingIteration> rankingIteration { get; set; }
        public List<Set> sets { get; set; }
        public List<Station> station { get; set; }

    }
}