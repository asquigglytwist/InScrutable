using InScrutable.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable
{
    internal struct ClusterMarker
    {
        private const int UnInitializedIndex = -1;

        internal int ClusterStartIndex { get; private set; }

        internal int ClusterEndIndex { get; private set; }

        internal bool IsInitialized { get; private set; }

        public ClusterMarker()
        {
            ResetToInitState();
        }

        internal ClusterMarker(int clusterStart, int clusterEnd)
        {
            Assign(clusterStart, clusterEnd);
        }

        internal void ResetToInitState()
        {
            ClusterStartIndex = ClusterEndIndex = UnInitializedIndex;
            IsInitialized = false;
        }

        internal void Assign(int clusterStart, int clusterEnd)
        {
            ClusterStartIndex = clusterStart;
            ClusterEndIndex = clusterEnd;
            IsInitialized = true;
        }
    }

    internal enum ScramblerState
    {
        Append,
        FirstClusterStart,
        FirstClusterEnd,
        SecondClusterStart,
        SecondClusterEnd
    }

    internal static class Argot
    {
        public delegate bool CheckCharInterest(char charOfInterest);

        internal static string PhoneticSwap(string plainString, bool swapVowels = true)
        {
            CheckCharInterest charInterestChecker = swapVowels ? Constants.IsVowelOrY : Constants.IsNotVowelOrY;
            ScramblerState scramblerState = ScramblerState.Append;
            StringBuilder sb = new(plainString.Length);
            ClusterMarker previousVowelCluster = new();
            ClusterMarker currentVowelCluster = new();

            int ixVowelClusterStart = 0;
            for (int iiCurrentIndex = 0; iiCurrentIndex < plainString.Length; iiCurrentIndex++)
            {
                var chCurrentChar = plainString[iiCurrentIndex];
                var bIsCharAVowelOrY = charInterestChecker(chCurrentChar);
                Debug.WriteLine("Char {0} is {1}a Vowel", chCurrentChar, bIsCharAVowelOrY ? "" : "not ");
                if (!bIsCharAVowelOrY)
                {
                    switch (scramblerState)
                    {
                        case ScramblerState.Append:
                            sb.Append(chCurrentChar);
#if DEBUG
                            Debug.WriteLine("After append:  {0}", sb.ToString());
#endif
                            break;
                        case ScramblerState.FirstClusterStart:
                            scramblerState = ScramblerState.FirstClusterEnd;
                            previousVowelCluster.Assign(ixVowelClusterStart, iiCurrentIndex - 1);
                            break;
                        case ScramblerState.SecondClusterStart:
                            // scramblerState = ScramblerState.SecondClusterEnd;
                            currentVowelCluster.Assign(ixVowelClusterStart, iiCurrentIndex - 1);
                            // TODO:  Complete all append ops
                            for (int jj = currentVowelCluster.ClusterStartIndex; jj <= currentVowelCluster.ClusterEndIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            for (int jj = previousVowelCluster.ClusterEndIndex + 1; jj < currentVowelCluster.ClusterStartIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            for (int jj = previousVowelCluster.ClusterStartIndex; jj <= previousVowelCluster.ClusterEndIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            previousVowelCluster.ResetToInitState();
                            scramblerState = ScramblerState.Append;
                            break;
                        case ScramblerState.FirstClusterEnd:
                        case ScramblerState.SecondClusterEnd:
                        default:
                            break;
                    }
                }
                else
                {
                    switch (scramblerState)
                    {
                        case ScramblerState.Append:
                            scramblerState = ScramblerState.FirstClusterStart;
                            ixVowelClusterStart = iiCurrentIndex;
                            break;
                        case ScramblerState.FirstClusterEnd:
                            scramblerState = ScramblerState.SecondClusterStart;
                            ixVowelClusterStart = iiCurrentIndex;
                            break;
                        case ScramblerState.FirstClusterStart:
                        case ScramblerState.SecondClusterStart:
                        default:
                            break;
                    }
                }
                if (previousVowelCluster.IsInitialized)
                {
                    for (iiCurrentIndex = previousVowelCluster.ClusterStartIndex; iiCurrentIndex < plainString.Length; iiCurrentIndex++)
                    {
                        sb.Append(plainString[iiCurrentIndex]);
                    }
                }
            }
            return sb.ToString();
        }
    }
}
