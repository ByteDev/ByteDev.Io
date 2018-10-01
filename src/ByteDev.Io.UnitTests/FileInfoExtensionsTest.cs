using System;
using System.IO;
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

        [TestFixture]
        public class RenameExtension : FileInfoExtensionsTest
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.RenameExtension(null, ".txt"));
            }

            [Test]
            public void WhenNewExtensionIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => new FileInfo(@"C:\").RenameExtension(null));
            }
        }

        [TestFixture]
        public class AddExtension : FileInfoExtensionsTest
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.AddExtension(null, ".txt"));
            }

            [Test]
            public void WhenExtensionIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => new FileInfo(@"C:\").AddExtension(null));
            }
        }

        [TestFixture]
        public class HasExtension : FileInfoExtensionsTest
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.HasExtension(null));
            }
        }
    }
}