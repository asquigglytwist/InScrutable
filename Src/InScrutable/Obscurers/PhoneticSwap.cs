using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    /// <summary>
    /// <para>Argot generation / string mutation based on phonetically grouped character clusters</para>
    /// </summary>
    /// <remarks>This is an Involution (<seealso href="https://en.wikipedia.org/wiki/Involution_(mathematics)" />) <see cref="IArgot" /> i.e., it is its own inverse.  Invoking <see cref="Obscure(string)" /> twice in succession as a chain results in the original PlainText  (Same as the infamous ROT13 cipher - <seealso href="https://en.wikipedia.org/wiki/ROT13" />)</remarks>
    internal class PhoneticSwap : IArgot
    {
        /// <summary>
        /// Delegate to function (Predicate) that checks if given <see cref="char" /> meets expected criteria
        /// </summary>
        /// <param name="charOfInterest">The <see cref="char" /> to be checked</param>
        /// <returns>True, if criteria is met; False, otherwise</returns>
        public delegate bool CheckCharInterest(char charOfInterest);

        private readonly CheckCharInterest charInterestChecker;
        ClusterHandlerInternalState scramblerState;
        StringBuilder? sb;
        ClusterMarker firstClusterOfInterest, secondClusterOfInterest;

        internal PhoneticSwap(bool swapVowels = true)
        {
            scramblerState = ClusterHandlerInternalState.Append;
            Debug.WriteLine($"SwapVowels = {swapVowels}");
            charInterestChecker = swapVowels ? Extensions.DoesCharSoundAsAVowel : Extensions.DoesCharSoundAsAConsonant;
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
            firstClusterOfInterest = new(inputString);
            secondClusterOfInterest = new(inputString);
            for (int iiCurrentIndex = 0; iiCurrentIndex < inputString.Length; iiCurrentIndex++)
            {
                var chCurrentChar = inputString[iiCurrentIndex];
                var bIsCharOfInterest = charInterestChecker(chCurrentChar);
                Debug.WriteLine($"Char {chCurrentChar} does {(bIsCharOfInterest ? string.Empty : "not ")}match criteria / interest check");
                if (!bIsCharOfInterest)
                {
                    switch (scramblerState)
                    {
                        case ClusterHandlerInternalState.Append:
                            sb.Append(chCurrentChar);
                            Debug.WriteLine("Appending char:  {0}", chCurrentChar);
                            break;
                        case ClusterHandlerInternalState.FirstClusterStart:
                            scramblerState = ClusterHandlerInternalState.FirstClusterEnd;
                            firstClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(First) Cluster marked:  {0}-{1}",
                                firstClusterOfInterest.ClusterStartIndex, firstClusterOfInterest.ClusterEndIndex);
                            break;
                        case ClusterHandlerInternalState.SecondClusterStart:
                            secondClusterOfInterest.Assign(ixClusterOfInterestStart, iiCurrentIndex - 1);
                            Debug.WriteLine("(Second) Cluster marked:  {0}-{1}",
                                secondClusterOfInterest.ClusterStartIndex, secondClusterOfInterest.ClusterEndIndex);
                            HandleMarkedClusters(inputString);
                            sb.Append(chCurrentChar);
                            Debug.WriteLine($"After appending current char:  {sb}");
                            break;
                        case ClusterHandlerInternalState.FirstClusterEnd:
                        case ClusterHandlerInternalState.SecondClusterEnd:
                        default:
                            break;
                    }
                }
                else
                {
                    switch (scramblerState)
                    {
                        case ClusterHandlerInternalState.Append:
                            scramblerState = ClusterHandlerInternalState.FirstClusterStart;
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (First) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case ClusterHandlerInternalState.FirstClusterEnd:
                            scramblerState = ClusterHandlerInternalState.SecondClusterStart;
                            ixClusterOfInterestStart = iiCurrentIndex;
                            Debug.WriteLine($"Detected (Second) char of interst {chCurrentChar} at {ixClusterOfInterestStart}");
                            break;
                        case ClusterHandlerInternalState.FirstClusterStart:
                        case ClusterHandlerInternalState.SecondClusterStart:
                        default:
                            break;
                    }
                }
            }
            if (scramblerState == ClusterHandlerInternalState.SecondClusterStart)
            {
                secondClusterOfInterest.Assign(ixClusterOfInterestStart, inputString.Length - 1);
                HandleMarkedClusters(inputString);
            }
            int residuesStartIndex = (firstClusterOfInterest.IsInitialized ? firstClusterOfInterest.ClusterStartIndex :
                (scramblerState != ClusterHandlerInternalState.Append ? ixClusterOfInterestStart : -1));
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
                scramblerState = ClusterHandlerInternalState.Append;
                Debug.WriteLine($"After appending the two clusters:  {sb}");
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
