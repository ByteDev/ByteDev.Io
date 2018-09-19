﻿using System;
using System.IO;
using System.Linq;
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

        [TestFixture]
        public class FirstExists : FileSystemTest
        {
            [Test]
            public void WhenPathsIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.FirstExists(null));
            }

            [Test]
            public void WhenPathsIsEmpty_ThenThrowException()
            {
                Assert.Throws<ArgumentException>(() => _sut.FirstExists(Enumerable.Empty<string>()));
            }
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

        [TestFixture]
        public class DeleteDirectoriesWithName : FileSystemTest
        {
            [Test]
            public void WhenBasePathDirInfoIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.DeleteDirectoriesWithName(null as DirectoryInfo, "someDir"));
            }

            [Test]
            public void WhenBasePathIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.DeleteDirectoriesWithName(null as string, "someDir"));
            }

            [Test]
            public void WhenDirNameIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentException>(() => _sut.DeleteDirectoriesWithName(@"C:\Temp", null));
            }
        }
    }
}