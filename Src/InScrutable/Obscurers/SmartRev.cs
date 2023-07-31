using InScrutable.Global;
using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    /// <summary>
    /// <para>Argot generation / string mutation by reversing words within a sentence without affecting their ordinal position</para>
    /// NOTE:  This is an Involution (https://en.wikipedia.org/wiki/Involution_(mathematics)) <see cref="IArgot" /> i.e., it is its own inverse.  Invoking <see cref="Obscure(string)" /> twice in succession as a chain results in the original PlainText  (Same as the infamous ROT13 cipher - https://en.wikipedia.org/wiki/ROT13)
    /// </summary>
    internal class SmartRev : IArgot
    {
        StringBuilder? sb;

        internal SmartRev()
        {
        }

        protected string SmartReverseInternal(string inputString)
        {
            sb = new StringBuilder(inputString.Length);
            var wordsInString = inputString.Split(Constants.SingleSpace);
            foreach (var word in wordsInString)
            {
                var wordAsCharArray = word.ToCharArray();
                Array.Reverse(wordAsCharArray);
                sb.Append(wordAsCharArray).Append(Constants.SingleSpace);
                Debug.WriteLine($"Word:  {word}\tReverse:  {wordAsCharArray}");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            Debug.WriteLine($"After reversal as intended:  {sb}");
            return sb.ToString();
        }

        protected string SmartReverseInternal_Alt(string inputString)
        {
            sb = new StringBuilder(inputString.Length);
            var asCharArray = inputString.ToCharArray();
            var wordStartIndex = 0;
            Debug.WriteLine($"Input:  {inputString}\tLength:  {inputString.Length}\tArrayLength:  {asCharArray.Length}");
            for (var iiArrayIndex = 0; iiArrayIndex < asCharArray.Length; iiArrayIndex++)
            {
                var charInString = asCharArray[iiArrayIndex];
                Debug.WriteLine($"Current char:  {charInString}");
                if (!char.IsLetterOrDigit(charInString))
                {
                    Debug.WriteLine("Char is not a letter or digit; Treating as word-break and proceeding with appending in reverse");
                    for (int iiClusterIndex = iiArrayIndex - 1; iiClusterIndex >= wordStartIndex; iiClusterIndex--)
                    {
                        sb.Append(asCharArray[iiClusterIndex]);
                    }
                    sb.Append(charInString);
                    wordStartIndex = iiArrayIndex + 1;
                }
            }
            for (int iiClusterIndex = asCharArray.Length - 1; iiClusterIndex >= wordStartIndex; iiClusterIndex--)
            {
                Debug.WriteLine($"Appending:  {asCharArray[iiClusterIndex]}");
                sb.Append(asCharArray[iiClusterIndex]);
            }
            Debug.WriteLine($"After reversal as intended:  {sb}");
            return sb.ToString();
        }

        #region IArgot Implementation
        string IArgot.Obscure(string plainString)
        {
            return SmartReverseInternal_Alt(plainString);
        }

        string IArgot.Reveal(string obscuredString)
        {
            return SmartReverseInternal(obscuredString);
        }
        #endregion
    }
}
