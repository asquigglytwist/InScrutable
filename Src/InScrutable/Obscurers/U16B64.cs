using InScrutable.Global;
using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    /// <summary>
    /// <para>Argot generation / string mutation by converting to Base64 (UTF-16) Encoding</para>
    /// </summary>
    internal class U16B64 : IArgot
    {
        #region IArgot Implementation
        string IArgot.Obscure(string plainString)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(plainString));
        }

        string IArgot.Reveal(string obscuredString)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(obscuredString));
        }
        #endregion
    }
}
