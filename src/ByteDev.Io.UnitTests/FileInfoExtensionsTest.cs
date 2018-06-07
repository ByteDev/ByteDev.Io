using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    public class FileInfoExtensionsTest
    {
        [TestFixture]
        public class GetNextAvailableFileName : FileInfoExtensionsTest
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.GetNextAvailableFileName(null));
            }
        }
    }
}