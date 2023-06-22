using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace InScrutable.Global
{
    internal static class Constants
    {
        internal static ImmutableHashSet<char> KnownVowels = ImmutableHashSet.Create('A', 'E', 'I', 'O', 'U', 'Y');

        internal static bool IsVowelOrY(this char alphabet)
        {
            var alphabetInUpperCase = char.ToUpper(alphabet, System.Globalization.CultureInfo.InvariantCulture);
            return KnownVowels.Any(vowel => vowel == alphabetInUpperCase);
        }

        internal static bool IsNotVowelOrY(this char alphabet)
        {
            return !alphabet.IsVowelOrY();
        }
    }
}
