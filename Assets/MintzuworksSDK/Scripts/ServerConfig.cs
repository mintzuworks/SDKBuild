using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mintzuworks.Network
{
    [CreateAssetMenu(fileName = "Server Config", menuName = "Mintzuworks/Server Config")]
    public class ServerConfig : ScriptableObject
    {
        public ServerRegion region;
        public string serverName;
        public string serverURL;
        public string cdnURL;
    }

    public enum ServerRegion
    {
        Global,
        Asia,
        US,
        CN,
        Local
    }
}