using System;
using System.IO;
using System.Reflection;
using ByteDev.Testing.NUnit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileInfoExtensionsTests : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            SetWorkingDir(MethodBase.GetCurrentMethod().DeclaringType, methodName);
        }

        [TestFixture]
        public class DeleteIfExists : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());                
            }

            [Test]
            public void WhenFileExists_ThenDelete()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                sut.DeleteIfExists();

                AssertFile.NotExists(sut);
            }

            [Test]
            public void WhenFileDoesNotExist_ThenDoNothing()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "DoesntExist"));

                sut.DeleteIfExists();

                AssertFile.NotExists(sut);
            }
        }

        [TestFixture]
        public class GetNextAvailableFileName : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());                
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

        [TestFixture]
        public class AddExtension : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
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
        public class RenameExtension : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                var sut = new FileInfo(Path.Combine(WorkingDir, "DoesntExist.txt"));

                Assert.Throws<FileNotFoundException>(() => sut.RenameExtension(".log"));
            }

            [Test]
            public void WhenTargetNameAlreadyExists_ThenThrowException()
            {
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.log")).Build();
                
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var ex = Assert.Throws<IOException>(() => sut.RenameExtension(".log"));
                Assert.That(ex.Message, Is.EqualTo("Cannot create a file when that file already exists."));
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
                Assert.That(ex.Message, Is.EqualTo("Cannot create a file when that file already exists."));

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
        public class RenameExtensionToLower : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenFileHasExtension_ThenMakeLowerCase()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test.TXT")).Build();

                sut.RenameExtensionToLower();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenFileHasNoExtension_ThenDoNothing()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.RenameExtensionToLower();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
            }
        }

        [TestFixture]
        public class RemoveExtension : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
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
            public void WhenFileHasNoExtension_ThenDoNothing()
            {
                var sut = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test")).Build();

                sut.RemoveExtension();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
            }
        }

        [TestFixture]
        public class IsBinary : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenFileIsNotBinary_ThenReturnFalse()
            {
                var sut = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.txt"))
                    .WithSize(10000)
                    .Build();

                var result = sut.IsBinary();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenFileIsBinary_ThenReturnTrue()
            {
                var sut = FileTestBuilder.InFileSystem
                    .WithFilePath(Path.Combine(WorkingDir, "Test1.bin"))
                    .WithText("abc123\0123")
                    .Build();

                var result = sut.IsBinary();

                Assert.That(result, Is.True);
            }

            [TestCase(@"C:\Windows\notepad.exe")]
            [TestCase(@"C:\Windows\regedit.exe")]
            [TestCase(@"C:\Windows\write.exe")]
            [TestCase(@"C:\Windows\explorer.exe")]
            [TestCase(@"C:\Windows\System32\Windows.UI.Xaml.dll")]
            public void WhenRealWorldBinaryFiles_ThenReturnTrue(string file)
            {
                if (File.Exists(file))
                {
                    var sut = new FileInfo(file);

                    var result = sut.IsBinary();

                    Assert.That(result, Is.True);
                }
                else
                {
                    Console.WriteLine($"'{file}' does not exist.");
                }
            }
        }
    }
}
