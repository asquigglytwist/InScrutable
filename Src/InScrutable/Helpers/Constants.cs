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
        internal static ImmutableHashSet<char> KnownVowels = ImmutableHashSet.Create('A', 'E', 'I', 'O', 'U', 'Y');
        /// <summary>
        /// List (Set) of vowels from alphabets of (mutiple) language(s)
        /// </summary>
    }
}
