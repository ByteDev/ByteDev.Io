using System.IO;
using System.Reflection;
using ByteDev.Testing.NUnit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileSystemTests : IoTestBase
    {
        private IFileSystem _sut;

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
                var fileInfo = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "test.txt")).Build();

                var result = _sut.IsFile(fileInfo.FullName);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenDirectoryExists_ThenReturnFalse()
            {
                var dirInfo = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDir")).Build();

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
                var fileInfo = FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "test.txt")).Build();

                var result = _sut.IsDirectory(fileInfo.FullName);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenDirectoryExists_ThenReturnTrue()
            {
                var dirInfo = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "TestDir")).Build();

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
    }
}
