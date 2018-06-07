using System;
using System.IO;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    namespace DirectoryInfoExtensionsTest
    {
        [TestFixture]
        public class GetFilesByExtensions
        {
            [Test]
            public void WhenExtensionsIsNull_ThenThrowException()
            {
                var sut = new DirectoryInfo(@"C:\");

                Assert.Throws<ArgumentNullException>(() => sut.GetFilesByExtensions(null as string[]));
            }
        }
    }
}
