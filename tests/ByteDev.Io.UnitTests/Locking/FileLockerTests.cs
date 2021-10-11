using System;
using System.IO;
using ByteDev.Io.Locking;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests.Locking
{
    [TestFixture]
    public class FileLockerTests
    {
        [TestFixture]
        public class Lock : FileLockerTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIsNullOrEmpty_ThenThrowException(string filePath)
            {
                Assert.Throws<ArgumentException>(() => FileLocker.Lock(filePath));
            }

            [Test]
            public void WhenFileInfoIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileLocker.Lock(null as FileInfo));
            }
        }

        [TestFixture]
        public class IsLocked : FileLockerTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIsNullOrEmpty_ThenThrowException(string filePath)
            {
                Assert.Throws<ArgumentException>(() => FileLocker.IsLocked(filePath));
            }

            [Test]
            public void WhenFileInfoIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileLocker.IsLocked(null as FileInfo));
            }
        }

        [TestFixture]
        public class Unlock : FileLockerTests
        {
            [TestCase(null)]
            [TestCase("")]
            public void WhenIsNullOrEmpty_ThenThrowException(string filePath)
            {
                Assert.Throws<ArgumentException>(() => FileLocker.Unlock(filePath));
            }

            [Test]
            public void WhenFileInfoIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileLocker.Unlock(null as FileInfo));
            }
        }
    }
}