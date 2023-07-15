using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    /// <summary>
    /// <para>Possible internal states of the <see cref="PhoneticSwap"/> variant of <see cref="IArgot"/></para>
    /// [DevRef]:  <see cref="PhoneticSwap" /> internally functions as a finite state machine - where state dictates the reaction to continuous parsing
    /// </summary>
    internal enum PhoneticSwapInternalState
    {
        /// <summary>
        /// Parser in "Append" mode where the inflow is directly appended to the final output
        /// </summary>
        Append,
        /// <summary>
        /// Discovered / encountered the "First" cluster that matches the "Phonetic" criteria
        /// </summary>
        FirstClusterStart,
        /// <summary>
        /// "First" cluster of interest has been completed
        /// </summary>
        FirstClusterEnd,
        /// <summary>
        /// Discovered / encountered the "Second" cluster that matches the "Phonetic" criteria
        /// </summary>
        SecondClusterStart,
        /// <summary>
        /// <para>"Second" cluster of interest has been completed</para>
        /// The <see cref="PhoneticSwap" /> parser performs the swap at this point
        /// </summary>
        SecondClusterEnd
    }

    /// <summary>
    /// <para>Argot generation / string mutation based on phonetically grouped character clusters</para>
    /// NOTE:  This is an Involution (https://en.wikipedia.org/wiki/Involution_(mathematics)) <see cref="IArgot" /> i.e., it is its own inverse.  Invoking <see cref="Obscure(string)" /> twice in succession as a chain results in the original PlainText  (Same as the infamous ROT13 cipher - https://en.wikipedia.org/wiki/ROT13)
    /// </summary>
    internal class PhoneticSwap : IArgot
    {
        /// <summary>
        /// Delegate to function (Predicate) that checks if given <see cref="char" /> meets expected criteria
        /// </summary>
        /// <param name="charOfInterest">The <see cref="char" /> to be checked</param>
        /// <returns>True, if criteria is met; False, otherwise</returns>
        public delegate bool CheckCharInterest(char charOfInterest);

        private readonly CheckCharInterest charInterestChecker;
        PhoneticSwapInternalState scramblerState;
        StringBuilder? sb;
        ClusterMarker firstClusterOfInterest, secondClusterOfInterest;

        internal PhoneticSwap(bool swapVowels = true)
        {
            scramblerState = PhoneticSwapInternalState.Append;
            Debug.WriteLine($"SwapVowels = {swapVowels}");
            charInterestChecker = swapVowels ? Extensions.IsVowelOrY : Extensions.IsNotVowelOrY;
        }

        /// <summary>
        /// Internal function to swap identified phonetic clusters
        /// </summary>
        /// <param name="inputString">Input string where the cluster swap is to be performed</param>
        /// <returns>The resulting string after the cluster swap is performed</returns>
        protected string SwapPhoneticClustersInternal(string inputString)
        {
            int ixClusterOfInterestStart = 0;
            Debug.WriteLine($"Input:  {inputString}");
            sb = new(inputString.Length);
#if DEBUG
            firstClusterOfInterest = new(inputString);
            secondClusterOfInterest = new(inputString);
#else
            firstClusterOfInterest = new();
            secondClusterOfInterest = new();
#endif
            for (int iiCurrentIndex = 0; iiCurrentIndex < inputString.Length; iiCurrentIndex++)
            {
                var chCurrentChar = inputString[iiCurrentIndex];
                var bIsCharOfInterest = charInterestChecker(chCurrentChar);
                Debug.WriteLine($"Char {chCurrentChar} does {(bIsCharOfInterest ? string.Empty : "not ")}match criteria / interest check");
                if (!bIsCharOfInterest)
                {
                    switch (scramblerState)
                    {
                        case PhoneticSwapInternalState.Append:
                            sb.Append(chCurrentChar);
                            Debug.WriteLine("Appending char:  {0}", chCurrentChar);
                            break;
                        case PhoneticSwapInternalState.FirstClusterStart:
                            scramblerState = PhoneticSwapInternalState.FirstClusterEnd;
                            firstClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(First) Cluster marked:  {0}-{1}",
                                firstClusterOfInterest.ClusterStartIndex, firstClusterOfInterest.ClusterEndIndex);
                            break;
                        case PhoneticSwapInternalState.SecondClusterStart:
                            secondClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(Second) Cluster marked:  {0}-{1}",
                                secondClusterOfInterest.ClusterStartIndex, secondClusterOfInterest.ClusterEndIndex);
                            HandleMarkedClusters(inputString);
                            sb.Append(chCurrentChar);
                            Debug.WriteLine($"After appending current char:  {sb}");
                            break;
                        case PhoneticSwapInternalState.FirstClusterEnd:
                        case PhoneticSwapInternalState.SecondClusterEnd:
                        default:
                            break;
                    }
                }
                else
                {
                    switch (scramblerState)
                    {
                        case PhoneticSwapInternalState.Append:
                            scramblerState = PhoneticSwapInternalState.FirstClusterStart;
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (First) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case PhoneticSwapInternalState.FirstClusterEnd:
                            scramblerState = PhoneticSwapInternalState.SecondClusterStart;
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (Second) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case PhoneticSwapInternalState.FirstClusterStart:
                        case PhoneticSwapInternalState.SecondClusterStart:
                        default:
                            break;
                    }
                }
            }
            if (scramblerState == PhoneticSwapInternalState.SecondClusterStart)
            {
                secondClusterOfInterest.Assign(ixClusterOfInterestStart, inputString.Length - 1);
                HandleMarkedClusters(inputString);
            }
            int residuesStartIndex = (firstClusterOfInterest.IsInitialized ? firstClusterOfInterest.ClusterStartIndex :
                (scramblerState != PhoneticSwapInternalState.Append ? ixClusterOfInterestStart : -1));
            if (residuesStartIndex > -1)
            {
                Debug.WriteLine("Residues detected; Appending, for completeness");
                for (int iiCurrentIndex = residuesStartIndex; iiCurrentIndex < inputString.Length; iiCurrentIndex++)
                {
                    sb.Append(inputString[iiCurrentIndex]);
                }
            }
            Debug.WriteLine($"After final append:  {sb}");
            return sb.ToString();
        }

        /// <summary>
        /// Internal function to handle the marked clusters
        /// </summary>
        /// <param name="inputString">Input string where the cluster swap is to be performed</param>
        protected void HandleMarkedClusters(string inputString)
        {
            if (sb != null)
            {
                for (int jj = secondClusterOfInterest.ClusterStartIndex; jj <= secondClusterOfInterest.ClusterEndIndex; jj++)
                {
                    sb.Append(inputString[jj]);
                }
                Debug.WriteLine($"After appending 2nd cluster:  {sb}");
                for (int jj = firstClusterOfInterest.ClusterEndIndex + 1; jj < secondClusterOfInterest.ClusterStartIndex; jj++)
                {
                    sb.Append(inputString[jj]);
                }
                Debug.WriteLine($"After appending interim cluster:  {sb}");
                for (int jj = firstClusterOfInterest.ClusterStartIndex; jj <= firstClusterOfInterest.ClusterEndIndex; jj++)
                {
                    sb.Append(inputString[jj]);
                }
                firstClusterOfInterest.ResetToInitState();
                secondClusterOfInterest.ResetToInitState();
                scramblerState = PhoneticSwapInternalState.Append;
                Debug.WriteLine($"After appending the two cluster:  {sb}");
            }
        }

        #region IArgot Implementation
        string IArgot.Obscure(string plainString)
        {
            return SwapPhoneticClustersInternal(plainString);
        }

        string IArgot.Reveal(string obscuredString)
        {
            return SwapPhoneticClustersInternal(obscuredString);
        }
        #endregion
    }
}
