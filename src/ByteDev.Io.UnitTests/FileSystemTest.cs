using System;
using System.IO;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class FileSystemTest
    {
        private const string FileExists = @"C:\Windows\explorer.exe";

        private FileSystem _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileSystem();
        }

        public class SwapFileNames : FileSystemTest
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