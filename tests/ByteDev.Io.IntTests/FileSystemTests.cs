using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Collections;
using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileSystemTests : IoTestBase
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
        public class GetPathExists : FileSystemTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();
            }

            [Test]
            public void WhenPathIsFile_AndExists_ThenReturnPath()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test.txt")).Build();

                var result = _sut.GetPathExists(fileInfo.FullName);

                Assert.That(result, Is.EqualTo(fileInfo.FullName));
            }

            [Test]
            public void WhenPathIsDir_AndExists_ThenReturnPath()
            {
                var dirInfo = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test")).Build();

                var result = _sut.GetPathExists(dirInfo.FullName);

                Assert.That(result, Is.EqualTo(dirInfo.FullName));
            }

            [Test]
            public void WhenPathIsFile_AndFileNotExist_ThenReturnPath()
            {
                var result = _sut.GetPathExists(Path.Combine(WorkingDir, Guid.NewGuid() + ".txt"));

                Assert.That(result, Is.EqualTo(WorkingDir));
            }

            [Test]
            public void WhenPathIsDir_AndDirNotExist_ThenReturnPath()
            {
                var result = _sut.GetPathExists(Path.Combine(WorkingDir, Guid.NewGuid().ToString()));

                Assert.That(result, Is.EqualTo(WorkingDir));
            }

            [Test]
            public void WhenPathIsFile_AndFileAndParentNotExist_ThenReturnPath()
            {
                var path = Path.Combine(WorkingDir, @"Test1\Test2\" + Guid.NewGuid() + ".txt");

                var result = _sut.GetPathExists(path);

                Assert.That(result, Is.EqualTo(WorkingDir));
            }

            [Test]
            public void WhenOnlyDriveExists_ThenReturnPath()
            {
                var result = _sut.GetPathExists(@"C:\0af383f9266d4311ad331f8b461148ee\Test1\Test2\test.txt");

                Assert.That(result, Is.EqualTo(@"C:\"));
            }

            [Test]
            public void WhenNoPartsOfPathExist_ThenThrowException()
            {
                Assert.Throws<PathNotFoundException>(() => _sut.GetPathExists(@"I:\Test1\Test2\test.txt"));
            }
        }

        [TestFixture]
        public class IsFile : FileSystemTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();
            }

            [Test]
            public void WhenPathDoesNotExist_ThenThrowException()
            {
                Assert.Throws<PathNotFoundException>(() => _sut.IsFile(@"C:\7b4feb7ab70845a78bec2511b532f55a"));
            }

            [Test]
            public void WhenFileExists_ThenReturnTrue()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test.txt")).Build();

                var result = _sut.IsFile(fileInfo.FullName);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenDirectoryExists_ThenReturnFalse()
            {
                var dirInfo = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDir")).Build();

                var result = _sut.IsFile(dirInfo.FullName);

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class IsDirectory : FileSystemTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();
            }

            [Test]
            public void WhenPathDoesNotExist_ThenThrowException()
            {
                Assert.Throws<PathNotFoundException>(() => _sut.IsDirectory(@"C:\7b4feb7ab70845a78bec2511b532f55a"));
            }

            [Test]
            public void WhenFileExists_ThenReturnFalse()
            {
                var fileInfo = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test.txt")).Build();

                var result = _sut.IsDirectory(fileInfo.FullName);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenDirectoryExists_ThenReturnTrue()
            {
                var dirInfo = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDir")).Build();

                var result = _sut.IsDirectory(dirInfo.FullName);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class FirstExists : FileSystemTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();
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
                var testFile = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test2.txt")).Build();

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
                var testDir = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDirectory")).Build();
                var testFile = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test2.txt")).Build();

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
                var testDir = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDirectory")).Build();
                var testFile = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test2.txt")).Build();

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
        public class Exists : FileSystemTests
        {
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();
            }

            [Test]
            public void WhenNoPathExists_ThenReturnList()
            {
                string[] paths = 
                {
                    Path.Combine(WorkingDir, "test1.txt"),
                    Path.Combine(WorkingDir, "test2.txt")
                };

                var result = _sut.Exists(paths);

                Assert.That(result.First().Path, Is.EqualTo(paths.First()));
                Assert.That(result.First().Exists, Is.False);
                
                Assert.That(result.Second().Path, Is.EqualTo(paths.Second()));
                Assert.That(result.Second().Exists, Is.False);
            }

            [Test]
            public void WhenMixExistingFilesAndDirectories_ThenReturnList()
            {
                var testDir = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDirectory")).Build();
                var testFile = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "test2.txt")).Build();

                string[] paths =
                {
                    testDir.FullName,
                    testFile.FullName,

                    Path.Combine(WorkingDir, "SomeDir"),
                    Path.Combine(WorkingDir, "test1.txt")
                };

                var result = _sut.Exists(paths);

                Assert.That(result.First().Path, Is.EqualTo(paths.First()));
                Assert.That(result.First().Exists, Is.True);
                
                Assert.That(result.Second().Path, Is.EqualTo(paths.Second()));
                Assert.That(result.Second().Exists, Is.True);

                Assert.That(result.Third().Path, Is.EqualTo(paths.Third()));
                Assert.That(result.Third().Exists, Is.False);

                Assert.That(result.Fourth().Path, Is.EqualTo(paths.Fourth()));
                Assert.That(result.Fourth().Exists, Is.False);
            }
        }

        [TestFixture]
        public class MoveFile : FileSystemTests
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

                DirectoryBuilder.InFileSystem.WithPath(_sourceDir).EmptyIfExists().Build();
                DirectoryBuilder.InFileSystem.WithPath(_destinationDir).EmptyIfExists().Build();
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
                return FileBuilder.InFileSystem.WithPath(Path.Combine(_sourceDir, filePath)).WithSize(size).Build();
            }
        }

        [TestFixture]
        public class CopyFile : FileSystemTests
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

                DirectoryBuilder.InFileSystem.WithPath(_sourceDir).EmptyIfExists().Build();
                DirectoryBuilder.InFileSystem.WithPath(_destinationDir).EmptyIfExists().Build();
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
                return FileBuilder.InFileSystem.WithPath(Path.Combine(_sourceDir, filePath)).WithSize(size).Build();
            }
        }

        [TestFixture]
        public class SwapFileNames : FileSystemTests
        {
            private string _filePath1;
            private string _filePath2;
            
            [SetUp]
            public new void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                CreateOrEmptyWorkingDir();

                _filePath1 = Path.Combine(WorkingDir, "file1.txt");
                _filePath2 = Path.Combine(WorkingDir, "file2.txt");
            }

            [Test]
            public void WhenBothFilesExist_ThenSwapFileNames()
            {
                var file1 = FileBuilder.InFileSystem.WithPath(_filePath1).WithSize(1).Build();
                var file2 = FileBuilder.InFileSystem.WithPath(_filePath2).WithSize(2).Build();

                _sut.SwapFileNames(file1, file2);

                AssertFile.SizeEquals(_filePath1, 2);
                AssertFile.SizeEquals(_filePath2, 1);
            }

            [Test]
            public void WhenFirstFileDoesNotExist_ThenThrowException()
            {
                var file1 = new FileInfo(_filePath1);
                var file2 = FileBuilder.InFileSystem.WithPath(_filePath2).WithSize(2).Build();

                Assert.Throws<FileNotFoundException>(() => Act(file1, file2));
            }
            [Test]
            public void WhenSecondFileDoesNotExist_ThenThrowException()
            {
                var file1 = FileBuilder.InFileSystem.WithPath(_filePath1).WithSize(1).Build();
                var file2 = new FileInfo(_filePath2);

                Assert.Throws<FileNotFoundException>(() => Act(file1, file2));
            }

            [Test]
            public void WhenSecondFileDoesNotExist_ThenRenameFirstFileBackToOriginalName()
            {
                var file1 = FileBuilder.InFileSystem.WithPath(_filePath1).WithSize(1).Build();
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
    }
}
