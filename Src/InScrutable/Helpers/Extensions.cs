using System;
using System.Collections.Generic;
using System.Text;

namespace InScrutable
{
    internal static class Extensions
    {
        internal static bool IsVowelOrY(this char alphabet)
        {
            var alphabetInUpperCase = char.ToUpper(alphabet, System.Globalization.CultureInfo.InvariantCulture);
            return Global.Constants.KnownVowels.Any(vowel => vowel == alphabetInUpperCase);
        }

        internal static bool IsNotVowelOrY(this char alphabet)
        {
            return !alphabet.IsVowelOrY();
        }
    }
}
