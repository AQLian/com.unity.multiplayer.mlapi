using System.Collections.Generic;

namespace MLAPI.Connection
{
    /// <summary>
    /// A NetworkClient
    /// </summary>
    public class NetworkClient
    {
        public bool IsClientDoneLoadingScene;

        /// <summary>
        /// The ClientId of the NetworkClient
        /// </summary>
        public ulong ClientId;

        /// <summary>
        /// The PlayerObject of the Client
        /// </summary>
        public NetworkObject PlayerObject;

        /// <summary>
        /// The NetworkObject's owned by this Client
        /// </summary>
        public readonly List<NetworkObject> OwnedObjects = new List<NetworkObject>();
    }
}
