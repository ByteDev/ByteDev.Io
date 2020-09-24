using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class PathNotFoundExceptionTests
    {
        [Test]
        public void WhenNoArgs_ThenSetMessageToDefault()
        {
            var sut = new PathNotFoundException();

            Assert.That(sut.Message, Is.EqualTo("Unable to find the specified file or directory."));
        }

        [Test]
        public void WhenMessageSpecified_ThenSetMessage()
        {
            var sut = new PathNotFoundException("Some message.");

            Assert.That(sut.Message, Is.EqualTo("Some message."));
        }

        [Test]
        public void WhenMessageAndInnerExSpecified_ThenSetMessageAndInnerEx()
        {
            var innerException = new Exception();

            var sut = new PathNotFoundException("Some message.", innerException);

            Assert.That(sut.Message, Is.EqualTo("Some message."));
            Assert.That(sut.InnerException, Is.SameAs(innerException));
        }
    }
}