using System;
using System.Collections.Generic;
using System.Text;

namespace InScrutable.Helpers
{
    /// <summary>
    /// <para>Possible internal states of the <see cref="PhoneticSwap"/> variant of <see cref="IArgot"/></para>
    /// [DevRef]:  <see cref="PhoneticSwap" /> internally functions as a finite state machine - where state dictates the reaction to continuous parsing
    /// </summary>
    internal enum PhoneticSwapInternalState
    {
        /// <summary>
        /// Parser in "Append" mode where the inflow is directly appended to the final output
        /// </summary>
        Append,
        /// <summary>
        /// Discovered / encountered the "First" cluster that matches the "Phonetic" criteria
        /// </summary>
        FirstClusterStart,
        /// <summary>
        /// "First" cluster of interest has been completed
        /// </summary>
        FirstClusterEnd,
        /// <summary>
        /// Discovered / encountered the "Second" cluster that matches the "Phonetic" criteria
        /// </summary>
        SecondClusterStart,
        /// <summary>
        /// <para>"Second" cluster of interest has been completed</para>
        /// The <see cref="PhoneticSwap" /> parser performs the swap at this point
        /// </summary>
        SecondClusterEnd
    }

}
