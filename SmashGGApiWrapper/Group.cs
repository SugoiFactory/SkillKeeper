using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Group
    {
        public int id { get; set; }
        public int phaseId { get; set; }
        public object waveId { get; set; }
        public object poolRefId { get; set; }
        public int groupTypeId { get; set; }
        public object title { get; set; }
        public string identifier { get; set; }
        public string displayIdentifier { get; set; }
        public int state { get; set; }
        public object tiebreakOrder { get; set; }
        public object tiebreaks { get; set; }
        public object bestOf { get; set; }
        public int setsOnDeck { get; set; }
        public int rematchSeconds { get; set; }
        public bool finalized { get; set; }
        public object winnersTargetPhaseId { get; set; }
        public int numProgressing { get; set; }
        public object losersTargetPhaseId { get; set; }
        public object startAt { get; set; }
        public object startedAt { get; set; }
        public List<object> seeds { get; set; }
        public List<object> sets { get; set; }
        public object rounds { get; set; }
        public object numRounds { get; set; }
        public object pointsPerMatchWin { get; set; }
        public int pointsPerGameWin { get; set; }
        public object pointsPerBye { get; set; }
        public bool matchmakingEnabled { get; set; }
        public object scheduleId { get; set; }
        public List<object> expand { get; set; }
        public bool hasSets { get; set; }
        public int stationId { get; set; }
        public bool hasCustomWinnerByes { get; set; }
    }
}