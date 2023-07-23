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
        /// <para>List (Set) of vowels from alphabets of (mutiple) language(s)</para>
        /// NOTE:  This collection includes the alphabets that are classified as vowels and also those sound as one
        /// </summary>
        internal static ImmutableHashSet<char> VowelSounds = ImmutableHashSet.Create(
            // EN
            'A', 'E', 'I', 'O', 'U', 'Y'
            , 'a', 'e', 'i', 'o', 'u', 'y'

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
            , 'Ä', 'Ö', 'Ü'
            , 'ä', 'ö', 'ü'
            // FR
            , 'Â', 'À', 'È', 'É', 'Ê', 'Ë', 'Î', 'Ô', 'Ù', 'Û', 'Ü'
            , 'â', 'à', 'è', 'é', 'ê', 'ë', 'î', 'ô', 'ù', 'û', 'ü'
            );
    }
}
