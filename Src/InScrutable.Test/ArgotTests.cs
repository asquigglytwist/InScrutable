namespace InScrutable.Test
{
    [TestClass]
    public class ArgotTests
    {
        [TestMethod]
        public void ScramblerTest()
        {
            var plainString = "asdfgf";
            Assert.AreEqual(plainString, Argot.Scramble(plainString));
        }
    }
}