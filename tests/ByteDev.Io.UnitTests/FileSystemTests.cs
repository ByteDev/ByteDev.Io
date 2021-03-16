using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class FileSystemTests
    {
        private const string FileExists = @"C:\Windows\explorer.exe";

        private FileSystem _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileSystem();
        }

        [TestFixture]
        public class GetPathExists : FileSystemTests
        {
            [Test]
            public void WhenPathIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.GetPathExists(null));
            }

            [Test]
            public void WhenPathIsEmpty_ThenThrowException()
            {
                Assert.Throws<ArgumentException>(() => _sut.GetPathExists(string.Empty));
            }
        }

        [TestFixture]
        public class FirstExists : FileSystemTests
        {
            [Test]
            public void WhenPathsIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.FirstExists(null));
            }

            [Test]
            public void WhenPathsIsEmpty_ThenThrowException()
            {
                Assert.Throws<PathNotFoundException>(() => _sut.FirstExists(Enumerable.Empty<string>()));
            }
        }

        [TestFixture]
        public class MoveFile : FileSystemTests
        {
            [Test]
            public void WhenFileInfo1IsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.SwapFileNames(null, new FileInfo(FileExists)));
            }
        }

        public class SwapFileNames : FileSystemTests
        {
            [Test]
            public void WhenFileInfo1IsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.SwapFileNames(null, new FileInfo(FileExists)));
            }

            [Test]
            public void WhenFileInfo2IsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.SwapFileNames(new FileInfo(FileExists), null));
            }

            [Test]
            public void WhenFile1IsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.SwapFileNames(null, FileExists));
            }

            [Test]
            public void WhenFile2IsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.SwapFileNames(FileExists, null));
            }
        }
    }
}