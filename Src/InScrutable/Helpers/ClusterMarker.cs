using System;
using System.Collections.Generic;
using System.Text;

namespace InScrutable.Helpers
{
    internal struct ClusterMarker
    {
        private const int UnInitializedIndex = -1;

        internal int ClusterStartIndex { get; private set; }

        internal int ClusterEndIndex { get; private set; }

        internal bool IsInitialized { get; private set; }

#if DEBUG
        internal readonly string OriginalCompleteString;

        internal string SubStringCluster { get; private set; }
#endif

        public ClusterMarker()
#if DEBUG
        : this(string.Empty)
#endif
        {
            ResetToInitState();
        }

#if DEBUG
        internal ClusterMarker(string original)
        {
            ResetToInitState();
            OriginalCompleteString = original;
            SubStringCluster = original;
        }
#endif

        internal void ResetToInitState()
        {
            ClusterStartIndex = ClusterEndIndex = UnInitializedIndex;
            IsInitialized = false;
#if DEBUG
            SubStringCluster = string.Empty;
#endif
        }

        internal void Assign(int clusterStart, int clusterEnd)
        {
            ClusterStartIndex = clusterStart;
            ClusterEndIndex = clusterEnd;
            IsInitialized = true;
#if DEBUG
            if (!string.IsNullOrEmpty(OriginalCompleteString))
            {
                SubStringCluster = OriginalCompleteString.Substring(ClusterStartIndex,
                    ClusterEndIndex - ClusterStartIndex + 1);
            }
#endif
        }
    }
}
