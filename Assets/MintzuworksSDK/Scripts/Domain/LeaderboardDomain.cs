using System;
using System.Collections.Generic;

namespace Mintzuworks.Domain
{
    [Serializable]
    public class GetLeaderboardRequest
    {
        public string statisticName;
        public int version;
        public int maxResultsCount = 1;
    }

    [Serializable]
    public class LeaderboardEntry
    {
        public string userID;
        public string customID;
        public string displayName;
        public int value;
        public int rank;
    }

    [Serializable]
    public class GetLeaderboardResult : CommonResult
    {
        public List<LeaderboardEntry> data;
    }

}
