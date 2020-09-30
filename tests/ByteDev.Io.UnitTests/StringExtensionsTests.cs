using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestFixture]
        public class GetEndLineChars : StringExtensionsTests
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

        [TestFixture]
        public class RemoveEndLineChars : StringExtensionsTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenNullOrEmpty_ThenReturnSame(string sut)
            {
                var result = sut.RemoveEndLineChars();

                Assert.That(result, Is.EqualTo(sut));
            }

            [Test]
            public void WhenHasNoEndLineChars_ThenReturnSame()
            {
                const string sut = "Line number 1";

                var result = sut.RemoveEndLineChars();

                Assert.That(result, Is.EqualTo(sut));
            }

            [Test]
            public void WhenHasWindowsEndLineChars_ThenRemove()
            {
                const string sut = "Line number 1\r\n" +
                                   "Line number 2\r\n";

                const string expected = "Line number 1\r\n" +
                                        "Line number 2";

                var result = sut.RemoveEndLineChars();

                Assert.That(result, Is.EqualTo(expected));
            }

            [Test]
            public void WhenHasUnixEndLineChars_ThenRemove()
            {
                const string sut = "Line number 1\n" +
                                   "Line number 2\n";

                const string expected = "Line number 1\n" +
                                        "Line number 2";

                var result = sut.RemoveEndLineChars();

                Assert.That(result, Is.EqualTo(expected));
            }
        }
    }
}