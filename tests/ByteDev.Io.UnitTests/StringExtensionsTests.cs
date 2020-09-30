using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [TestFixture]
        public class AddFileExtensionDotPrefix : StringExtensionsTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIsNullOrEmpty_ThenReturnSame(string sut)
            {
                var result = sut.AddFileExtensionDotPrefix();

                Assert.That(result, Is.EqualTo(sut));
            }

            [TestCase(".")]
            [TestCase(".t")]
            [TestCase(".txt")]
            public void WhenHasDotPrefix_ThenReturnSame(string sut)
            {
                var result = sut.AddFileExtensionDotPrefix();

                Assert.That(result, Is.EqualTo(sut));
            }

            [TestCase("t")]
            [TestCase("txt")]
            public void WhenHasNoDotPrefix_ThenReturnWithDotPrefix(string sut)
            {
                var result = sut.AddFileExtensionDotPrefix();

                Assert.That(result, Is.EqualTo("." + sut));
            }
        }
    }
}