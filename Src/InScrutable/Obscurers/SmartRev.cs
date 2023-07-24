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
        internal SmartRev()
        {
        }

        protected string SmartReverseInternal(string inputString)
        {
            return inputString;
        }

        #region IArgot Implementation
        string IArgot.Obscure(string plainString)
        {
            return SmartReverseInternal(plainString);
        }

        string IArgot.Reveal(string obscuredString)
        {
            return SmartReverseInternal(obscuredString);
        }
        #endregion
    }
}
