using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Set
    {
        public int id { get; set; }
        public int eventId { get; set; }
        public int phaseGroupId { get; set; }
        public int? stationId { get; set; }
        public int? entrant1Id { get; set; }
        public int? entrant2Id { get; set; }
        public int? winnerId { get; set; }
        public int? loserId { get; set; }
        public string bracketId { get; set; }
        public string entrant1PrereqType { get; set; }
        public int? entrant1PrereqId { get; set; }
        public string entrant1PrereqCondition { get; set; }
        public string entrant2PrereqType { get; set; }
        public int? entrant2PrereqId { get; set; }
        public string entrant2PrereqCondition { get; set; }
        public object wProgressionSeedId { get; set; }
        public object lProgressionSeedId { get; set; }
        public int? wPlacement { get; set; }
        public int? lPlacement { get; set; }
        public int? wOverallPlacement { get; set; }
        public int? lOverallPlacement { get; set; }
        public object lEligiblePhaseId { get; set; }
        public object wEligiblePhaseId { get; set; }
        public object unreachable { get; set; }
        public object isTiebreak { get; set; }
        public int type { get; set; }
        public object crewPlayerCount { get; set; }
        public string identifier { get; set; }
        public int round { get; set; }
        public int state { get; set; }
        public int? entrant1Score { get; set; }
        public int? entrant2Score { get; set; }
        public int bestOf { get; set; }
        public object totalGames { get; set; }
        public object setGamesType { get; set; }
        public bool isLast { get; set; }
        public bool isGF { get; set; }
        public bool hasPlaceholder { get; set; }
        public bool entrant1Present { get; set; }
        public bool entrant2Present { get; set; }
        public object adminViewedBy { get; set; }
        public object adminMessagedBy { get; set; }
        public object subState { get; set; }
        public object modRequestedBy { get; set; }
        public object modRequestedAt { get; set; }
        public object vodUrl { get; set; }
        public object fbUrl { get; set; }
        public object smashggUrl { get; set; }
        public object durationSeconds { get; set; }
        public int updatedAt { get; set; }
        public double? updatedAtMicro { get; set; }
        public object startAt { get; set; }
        public int? startedAt { get; set; }
        public long? completedAt { get; set; }
        public object adminViewedAt { get; set; }
        public object adminMessagedAt { get; set; }
        public object progressionPlacement { get; set; }
        public object progressionCount { get; set; }
        public List<object> games { get; set; }
        public int videogameId { get; set; }
        public int eventType { get; set; }
        public List<object> images { get; set; }
        public List<string> expand { get; set; }
        public int displayRound { get; set; }
        public int originalRound { get; set; }
        public string fullRoundText { get; set; }
        public string midRoundText { get; set; }
        public string shortRoundText { get; set; }
        public int? roundDivision { get; set; }
        public List<int?> entrant1CharacterIds { get; set; }
        public string entrant1PrereqStr { get; set; }
        public string entrant2PrereqStr { get; set; }
    }
}