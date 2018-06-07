using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [TestFixture]
        public class AppendBackSlash
        {
            [Test]
            public void WhenPathIsNull_ThenThrowException()
            {
                string sut = null;

                Assert.Throws<ArgumentException>(() => sut.AppendBackSlash());
            }

            [Test]
            public void WhenPathIsEmpty_ThenThrowException()
            {
                string sut = string.Empty;

                Assert.Throws<ArgumentException>(() => sut.AppendBackSlash());
            }

            [Test]
            public void WhenPathHasBackSlash_ThenReturnSamePath()
            {
                const string sut = @"C:\Windows\";

                var result = sut.AppendBackSlash();

                Assert.That(result, Is.EqualTo(sut));
            }

            [Test]
            public void WhenPathHasNoBackSlash_ThenReturnPathWithBackSlash()
            {
                const string sut = @"C:\Windows";

                var result = sut.AppendBackSlash();

                Assert.That(result, Is.EqualTo(sut + @"\"));
            }

            [Test] 
            public void WhenPathIsBackSlash_ThenReturnSamePath()
            {
                const string sut = @"\";

                var result = sut.AppendBackSlash();

                Assert.That(result, Is.EqualTo(sut));
            }

            [Test]
            public void WhenPathIsNotBackSlash_AndOneCharLong_ThenReturnPathWithBackSlash()
            {
                const string sut = @"C";

                var result = sut.AppendBackSlash();

                Assert.That(result, Is.EqualTo(sut + @"\"));
            }
        }
    }
}
