using InScrutable.Global;
using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    internal class U32B64 : IArgot
    {
        #region IArgot Implementation
        string IArgot.Obscure(string plainString)
        {
            return Convert.ToBase64String(Encoding.UTF32.GetBytes(plainString));
        }

        string IArgot.Reveal(string obscuredString)
        {
            return Encoding.UTF32.GetString(Convert.FromBase64String(obscuredString));
        }
        #endregion
    }
}
