using System;
using System.IO;
using System.Reflection;
using ByteDev.Testing.Nunit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileInfoExtensionsTest : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            SetWorkingDir(MethodBase.GetCurrentMethod().DeclaringType, methodName);
        }

        [TestFixture]
        public class HasExtension : FileInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileHasExtension_ThenReturnTrue()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "Test.txt"));

                var result = sut.HasExtension();

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenFileHasNoExtension_ThenReturnFalse()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "Test"));

                var result = sut.HasExtension();

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class AddExtension : FileInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "DoesntExist"));

                Assert.Throws<FileNotFoundException>(() => sut.AddExtension(".log"));
            }
            
            [Test]
            public void WhenFileHasNoExtension_ThenAddExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.AddExtension(".log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test"));
            }

            [Test]
            public void WhenFileAlreadyHasExtension_ThenThrowException()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                Assert.Throws<InvalidOperationException>(() => sut.AddExtension(".log"));
            }

            [Test]
            public void WhenExtensionHasNoDotPrefix_ThenAddWithDotPrefix()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.AddExtension("log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test"));
            }
        }

        [TestFixture]
        public class RemoveExtension : FileInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileHasExtension_ThenRemoveExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RemoveExtension();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenFileHasNoExtension_ThenDoNotRenameFile()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.RemoveExtension();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
            }
        }

        [TestFixture]
        public class RenameExtension : FileInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "DoesntExist.txt"));

                Assert.Throws<FileNotFoundException>(() => sut.RenameExtension(".log"));
            }

            [Test]
            public void WhenNewExtensionIsEmpty_ThenRemoveExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(string.Empty);

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
            }

            [Test]
            public void WhenNewAndOldExtensionAreEqual_ThenKeepExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(".txt");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenNewExtensionIsDifferent_ThenRenameExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(".log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenNewExtensionIsDifferent_AndNewFileExists_ThenThrowException()
            {
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.log")).Build();

                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var ex = Assert.Throws<IOException>(() => sut.RenameExtension(".log"));      
                Assert.That(ex.Message, Is.EqualTo("Cannot create a file when that file already exists"));

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
            }

            [Test]
            public void WhenExtensionHasNoDotPrefix_ThenAddDotPrefix()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension("log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenExistingFileHasNoExtension_ThenAddFileExtension()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.RenameExtension(".log");
                
                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test"));
            }
        }

        [TestFixture]
        public class GetNextAvailableFileName : FileInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());                
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileDoesNotExist_ThenReturnSameFile()
            {
                var fileName = Path.Combine(WorkingDir, Path.GetRandomFileName());

                var sut = new FileInfo(fileName);

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(fileName));
            }

            [Test]
            public void WhenFirstFileExists_ThenReturnSecondFileName()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test1 (2).txt")));
            }

            [Test]
            public void WhenSecondFileExists_ThenReturnThirdFileName()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1 (2).txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test1 (3).txt")));
            }

            [Test]
            public void WhenFirstAndThirdFileExists_ThenReturnSecondFileName()
            {
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test (3).txt")).Build();

                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test (2).txt")));
            }

            [Test]
            public void WhenFileWithZeroFlagExists_ThenReturnFirstFileName()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test (0).txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test (1).txt")));
            }

            [Test]
            public void WhenFileExistsWithNonSpacedFlag_ThenReturnSecondFileName()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test(1).txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test(1) (2).txt")));
            }

            private string GetExpectedPath(string fileName)
            {
                return Path.Combine(WorkingDir, fileName);
            }
        }
    }
}
