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
        /// <summary>
        /// Checks if given char <paramref name="alphabet" /> is a Vowel (or Y) - Case insensitive & Culture invariant
        /// See <seealso cref="IsNotVowelOrY(char)" /> for the inverse of this check
        /// </summary>
        /// <param name="alphabet">Char to be checked</param>
        /// <returns>True, if Vowel or Y; False otherwise</returns>
        internal static bool IsVowelOrY(this char alphabet)
        {
            // var alphabetInUpperCase = char.ToUpper(alphabet, System.Globalization.CultureInfo.InvariantCulture);
            return Global.Constants.VowelSounds.Any(vowel => vowel == alphabet);
        }

        /// <summary>
        /// <para>Checks if given char <paramref name="alphabet" /> is NOT a Vowel (or Y) - Case insensitive & Culture invariant</para>
        /// See <seealso cref="IsVowelOrY(char)" /> for the inverse of this check
        /// </summary>
        /// <param name="alphabet">Char to be checked</param>
        /// <returns>True, if NOT a Vowel or Y; False otherwise</returns>
        internal static bool IsNotVowelOrY(this char alphabet)
        {
            return !alphabet.IsVowelOrY();
        }
    }
}
