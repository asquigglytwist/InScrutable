using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace InScrutable.Global
{
    internal static class Constants
    {
        internal static ImmutableHashSet<char> KnownVowels = ImmutableHashSet.Create('A', 'E', 'I', 'O', 'U', 'Y');
    }
}
