using System;

namespace PlaylistCode
{
    /// <summary>
    ///     The collection that bplist files holds the song id's in.
    /// </summary>
    [Serializable]
    public struct HashBase
    {
        /// <summary> The hash/id of the current song. </summary>
        public string hash;
    }
}