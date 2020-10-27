using AutoMovePipWindow.Configuration;
using NUnit.Framework;

namespace AutoMovePipWindowTests
{
    public class ConfigurationTests
    {
        [Test]
        public void TestSizeConfiguration()
        {
            SizeConfiguration size = "40*16/9:8";
            Assert.AreEqual(8, size.Margin);
            Assert.AreEqual(40 * 16, size.Width);
            Assert.AreEqual(40 * 9, size.Height);
        }
    }
}
