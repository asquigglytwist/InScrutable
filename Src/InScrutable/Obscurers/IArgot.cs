using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    /// <summary>
    /// <para>A (Cryptographically-InSecure) string transformation while retaining similarities to the original text.</para>
    /// Intent of usage is NOT a Cryptographically Secure Encryption but to computationally generate "Argot"-ized mutation of the input PlainText, resulting in an <see cref="Obscure(string)" />-d form
    /// </summary>
    internal interface IArgot
    {
        /// <summary>
        /// <para>Modify (mutate) the input PlainText, resulting in the Obscured (<see cref="IArgot" />-ized) form</para>
        /// See <seealso cref="Reveal(string)" /> - for the reversal
        /// </summary>
        /// <param name="plainString">The input PlainText to be Obscured</param>
        /// <returns>The Obscured (<see cref="IArgot" />-ized) form</returns>
        public string Obscure(string plainString);

        /// <summary>
        /// <para>Reverses the applied obscurity (mutation) to retrieve the original PlainText</para>
        /// See <seealso cref="Obscure(string)" /> - for the initial mutation
        /// </summary>
        /// <param name="obscuredString">The Obscured (<see cref="IArgot" />-ized) form</param>
        /// <returns>The original PlainText prior to being obscured</returns>
        public string Reveal(string obscuredString);
    }
}
