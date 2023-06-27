using InScrutable.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Obscurers
{
    internal interface IArgot
    {
        public string Obscure(string plainString);

        public string Reveal(string obscuredString);
    }
}
