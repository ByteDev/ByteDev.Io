using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestCase(null)]
        [TestCase("")]
        public void WhenNullOrEmpty_ThenReturnEmpty(string sut)
        {
            var result = sut.GetEndLineChars();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenHasNoEndLineChars_ThenReturnEmpty()
        {
            var result = "Line 1".GetEndLineChars();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenHasNoEndLineChars_AndMultiLine_ThenReturnEmpty()
        {
            var result = "Line 1\r\nLine2"
                .GetEndLineChars();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenHasWindowsEndLineChars_ThenReturnWindowsEndLineChars()
        {
            var result = "Line 1\nLine2\r\n"
                .GetEndLineChars();

            Assert.That(result, Is.EqualTo("\r\n"));
        }

        [Test]
        public void WhenHasUnixEndLineChars_ThenReturnUnixEndLineChars()
        {
            var result = "Line 1\r\nLine2\n"
                .GetEndLineChars();

            Assert.That(result, Is.EqualTo("\n"));
        }
    }
}