using System;
using System.Collections.Generic;

namespace Mintzuworks.Domain
{
    [Serializable]
    public class StartMatchRequest
    {

    }

    [Serializable]
    public class StartMatchResult : CommonResult
    {
        public string matchID;
        public string matchCode;
        public string matchType;
        public string matchStatus;
        public string matchStartTime;
        public string matchEndTime;
        public int playerCount;
        public int maxPlayerCount;
    }

    [Serializable]
    public class EndMatchRequest
    {
        public string matchID;
        public string gameMode;
        public string map;
        public int depth;
        public float duration;
        public Dictionary<string, object> customJSON;
    }


    [Serializable]
    public class MatchData
    {
        public string id;
        public string matchID;
        public string gameMode;
        public string map;
        public string startTime;
        public string endTime;
        public int depth;
        public float duration;
        public float score;
        public Dictionary<string, object> customJSON;
        public string createdAt;

        public DateTime CreatedAt => DateTime.Parse(createdAt);
        public DateTime StartTime => DateTime.Parse(startTime);
        public DateTime EndTime => DateTime.Parse(endTime);
    }

    [System.Serializable]
    public class MatchResult : GeneralResultWithData<MatchData>
    {
    }
}