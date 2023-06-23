namespace InScrutable.Test
{
    [TestClass]
    public class ArgotTests
    {
        [TestMethod]
        public void PhoneticSwap_Vowels()
        {
            (string inputPlain, string outputScrambled)[] testCombos_RemainUnchanged_Short = new[] {
                (string.Empty, string.Empty),
                ("A", "A"),
                ("e", "e"),
                ("Z", "Z"),
                ("x", "x"),
                ("Y", "Y"),
                ("y", "y"),
                ("Aa", "Aa"),
                ("Yy", "Yy")
            };
            (string inputPlain, string outputScrambled)[] testCombos_RemainUnchanged_Long = new[] {
                ("AEIOUYaeiouy", "AEIOUYaeiouy"),
                ("aBCDFGHJKLMNPQRSTVWXZ", "aBCDFGHJKLMNPQRSTVWXZ"),
            };
            (string inputPlain, string outputScrambled)[] testCombos_TwoVowelClusters = new[] {
                ("Abecdfiogh", "ebAcdfiogh"),
                ("AebcdifOgh", "ibcdAefOgh")
            };
            (string inputPlain, string outputScrambled)[] testCombos_LongerVowelClusters = new[] {
                ("AebcdifOghuyujklm", "ibcdAefuyughOjklm")
            };
            PhoneticSwap_TestList(testCombos_RemainUnchanged_Short);
            PhoneticSwap_TestList(testCombos_RemainUnchanged_Long);
            PhoneticSwap_TestList(testCombos_TwoVowelClusters);
            PhoneticSwap_TestList(testCombos_LongerVowelClusters);

            static void PhoneticSwap_TestList((string inputPlain, string outputScrambled)[] testCombos)
            {
                foreach (var scramblerTestCombo in testCombos)
                {
                    var inputPlain = scramblerTestCombo.inputPlain;
                    var outputExpected = scramblerTestCombo.outputScrambled;
                    var outputActuals = Argot.PhoneticSwap(inputPlain);
                    Assert.AreEqual(inputPlain.Length, outputExpected.Length);
                    Assert.AreEqual(inputPlain.Length, outputActuals.Length);
                    //Assert.AreNotEqual(inputPlain, outputExpected);
                    Assert.AreEqual(outputExpected, outputActuals);
                }
            }
        }
    }
}