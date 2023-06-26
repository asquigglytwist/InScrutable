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

    internal class PhoneticSwap : IArgot
    {
        public delegate bool CheckCharInterest(char charOfInterest);

        private CheckCharInterest charInterestChecker;
        ScramblerState scramblerState;
        StringBuilder? sb;
        ClusterMarker firstClusterOfInterest, secondClusterOfInterest;

        internal PhoneticSwap(bool swapVowels = true)
        {
            scramblerState = ScramblerState.Append;
            Debug.WriteLine($"SwapVowels = {swapVowels}");
            charInterestChecker = swapVowels ? Extensions.IsVowelOrY : Extensions.IsNotVowelOrY;
        }

        public string Obscure(string plainString)
        {
            int ixClusterOfInterestStart = 0;
            Debug.WriteLine($"Input:  {plainString}");
            sb = new(plainString.Length);
#if DEBUG
            firstClusterOfInterest = new(plainString);
            secondClusterOfInterest = new(plainString);
#else
            currentClusterOfInterest = new();
            previousClusterOfInterest = new();
#endif
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
                            firstClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(First) Cluster marked:  {0}-{1}",
                                firstClusterOfInterest.ClusterStartIndex, firstClusterOfInterest.ClusterEndIndex);
                            break;
                        case ScramblerState.SecondClusterStart:
                            secondClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(Second) Cluster marked:  {0}-{1}",
                                secondClusterOfInterest.ClusterStartIndex, secondClusterOfInterest.ClusterEndIndex);
                            HandleMarkedClusters(plainString);
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
                secondClusterOfInterest.Assign(ixClusterOfInterestStart, plainString.Length - 1);
                HandleMarkedClusters(plainString);
            }
            int residuesStartIndex = (firstClusterOfInterest.IsInitialized ? firstClusterOfInterest.ClusterStartIndex :
                (scramblerState != ScramblerState.Append ? ixClusterOfInterestStart : -1));
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

        private void HandleMarkedClusters(string plainString)
        {
            if (sb != null)
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

        #region IArgot
        string IArgot.Obscure(string plainString)
        {
            return Obscure(plainString);
        }

        string IArgot.Reveal(string obscuredString)
        {
            return Obscure(obscuredString);
        }
        #endregion
    }
}
