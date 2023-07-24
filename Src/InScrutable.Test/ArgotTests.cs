using InScrutable.Obscurers;
using System.Diagnostics;

namespace InScrutable.Test
{
    [TestClass]
    public class PhoneticSwapTests
    {
        static void TestHelper_PhoneticSwap_HandleInputList((string inputPlain, string outputVowelMode, string outputConsonantMode)[] testCombos)
        {
            foreach (var scramblerTestCombo in testCombos)
            {
                var inputPlain = scramblerTestCombo.inputPlain;
                var outputExpected_VowelMode = scramblerTestCombo.outputVowelMode;
                var outputExpected_ConsonantMode = scramblerTestCombo.outputConsonantMode;
                IArgot phSwap_VowelMode = new PhoneticSwap();
                IArgot phSwap_ConsonantMode = new PhoneticSwap(false);
                var outputActuals_VowelMode = phSwap_VowelMode.Obscure(inputPlain);
                var outputActuals_ConsonantMode = phSwap_ConsonantMode.Obscure(inputPlain);
                Debug.WriteLine($"[TEST]  Input = {inputPlain}    Actuals_VowelMode = {outputActuals_VowelMode}    Actuals_ConsonantMode = {outputActuals_ConsonantMode}");
                Assert.AreEqual(inputPlain.Length, outputExpected_VowelMode.Length);
                Assert.AreEqual(inputPlain.Length, outputActuals_VowelMode.Length);
                Assert.AreEqual(inputPlain.Length, outputActuals_ConsonantMode.Length);
                if (inputPlain != outputExpected_VowelMode)
                {
                    Assert.AreNotEqual(inputPlain, outputActuals_VowelMode);
                    Assert.AreNotEqual(inputPlain, outputActuals_ConsonantMode);
                }
                Assert.AreEqual(outputExpected_VowelMode, outputActuals_VowelMode);
                Assert.AreEqual(outputExpected_ConsonantMode, outputActuals_ConsonantMode);
            }
        }

        [TestMethod]
        public void PhoneticSwap_RemainUnchanged_Short()
        {
            (string, string, string)[] testCombos_RemainUnchanged_Short = new[] {
                (string.Empty, string.Empty, string.Empty),
                ("A", "A", "A"),
                ("e", "e", "e"),
                ("Z", "Z", "Z"),
                ("x", "x", "x"),
                ("Y", "Y", "Y"),
                ("y", "y", "y"),
                ("Aa", "Aa", "Aa"),
                ("Yy", "Yy", "Yy"),
                ("BbC", "BbC", "BbC")
            };
            TestHelper_PhoneticSwap_HandleInputList(testCombos_RemainUnchanged_Short);
        }

        [TestMethod]
        public void PhoneticSwap_RemainUnchanged_Long()
        {
            (string, string, string)[] testCombos_RemainUnchanged_Long = new[] {
                ("AEIOUYaeiouy", "AEIOUYaeiouy", "AEIOUYaeiouy"),
                ("aBCDFGHJKLMNPQRSTVWXZ", "aBCDFGHJKLMNPQRSTVWXZ", "aBCDFGHJKLMNPQRSTVWXZ"),
            };
            TestHelper_PhoneticSwap_HandleInputList(testCombos_RemainUnchanged_Long);
        }

        [TestMethod]
        public void PhoneticSwap_TwoVowelClusters()
        {
            (string, string, string)[] testCombos_TwoVowelClusters = new[] {
                ("Abecdfiogh", "ebAcdfiogh", "Acdfebiogh"),
                ("AebcdifOgh", "ibcdAefOgh", "AefibcdOgh")
            };
            TestHelper_PhoneticSwap_HandleInputList(testCombos_TwoVowelClusters);
        }

        [TestMethod]
        public void PhoneticSwap_TwoConsonantClusters()
        {
            (string, string, string)[] testCombos_TwoConsonantClusters = new[] {
                ("CaT", "CaT", "TaC"),
                ("CaTch", "CaTch", "TchaC"),
                ("ChaRt", "ChaRt", "RtaCh")
            };
            TestHelper_PhoneticSwap_HandleInputList(testCombos_TwoConsonantClusters);
        }

        [TestMethod]
        public void PhoneticSwap_LongerVowelClusters()
        {
            (string, string, string)[] testCombos_LongerVowelClusters = new[] {
                ("AebcdifOghuyujklm", "ibcdAefuyughOjklm", "AefibcdOjklmuyugh")
            };
            TestHelper_PhoneticSwap_HandleInputList(testCombos_LongerVowelClusters);
        }
    }

    [TestClass]
    public class SmartRevTests
    {
        [TestMethod]
        public void SmartRevTest()
        {
            var xyz = new[] {
                ("This is a string", "sihT si a gnirts")
                , ("This is a test", "sihT si a tset")
            };
            IArgot srObj = new SmartRev();
            foreach (var (input, expected) in xyz)
            {
                var actuals = srObj.Obscure(input);
                Assert.AreEqual(expected, actuals);
            }
        }
    }
}