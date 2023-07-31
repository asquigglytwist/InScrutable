using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace InScrutable.Helpers
{
    internal static class TimeKeeper
    {
        static readonly Dictionary<string, Stopwatch> namedTimers;

        static TimeKeeper()
        {
            namedTimers = new Dictionary<string, Stopwatch>();
        }

        internal static Stopwatch GetTimerFor(string timerName)
        {
            if (!namedTimers.TryGetValue(timerName, out var timer))
            {
                Debug.WriteLine($"Timer not available for {timerName} among the known {namedTimers.Count} timers; Creating anew");
                timer = new Stopwatch();
                namedTimers[timerName] = timer;
            }
            return timer;
        }

        internal static void StartTimer(string timerName)
        {
            var stopWatch = GetTimerFor(timerName);
            stopWatch.Start();
        }

        internal static long StopTimer(string timerName)
        {
            var stopWatch = GetTimerFor(timerName);
            stopWatch.Stop();
            return stopWatch.ElapsedTicks;
        }

        public static bool IsHighResolution { get => Stopwatch.IsHighResolution; }
    }

}
