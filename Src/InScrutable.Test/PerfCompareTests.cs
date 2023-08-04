using InScrutable.Helpers;
using InScrutable.Obscurers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmartRevTestFunc = System.Func<InScrutable.Obscurers.SmartRev, string, string>;

namespace InScrutable.Test
{
    [TestClass]
    internal class PerfCompareTests
    {
        static readonly string[] testValues =
        {
            "abcdefghijklmnopqrstuvwxyz"
            , "abcd efgh ijkl mnop qrst uvwx yz"
            , "ab cd ef gh ij kl mn op qr st uv wx yz"
            , "a b c d e f g h i j k l m n o p q r s t u v w x y z"
            , "abc def ghi jkl mno pqr stu vwx yz"
            , "abcdef ghijkl mnopqr stuvwx yz"
            , "abcdefghijkl mnopqrstuvwx yz"
            , "abcdefghijklm nopqrstuvwxyz"
        };

        const int kIterationsPerRun = 2000;

        [TestMethod]
        public static void RunTests()
        {
            const string kTimerNormal = "Normal";
            const string kTimerAlt = "Alt";
            SmartRevTestFunc normalRoute = (sr, plainText) => sr.SmartReverseInternal(plainText);
            SmartRevTestFunc altRoute = (sr, plainText) => sr.SmartReverseInternal_Alt(plainText);
            TimeKeeper.StartTimer(kTimerNormal);
            SmartReverseTestRepeater(normalRoute);
            var elapsedNormal = TimeKeeper.StopTimer(kTimerNormal);
            TimeKeeper.StartTimer(kTimerAlt);
            SmartReverseTestRepeater(altRoute);
            var elapsedAlt = TimeKeeper.StopTimer(kTimerAlt);
            Console.WriteLine($"Elapsed - Normal: {elapsedNormal}\tAlt: {elapsedAlt}");
            if (elapsedNormal < elapsedAlt)
            {
                Console.WriteLine($"Normal approach is {elapsedAlt.AsDecimal() / elapsedNormal.AsDecimal()}x faster");
            }
            else
            {
                Console.WriteLine($"Alt approach is {elapsedNormal.AsDecimal() / elapsedAlt.AsDecimal()}x faster");
            }
        }

        private static void SmartReverseTestRepeater(SmartRevTestFunc smartRevFunctor)
        {
            for (int iiRepeatCounter = 0; iiRepeatCounter < kIterationsPerRun; iiRepeatCounter++)
            {
                Console.WriteLine($"Iteration:  {iiRepeatCounter}");
                SmartRev sr = new();
                for (int iiTestSetIterator = 0; iiTestSetIterator < testValues.Length; iiTestSetIterator++)
                {
                    Console.WriteLine($"Input:  {testValues[iiTestSetIterator]}\tOutput:  {smartRevFunctor(sr, testValues[iiTestSetIterator])}");
                }
            }
        }
    }
}
