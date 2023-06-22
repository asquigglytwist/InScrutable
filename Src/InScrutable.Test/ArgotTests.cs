namespace InScrutable.Test
{
    [TestClass]
    public class ArgotTests
    {
        [TestMethod]
        public void ScramblerTest()
        {
            var plainString = "Abecdfiogh";
            Assert.AreEqual(plainString, Argot.PhoneticSwap(plainString));
        }
    }
}