using System;
using System.IO;
using System.Linq;
using ByteDev.Collections;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class StreamExtensionsTests
    {
        [TestFixture]
        public class IsEmpty : StreamExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.IsEmpty(null));
            }

            [Test]
            public void WhenIsEmpty_ThenReturnTrue()
            {
                var sut = new MemoryStream();

                var result = sut.IsEmpty();

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenIsNotEmpty_ThenReturnFalse()
            {
                var sut = StreamFactory.Create("A");

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class ReadAsString : StreamExtensionsTests
        {
            [Test]
            public void WhenStreamIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadAsString(null));
            }

            [Test]
            public void WhenStreamIsEmpty_ThenReturnEmpty()
            {
                var sut = new MemoryStream();

                var result = sut.ReadAsString();

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenStreamContainsText_ThenReturnString()
            {
                var sut = StreamFactory.Create("ABC");

                var result = sut.ReadAsString();

                Assert.That(result, Is.EqualTo("ABC"));
            }
        }

        [TestFixture]
        public class ReadAsMemoryStream : StreamExtensionsTests
        {
            [Test]
            public void WhenStreamIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadAsMemoryStream(null));
            }

            [Test]
            public void WhenStreamIsNotNull_ThenReturnMemoryStream()
            {
                var sut = StreamFactory.Create("ABC");

                var result = sut.ReadAsMemoryStream();

                var text = result.ReadAsString();

                Assert.That(text, Is.EqualTo("ABC"));
            }
        }

        [TestFixture]
        public class ReadAsBytes : StreamExtensionsTests
        {
            [Test]
            public void WhenStreamIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => StreamExtensions.ReadAsBytes(null));
            }

            [Test]
            public void WhenStreamIsMemoryStream_ThenReturnBytes()
            {
                var sut = StreamFactory.Create(new byte[] { 0xA1, 0xA2 });

                var result = sut.ReadAsBytes();

                Assert.That(result.Length, Is.EqualTo(2));
                Assert.That(result.First(), Is.EqualTo(0xA1));
                Assert.That(result.Second(), Is.EqualTo(0xA2));
            }
        }
    }
}