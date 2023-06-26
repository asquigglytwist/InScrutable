using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
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
            CheckCharInterest charInterestChecker = swapVowels ? Extensions.IsVowelOrY : Extensions.IsNotVowelOrY;
            ScramblerState scramblerState = ScramblerState.Append;
            StringBuilder sb = new(plainString.Length);
            ClusterMarker previousClusterOfInterest =
#if DEBUG
                new(plainString);
#else
        new();
#endif
            ClusterMarker currentClusterOfInterest =
#if DEBUG
                new(plainString);
#else
        new();
#endif
            int ixClusterOfInterestStart = 0;
            for (int iiCurrentIndex = 0; iiCurrentIndex < plainString.Length; iiCurrentIndex++)
            {
                var chCurrentChar = plainString[iiCurrentIndex];
                var bIsCharOfInterest = charInterestChecker(chCurrentChar);
                Debug.WriteLine($"Char {chCurrentChar} does {(bIsCharOfInterest ? string.Empty : "not ")}match criteria / interest check");
                if (!bIsCharOfInterest)
                {
                    switch (scramblerState)
                    {
                        case ScramblerState.Append:
                            sb.Append(chCurrentChar);
                            Debug.WriteLine("Appending char:  {0}", chCurrentChar);
                            break;
                        case ScramblerState.FirstClusterStart:
                            scramblerState = ScramblerState.FirstClusterEnd;
                            previousClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(First) Cluster marked:  {0}-{1}",
                                previousClusterOfInterest.ClusterStartIndex, previousClusterOfInterest.ClusterEndIndex);
                            break;
                        case ScramblerState.SecondClusterStart:
                            currentClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(Second) Cluster marked:  {0}-{1}",
                                currentClusterOfInterest.ClusterStartIndex, currentClusterOfInterest.ClusterEndIndex);
                            HandleMarkedClusters(plainString, ref previousClusterOfInterest, ref currentClusterOfInterest, sb, ref scramblerState);
                            sb.Append(chCurrentChar);
                            Debug.WriteLine($"After appending current char:  {sb}");
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
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (First) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case ScramblerState.FirstClusterEnd:
                            scramblerState = ScramblerState.SecondClusterStart;
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (Second) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case ScramblerState.FirstClusterStart:
                        case ScramblerState.SecondClusterStart:
                        default:
                            break;
                    }
                }
            }
            if (scramblerState == ScramblerState.SecondClusterStart)
            {
                currentClusterOfInterest.Assign(ixClusterOfInterestStart, plainString.Length - 1);
                HandleMarkedClusters(plainString, ref previousClusterOfInterest, ref currentClusterOfInterest, sb, ref scramblerState);
            }
            int residuesStartIndex = (previousClusterOfInterest.IsInitialized ? previousClusterOfInterest.ClusterStartIndex :
                (scramblerState != ScramblerState.Append ? ixClusterOfInterestStart : - 1));
            if (residuesStartIndex > -1)
            {
                Debug.WriteLine("Residues detected; Appending, for completeness");
                for (int iiCurrentIndex = residuesStartIndex; iiCurrentIndex < plainString.Length; iiCurrentIndex++)
                {
                    sb.Append(plainString[iiCurrentIndex]);
                }
            }
            Debug.WriteLine($"After final append:  {sb}");
            return sb.ToString();
        }

        private static void HandleMarkedClusters(string plainString, ref ClusterMarker firstClusterOfInterest, ref ClusterMarker secondClusterOfInterest, StringBuilder sb, ref ScramblerState scramblerState)
        {
            for (int jj = secondClusterOfInterest.ClusterStartIndex; jj <= secondClusterOfInterest.ClusterEndIndex; jj++)
            {
                sb.Append(plainString[jj]);
            }
            Debug.WriteLine($"After appending 2nd cluster:  {sb}");
            for (int jj = firstClusterOfInterest.ClusterEndIndex + 1; jj < secondClusterOfInterest.ClusterStartIndex; jj++)
            {
                sb.Append(plainString[jj]);
            }
            Debug.WriteLine($"After appending interim cluster:  {sb}");
            for (int jj = firstClusterOfInterest.ClusterStartIndex; jj <= firstClusterOfInterest.ClusterEndIndex; jj++)
            {
                sb.Append(plainString[jj]);
            }
            firstClusterOfInterest.ResetToInitState();
            secondClusterOfInterest.ResetToInitState();
            scramblerState = ScramblerState.Append;
            Debug.WriteLine($"After appending the two cluster:  {sb}");
        }
    }
}
