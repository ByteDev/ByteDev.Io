using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Common.Collections;
using ByteDev.Testing.Nunit;
using ByteDev.Testing.TestBuilders.FileSystem;
using ByteDev.Testing.TestHelpers.Io;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileSystemTest : IoTestBase
    {
        private FileSystem _sut;

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }
        
        [SetUp]
        public void Setup()
        {
            _sut = new FileSystem();            
        }

        [TestFixture]
        public class FirstExists : FileSystemTest
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateWorkingDir();
            }

            [Test]
            public void WhenNoPathExists_ThenThrowException()
            {
                string[] paths = 
                {
                    Path.Combine(WorkingDir, "test1.txt"),
                    Path.Combine(WorkingDir, "test2.txt")
                };

                Assert.Throws<PathNotFoundException>(() => _sut.FirstExists(paths));
            }

            [Test]
            public void WhenOneFilePathMatches_ThenReturnPath()
            {
                var testFile = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "test2.txt")).Build();

                string[] paths =
                {
                    Path.Combine(WorkingDir, "test1.txt"),
                    testFile.FullName
                };

                var result = _sut.FirstExists(paths);

                Assert.That(result, Is.EqualTo(testFile.FullName));
            }

            [Test]
            public void WhenPathsMixed_AndDirectoryFound_ThenReturnDirectory()
            {
                var testDir = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDirectory")).Build();
                var testFile = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "test2.txt")).Build();

                string[] paths =
                {
                    Path.Combine(WorkingDir, "SomeDir"),
                    Path.Combine(WorkingDir, "test1.txt"),
                    testDir.FullName,
                    testFile.FullName
                };

                var result = _sut.FirstExists(paths);

                Assert.That(result, Is.EqualTo(testDir.FullName));
            }

            [Test]
            public void WhenPathsMixed_AndFileFound_ThenReturnFile()
            {
                var testDir = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDirectory")).Build();
                var testFile = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "test2.txt")).Build();

                string[] paths =
                {
                    Path.Combine(WorkingDir, "SomeDir"),
                    Path.Combine(WorkingDir, "test1.txt"),
                    testFile.FullName,
                    testDir.FullName
                };

                var result = _sut.FirstExists(paths);

                Assert.That(result, Is.EqualTo(testFile.FullName));
            }
        }

        [TestFixture]
        public class GetDirectories : FileSystemTest
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateWorkingDir();
            }

            [Test]
            public void WhenPathDoesNotExist_ThenThrowException()
            {
                Assert.Throws<DirectoryNotFoundException>(() => _sut.GetDirectories(@"C:\b7b17fa382754b138ec4e7d710e298f8"));
            }

            [Test]
            public void WhenPathIsEmpty_ThenReturnEmpty()
            {
                var result = _sut.GetDirectories(WorkingDir);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenPathHasDirectories_ThenReturnDirectories()
            {
                var dir1 = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test1")).Build();
                var dir2 = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test2")).Build();

                var result = _sut.GetDirectories(WorkingDir);

                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.First(), Is.EqualTo(dir1.FullName));
                Assert.That(result.Second(), Is.EqualTo(dir2.FullName));
            }
        }

        [TestFixture]
        public class GetFiles : FileSystemTest
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateWorkingDir();
            }

            [Test]
            public void WhenPathDoesNotExist_ThenThrowException()
            {
                Assert.Throws<DirectoryNotFoundException>(() => _sut.GetFiles(@"C:\b7b17fa382754b138ec4e7d710e298f8"));
            }

            [Test]
            public void WhenPathIsEmpty_ThenReturnEmpty()
            {
                var result = _sut.GetFiles(WorkingDir);

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenPathHasFiles_ThenReturnFiles()
            {
                var file1 = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "file1.txt")).Build();
                var file2 = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "file2.txt")).Build();

                var result = _sut.GetFiles(WorkingDir);

                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.First(), Is.EqualTo(file1.FullName));
                Assert.That(result.Second(), Is.EqualTo(file2.FullName));
            }
        }

        [TestFixture]
        public class SwapFileNames : FileSystemTest
        {
            private string _filePath1;
            private string _filePath2;
            
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateWorkingDir();

                _filePath1 = Path.Combine(WorkingDir, "file1.txt");
                _filePath2 = Path.Combine(WorkingDir, "file2.txt");
            }

            [Test]
            public void WhenBothFilesExist_ThenSwapFileNames()
            {
                var file1 = FileTestBuilder.InFileSystem.WithFilePath(_filePath1).WithSize(1).Build();
                var file2 = FileTestBuilder.InFileSystem.WithFilePath(_filePath2).WithSize(2).Build();

                _sut.SwapFileNames(file1, file2);

                AssertFile.SizeEquals(_filePath1, 2);
                AssertFile.SizeEquals(_filePath2, 1);
            }

            [Test]
            public void WhenFirstFileDoesNotExist_ThenThrowException()
            {
                var file1 = new FileInfo(_filePath1);
                var file2 = FileTestBuilder.InFileSystem.WithFilePath(_filePath2).WithSize(2).Build();

                Assert.Throws<FileNotFoundException>(() => Act(file1, file2));
            }
            [Test]
            public void WhenSecondFileDoesNotExist_ThenThrowException()
            {
                var file1 = FileTestBuilder.InFileSystem.WithFilePath(_filePath1).WithSize(1).Build();
                var file2 = new FileInfo(_filePath2);

                Assert.Throws<FileNotFoundException>(() => Act(file1, file2));
            }

            [Test]
            public void WhenSecondFileDoesNotExist_ThenRenameFirstFileBackToOriginalName()
            {
                var file1 = FileTestBuilder.InFileSystem.WithFilePath(_filePath1).WithSize(1).Build();
                var file2 = new FileInfo(_filePath2);

                Assert.Throws<FileNotFoundException>(() => Act(file1, file2));

                AssertFile.Exists(_filePath1);
                AssertFile.SizeEquals(_filePath1, 1);
            }

            private void Act(FileInfo file1, FileInfo file2)
            {
                _sut.SwapFileNames(file1, file2);
            }
        }

        [TestFixture]
        public class CopyFile : FileSystemTest
        {
            private string _sourceDir;
            private string _destinationDir;

            private const string FileName = "Test1.txt";

            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

                _sourceDir = Path.Combine(WorkingDir, "Source");
                _destinationDir = Path.Combine(WorkingDir, "Destination");

                DirectoryTestBuilder.InFileSystem.WithPath(_sourceDir).EmptyIfExists().Build();
                DirectoryTestBuilder.InFileSystem.WithPath(_destinationDir).EmptyIfExists().Build();
            }

            [Test]
            public void WhenSourceFileExists_ThenCopyFile()
            {
                var sourceFile = CreateSourceFile(FileName);

                var result = Act(sourceFile.FullName, Path.Combine(_destinationDir, FileName));

                AssertFile.Exists(sourceFile);
                AssertFile.Exists(result);
            }

            private FileInfo Act(string sourceFile, string destinationFile)
            {
                return _sut.CopyFile(new FileInfo(sourceFile), new FileInfo(destinationFile));
            }

            private FileInfo CreateSourceFile(string filePath, long size = 0)
            {
                return FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(_sourceDir, filePath)).WithSize(size).Build();
            }
        }

        [TestFixture]
        public class MoveFile : FileSystemTest
        {
            private string _sourceDir;
            private string _destinationDir;

            private const string FileName = "Test1.txt";

            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

                _sourceDir = Path.Combine(WorkingDir, "Source");
                _destinationDir = Path.Combine(WorkingDir, "Destination");

                DirectoryTestBuilder.InFileSystem.WithPath(_sourceDir).EmptyIfExists().Build();
                DirectoryTestBuilder.InFileSystem.WithPath(_destinationDir).EmptyIfExists().Build();
            }

            [Test]
            public void WhenSourceFileExists_ThenMoveFile()
            {
                var sourceFile = CreateSourceFile(FileName);

                var result = Act(sourceFile.FullName, Path.Combine(_destinationDir, FileName));

                AssertFile.NotExists(sourceFile);
                AssertFile.Exists(result);
            }

            private FileInfo Act(string sourceFile, string destinationFile)
            {
                return _sut.MoveFile(new FileInfo(sourceFile), new FileInfo(destinationFile));
            }

            private FileInfo CreateSourceFile(string filePath, long size = 0)
            {
                return FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(_sourceDir, filePath)).WithSize(size).Build();
            }
        }
        
        [TestFixture]
        public class DeleteDirectory : FileSystemTest
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ThenNotThrowException()
            {
                var dir = Path.Combine(WorkingDir, Guid.NewGuid().ToString());

                _sut.DeleteDirectory(dir);

                AssertDir.NotExists(dir);
            }

            [Test]
            public void WhenTwoFilesAndTwoDirectories_ThenDeleteAllFilesAndDirectories()
            {
                var newDir = Path.Combine(WorkingDir, "Test1");

                DirectoryHelper.EmptyIfExists(newDir);

                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(newDir, "DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(newDir, "DirTest2")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(newDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(newDir, "Test2.txt")).Build();

                _sut.DeleteDirectory(newDir);

                AssertDir.NotExists(newDir);
            }
        }
    }
}
