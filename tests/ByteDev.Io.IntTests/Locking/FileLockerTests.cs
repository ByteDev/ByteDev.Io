using System;
using System.IO;
using System.Reflection;
using ByteDev.Io.Locking;
using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.Locking
{
    [TestFixture]
    public class FileLockerTests : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            SetWorkingDir(MethodBase.GetCurrentMethod().DeclaringType, methodName);
        }

        [TestFixture]
        public class Lock : FileLockerTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());   
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => FileLocker.Lock(FileNotExist));
            }

            [Test]
            public void WhenExistsButIsNotFile_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => FileLocker.Lock(WorkingDir));
            }

            [Test]
            public void WhenLockFileExist_ThenThrowException()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                
                FileBuilder.InFileSystem.WithPath(fileInfo.FullName + ".lock").Build();
                
                Assert.Throws<InvalidOperationException>(() => FileLocker.Lock(fileInfo));
            }

            [Test]
            public void WhenFileExists_ThenCreateLockFile()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var result = FileLocker.Lock(fileInfo);

                Assert.That(result.File.FullName, Is.EqualTo(fileInfo.FullName));
                Assert.That(result.LockFile.FullName, Is.EqualTo(fileInfo.FullName + ".lock"));
                AssertFile.Exists(result.LockFile);
            }

            [Test]
            public void WhenExistingFileIsLockFile_ThenCreateLockFile()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.lock")).Build();

                var result = FileLocker.Lock(fileInfo);

                Assert.That(result.File.FullName, Is.EqualTo(fileInfo.FullName));
                Assert.That(result.LockFile.FullName, Is.EqualTo(fileInfo.FullName + ".lock"));
                AssertFile.Exists(result.LockFile);
            }
        }

        [TestFixture]
        public class IsLocked : FileLockerTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());   
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => FileLocker.IsLocked(FileNotExist));
            }

            [Test]
            public void WhenFileIsNotLocked_ThenReturnFalse()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var result = FileLocker.IsLocked(fileInfo);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenFileIsLocked_ThenReturnTrue()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                FileLocker.Lock(fileInfo);

                var result = FileLocker.IsLocked(fileInfo);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class Unlock : FileLockerTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());   
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => FileLocker.Unlock(FileNotExist));
            }

            [Test]
            public void WhenFileIsNotLocked_ThenDoNothing()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                Assert.DoesNotThrow(() => FileLocker.Unlock(fileInfo));
            }

            [Test]
            public void WhenFileIsLocked_ThenUnlock()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                FileLocker.Lock(fileInfo);

                FileLocker.Unlock(fileInfo);

                Assert.That(FileLocker.IsLocked(fileInfo), Is.False);
            }
        }
    }
}