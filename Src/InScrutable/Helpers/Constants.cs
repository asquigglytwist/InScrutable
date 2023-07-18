using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace InScrutable.Global
{
    /// <summary>
    /// <para>All Constant values (that can be hard-coded)</para>
    /// Typically, all baseline assumptions / immutable values
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// List (Set) of vowels from alphabets of (mutiple) language(s)
        /// </summary>
        internal static ImmutableHashSet<char> KnownVowels = ImmutableHashSet.Create(
            // EN
            'A', 'E', 'I', 'O', 'U', 'Y'

            // Indic Langs
            // TA (Independent)
            , 'அ', 'ஆ', 'இ', 'ஈ', 'உ', 'ஊ', 'எ', 'ஏ', 'ஐ', 'ஒ', 'ஓ', 'ஔ'
            // ML
            , 'അ', 'ആ', 'ഇ', 'ഈ', 'ഉ', 'ഉ', 'ഋ', 'ൠ', 'ഌ', 'ൡ', 'എ', 'ഏ', 'ഒ', 'ഓ', 'ഐ', 'ഔ'
            // TE
            , 'అ', 'ఆ', 'ఇ', 'ఈ', 'ఉ', 'ఊ', 'ఋ', 'ఌ', 'ఎ', 'ఏ', 'ఐ', 'ఒ', 'ఓ', 'ఔ'
            // SA
            , 'अ', 'आ', 'इ', 'ई', 'उ', 'ऊ', 'ऋ', 'ॠ', 'ऌ', 'ॡ', 'ए', 'ऐ', 'ओ', 'औ'

            // EU Langs
            // GE
            , 'ä', 'ö', 'ü'
            // FR
            , 'â', 'à', 'è', 'ê', 'û', 'î'
            );
    }
}
