using System;
using System.IO;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class DirectoryInfoExtensionsTests
    {
        private DirectoryInfo _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new DirectoryInfo(@"C:\Temp");
        }

        [TestFixture]
        public class DeleteDirectoriesWithName : DirectoryInfoExtensionsTests
        {
            [Test]
            public void WhenBasePathDirInfoIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => DirectoryInfoExtensions.DeleteDirectoriesWithName(null, "someDir"));
            }

            [TestCase(null)]
            [TestCase("")]
            public void WhenDirNameIsNullOrEmpty_ThenThrowException(string directoryName)
            {
                Assert.Throws<ArgumentException>(() => _sut.DeleteDirectoriesWithName(directoryName));
            }
        }
    }
}