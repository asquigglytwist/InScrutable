using System;
using System.Collections.Generic;
using System.Text;

namespace InScrutable
{
    /// <summary>
    /// A logical grouping of Extension (Helper) functions
    /// </summary>
    internal static class Extensions
    {
        #region Lexical Helpers
        /// <summary>
        /// Checks if given char <paramref name="alphabet" /> is a Vowel (or Y) - Case insensitive & Culture invariant
        /// See <seealso cref="DoesCharSoundAsAConsonant(char)" /> for the inverse of this check
        /// </summary>
        /// <param name="alphabet">Char to be checked</param>
        /// <returns>True, if Vowel or Y; False otherwise</returns>
        internal static bool DoesCharSoundAsAVowel(this char alphabet)
        {
            // var alphabetInUpperCase = char.ToUpper(alphabet, System.Globalization.CultureInfo.InvariantCulture);
            return Global.Constants.VowelSounds.Any(vowel => vowel == alphabet);
        }

        /// <summary>
        /// <para>Checks if given char <paramref name="alphabet" /> is NOT a Vowel (or Y) - Case insensitive & Culture invariant</para>
        /// See <seealso cref="DoesCharSoundAsAVowel(char)" /> for the inverse of this check
        /// </summary>
        /// <param name="alphabet">Char to be checked</param>
        /// <returns>True, if NOT a Vowel or Y; False otherwise</returns>
        internal static bool DoesCharSoundAsAConsonant(this char alphabet)
        {
            return !alphabet.DoesCharSoundAsAVowel();
        }
        #endregion

        #region Numeric Helpers
        /// <summary>
        /// <para>Converts the given <see cref="long" /> value as a <see cref="decimal" /> by an implicit cast</para>
        /// NOTE:  The effective numerical value does not change
        /// <example><value>1</value> becomes <value>1.0</value></example>
        /// </summary>
        /// <param name="valueInLong">The (input) value of type <see cref="long" /></param>
        /// <returns><see cref="decimal" /> form of the input <see cref="long" /></returns>
        internal static decimal AsDecimal(this long valueInLong) => valueInLong;
        #endregion
    }
}
