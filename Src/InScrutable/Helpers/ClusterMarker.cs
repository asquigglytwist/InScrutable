using InScrutable.Global;
using System;
using System.Collections.Generic;
using System.Text;

namespace InScrutable.Helpers
{
    /// <summary>
    /// A (sub) string cluster
    /// </summary>
    internal struct ClusterMarker
    {
        private const int UnInitializedClusterIndex = Constants.UnInitializedArrayIndex;

        /// <summary>
        /// <para>Start index (position) of the cluster - within the original string</para>
        /// </summary>
        /// <remarks>Indices are 0-Based</remarks>
        internal int ClusterStartIndex { get; private set; }

        /// <summary>
        /// <para>End index (position) of the cluster - within the original string</para>
        /// </summary>
        /// <remarks>Indices are 0-Based</remarks>
        internal int ClusterEndIndex { get; private set; }

        /// <summary>
        /// <para>Flag indicating initialization state of this <see cref="ClusterMarker" /></para>
        /// See <see cref="ResetToInitState" /> to reset this <see cref="ClusterMarker" />
        /// </summary>
        internal bool IsInitialized { get; private set; }

        /// <summary>
        /// <para>[DebugOnly]:  Original string from which this <see cref="ClusterMarker" /> indices are extracted</para>
        /// </summary>
        internal readonly string OriginalCompleteString;

#if DEBUG
        /// <summary>
        /// <para>[DebugOnly]:  SubString extracted from the original, based on indices in this <see cref="ClusterMarker" /></para>
        /// </summary>
        internal string SubStringCluster { get; private set; }
#endif

        internal ClusterMarker(string original)
        {
            ResetToInitState();
            OriginalCompleteString = original;
#if DEBUG
            SubStringCluster = original;
#endif
        }

        /// <summary>
        /// Reset this <see cref="ClusterMarker" /> to UnInitialized state
        /// </summary>
        internal void ResetToInitState()
        {
            ClusterStartIndex = ClusterEndIndex = UnInitializedClusterIndex;
            IsInitialized = false;
#if DEBUG
            SubStringCluster = string.Empty;
#endif
        }

        /// <summary>
        /// Assign start & end indices to this <see cref="ClusterMarker" />
        /// </summary>
        /// <param name="clusterStart">Start index of the SubString</param>
        /// <param name="clusterEnd">End index of the SubString</param>
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
