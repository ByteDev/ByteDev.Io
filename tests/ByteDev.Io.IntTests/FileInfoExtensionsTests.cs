﻿using System;
using System.IO;
using System.Reflection;
using ByteDev.Io.IntTests.TestFiles;
using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
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
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

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
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test1 (2).txt")));
            }

            [Test]
            public void WhenSecondFileExists_ThenReturnThirdFileName()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1 (2).txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test1 (3).txt")));
            }

            [Test]
            public void WhenFirstAndThirdFileExists_ThenReturnSecondFileName()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test (3).txt")).Build();

                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test (2).txt")));
            }

            [Test]
            public void WhenFileWithZeroFlagExists_ThenReturnFirstFileName()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test (0).txt")).Build();

                var result = sut.GetNextAvailableFileName();

                Assert.That(result.FullName, Is.EqualTo(GetExpectedPath("Test (1).txt")));
            }

            [Test]
            public void WhenFileExistsWithNonSpacedFlag_ThenReturnSecondFileName()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test(1).txt")).Build();

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
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

                sut.AddExtension(".log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test"));
            }

            [Test]
            public void WhenFileAlreadyHasExtension_ThenThrowException()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                Assert.Throws<InvalidOperationException>(() => sut.AddExtension(".log"));
            }

            [Test]
            public void WhenExtensionHasNoDotPrefix_ThenAddWithDotPrefix()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

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
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.log")).Build();
                
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var ex = Assert.Throws<IOException>(() => sut.RenameExtension(".log"));
                Assert.That(ex.Message, Is.EqualTo("Cannot create a file when that file already exists."));
            }

            [Test]
            public void WhenNewExtensionIsEmpty_ThenRemoveExtension()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(string.Empty);

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
            }

            [Test]
            public void WhenNewAndOldExtensionAreEqual_ThenKeepExtension()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(".txt");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenNewExtensionIsDifferent_ThenRenameExtension()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension(".log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenNewExtensionIsDifferent_AndNewFileExists_ThenThrowException()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.log")).Build();

                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                var ex = Assert.Throws<IOException>(() => sut.RenameExtension(".log"));      
                Assert.That(ex.Message, Is.EqualTo("Cannot create a file when that file already exists."));

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
            }

            [Test]
            public void WhenExtensionHasNoDotPrefix_ThenAddDotPrefix()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RenameExtension("log");

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.log"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenExistingFileHasNoExtension_ThenAddFileExtension()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

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
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.TXT")).Build();

                sut.RenameExtensionToLower();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenFileHasNoExtension_ThenDoNothing()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

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
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test.txt")).Build();

                sut.RemoveExtension();

                AssertFile.Exists(Path.Combine(WorkingDir, "Test"));
                AssertFile.NotExists(Path.Combine(WorkingDir, "Test.txt"));
            }

            [Test]
            public void WhenFileHasNoExtension_ThenDoNothing()
            {
                var sut = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

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
                var sut = FileBuilder.InFileSystem
                    .WithPath(Path.Combine(WorkingDir, "Test1.txt"))
                    .WithSize(10000)
                    .Build();

                var result = sut.IsBinary();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenFileHasConsecutiveNul_ThenReturnTrue()
            {
                var sut = FileBuilder.InFileSystem
                    .WithPath(Path.Combine(WorkingDir, "Test1.bin"))
                    .WithText("abc123\0\0123")
                    .Build();

                var result = sut.IsBinary(2);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenFileHasOneNul_AndTwoConsecutiveSpecified_ThenReturnFalse()
            {
                var sut = FileBuilder.InFileSystem
                    .WithPath(Path.Combine(WorkingDir, "Test1.bin"))
                    .WithText("abc123\0123")
                    .Build();

                var result = sut.IsBinary(2);

                Assert.That(result, Is.False);
            }

            [TestCase(TestFileNames.Binary.Png)]
            public void WhenFileIsBinaryTestFile_ThenReturnTrue(string filePath)
            {
                var sut = new FileInfo(filePath);

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

        [TestFixture]
        public class DeleteLine : FileInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenFileIsEmpty_ThenDeleteNothing()
            {
                var sut = FileBuilder.InFileSystem.WithPath(GetAbsolutePath("DeleteLine1.txt")).Build();

                var result = sut.DeleteLine(1, GetAbsolutePath("DeleteLine1-Output.txt"));

                AssertFile.IsEmpty(result);
            }

            [Test]
            public void WhenFileHasOneLine_ThenDeleteLine()
            {
                var sut = FileBuilder.InFileSystem.WithText("Line 1").WithPath(GetAbsolutePath("DeleteLine2.txt")).Build();

                var result = sut.DeleteLine(1, GetAbsolutePath("DeleteLine2-Output.txt"));

                AssertFile.IsEmpty(result);
            }

            [Test]
            public void WhenFileHasThreeLines_ThenDeleteLine()
            {
                const string text = "Line1\nLine2\nLine3";

                var sut = FileBuilder.InFileSystem.WithText(text).WithPath(GetAbsolutePath("DeleteLine3.txt")).Build();

                var result = sut.DeleteLine(2, GetAbsolutePath("DeleteLine3-Output.txt"));

                AssertFile.ContentEquals(result, "Line1\nLine3");
            }

            [Test]
            public void WhenLineToDeleteIsEmpty_ThenDeleteLine()
            {
                const string text = "Line1\n\nLine3";

                var sut = FileBuilder.InFileSystem.WithText(text).WithPath(GetAbsolutePath("DeleteLine4.txt")).Build();

                var result = sut.DeleteLine(2, GetAbsolutePath("DeleteLine4-Output.txt"));

                AssertFile.ContentEquals(result, "Line1\nLine3");
            }

            [Test]
            public void WhenLineDoesNotExist_ThenDeleteNothing()
            {
                var sut = FileBuilder.InFileSystem.WithText("Line 1").WithPath(GetAbsolutePath("DeleteLine5.txt")).Build();

                var result = sut.DeleteLine(2, GetAbsolutePath("DeleteLine5-Output.txt"));

                AssertFile.AreSame(result, sut);
            }
        }
    }
}
