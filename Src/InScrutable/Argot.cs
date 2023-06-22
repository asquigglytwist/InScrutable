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

        internal static string PhoneticSwap(string plainString, in bool swapVowels = true)
        {
            Debug.WriteLine($"SwapVowels = {swapVowels}\tInput:\n{plainString}");
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
                Debug.WriteLine($"Char {chCurrentChar} is {0}a Vowel", bIsCharAVowelOrY ? "" : "not ");
                if (!bIsCharAVowelOrY)
                {
                    switch (scramblerState)
                    {
                        case ScramblerState.Append:
                            sb.Append(chCurrentChar);
                            Debug.WriteLine("Appending char:  {0}", chCurrentChar);
                            break;
                        case ScramblerState.FirstClusterStart:
                            scramblerState = ScramblerState.FirstClusterEnd;
                            previousVowelCluster.Assign(ixVowelClusterStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(First) Cluster marked:  {0}-{1}",
                                previousVowelCluster.ClusterStartIndex, previousVowelCluster.ClusterEndIndex);
                            break;
                        case ScramblerState.SecondClusterStart:
                            currentVowelCluster.Assign(ixVowelClusterStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(Second) Cluster marked:  {0}-{1}",
                                currentVowelCluster.ClusterStartIndex, currentVowelCluster.ClusterEndIndex);
                            for (int jj = currentVowelCluster.ClusterStartIndex; jj <= currentVowelCluster.ClusterEndIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            Debug.WriteLine($"After appending 2nd cluster:  {sb}");
                            for (int jj = previousVowelCluster.ClusterEndIndex + 1; jj < currentVowelCluster.ClusterStartIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            Debug.WriteLine($"After appending interim cluster:  {sb}");
                            for (int jj = previousVowelCluster.ClusterStartIndex; jj <= previousVowelCluster.ClusterEndIndex; jj++)
                            {
                                sb.Append(plainString[jj]);
                            }
                            Debug.WriteLine($"After appending first cluster:  {sb}");
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
                            Debug.WriteLine($"Detected (First) char of interst {chCurrentChar} at {ixVowelClusterStart}");
                            break;
                        case ScramblerState.FirstClusterEnd:
                            scramblerState = ScramblerState.SecondClusterStart;
                            ixVowelClusterStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (Second) char of interst {chCurrentChar} at {ixVowelClusterStart}");
                            break;
                        case ScramblerState.FirstClusterStart:
                        case ScramblerState.SecondClusterStart:
                        default:
                            break;
                    }
                }
                if (previousVowelCluster.IsInitialized)
                {
                    Debug.WriteLine("Residues detected; Appending for completeness");
                    for (iiCurrentIndex = previousVowelCluster.ClusterStartIndex; iiCurrentIndex < plainString.Length; iiCurrentIndex++)
                    {
                        sb.Append(plainString[iiCurrentIndex]);
                    }
                    Debug.WriteLine($"After final append:  {sb}");
                }
                Debug.Assert(plainString.Length == iiCurrentIndex);
            }
            return sb.ToString();
        }
    }
}
