using System.IO;
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
            public void WhenIsEmpty_ThenReturnTrue()
            {
                var sut = new MemoryStream();

                var result = sut.IsEmpty();

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenIsNotEmpty_ThenReturnFalse()
            {
                var sut = StreamHelper.CreateStream("A");

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }
        }
    }
}