using System.Collections.Generic;

namespace SmashGGApiWrapper
{
    public class Event
    {
        public int id { get; set; }
        public int tournamentId { get; set; }
        public int state { get; set; }
        public int progressMeter { get; set; }
        public int videogameId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string slug { get; set; }
        public object entryFee { get; set; }
        public object entryCap { get; set; }
        public object entrantSizeMin { get; set; }
        public object entrantSizeMax { get; set; }
        public object requiredCheckinNum { get; set; }
        public object teamNameAllowed { get; set; }
        public object teamManagementDeadline { get; set; }
        public object allowAutoReport { get; set; }
        public object tmgEventType { get; set; }
        public object templateId { get; set; }
        public string gameName { get; set; }
        public int playersPerEntry { get; set; }
        public object exhibition { get; set; }
        public object platform { get; set; }
        public object version { get; set; }
        public int type { get; set; }
        public object formatType { get; set; }
        public int teamsFormat { get; set; }
        public int entrantMode { get; set; }
        public bool @private { get; set; }
        public bool isOnline { get; set; }
        public bool hasTasks { get; set; }
        public bool enableSlippi { get; set; }
        public bool hasMatchmaking { get; set; }
        public object isPlaceholder { get; set; }
        public int startAt { get; set; }
        public int endAt { get; set; }
        public object startedAt { get; set; }
        public object completedAt { get; set; }
        public int projectionMode { get; set; }
        public object rulesetId { get; set; }
        public PageConfig pageConfig { get; set; }
        public List<object> gameModeConfig { get; set; }
        public List<object> stations { get; set; }
        public List<object> streams { get; set; }
        public List<object> waves { get; set; }
        public List<object> images { get; set; }
        public int scheduleId { get; set; }
        public List<object> expand { get; set; }
        public string typeDisplayStr { get; set; }
    }
    public class PageConfig
    {
        public List<object> header { get; set; }
        public List<object> tabs { get; set; }
    }
}