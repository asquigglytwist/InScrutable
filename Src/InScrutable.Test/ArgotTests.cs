namespace InScrutable.Test
{
    [TestClass]
    public class ArgotTests
    {
        [TestMethod]
        public void ScramblerTest()
        {
            (string inputPlain, string outputScrambled)[] combosForTest = new [] {
                (string.Empty, string.Empty),
                ("A", "A"),
                ("e", "e"),
                ("Z", "Z"),
                ("x", "x"),
                ("Y", "Y"),
                ("y", "y"),
                ("Aa", "Aa"),
                ("Yy", "Yy"),
                ("Abecdfiogh", "ebAcdfiogh")
            };
            foreach (var scramblerTestCombo in combosForTest)
            {
                Assert.AreEqual(scramblerTestCombo.outputScrambled, Argot.PhoneticSwap(scramblerTestCombo.inputPlain));
            }
        }
    }
}