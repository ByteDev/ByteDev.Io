using System.IO;
using System.Reflection;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class FileComparerTest : IoTestBase
    {
        private const string FileName1 = "file1.txt";
        private const string FileName2 = "file2.txt";

        private string _sourceDir;
        private string _destinationDir;

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        private FileInfo CreateSourceFile(string fileName, long size = 0)
        {
            return FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(_sourceDir, fileName)).WithSize(size).Build();
        }

        private FileInfo CreateDestinationFile(string fileName, long size = 0)
        {
            return FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(_destinationDir, fileName)).WithSize(size).Build();
        }

        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            CreateOrEmptyWorkingDir();

            _sourceDir = Path.Combine(WorkingDir, "Source");
            _destinationDir = Path.Combine(WorkingDir, "Destination");

            DirectoryTestBuilder.InFileSystem.WithPath(_sourceDir).EmptyIfExists().Build();
            DirectoryTestBuilder.InFileSystem.WithPath(_destinationDir).EmptyIfExists().Build();
        }

        [TestFixture]
        public class IsSourceBigger : FileComparerTest
        {
            [Test]
            public void WhenSourceDoesNotExist_ThenThrowException()
            {
                var sourceFile = Path.Combine(_sourceDir, FileName2);
                var destinationFile = CreateDestinationFile(FileName1).FullName;

                Assert.Throws<FileNotFoundException>(() => Act(sourceFile, destinationFile));
            }

            [Test]
            public void WhenDestinationDoesNotExist_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1).FullName;
                var destinationFile = Path.Combine(_destinationDir, FileName2);

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceAndDestinationAreEqualSize_ThenReturnFalse()
            {
                var sourceFile = CreateSourceFile(FileName1, 1).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenSourceIsBigger_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1, 2).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceIsSmaller_ThenReturnFalse()
            {
                var sourceFile = CreateSourceFile(FileName1, 1).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 2).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.False);
            }

            private static bool Act(string sourceFile, string destinationFile)
            {
                return FileComparer.IsSourceBigger(sourceFile, destinationFile);
            }
        }

        [TestFixture]
        public class IsSourceBiggerOrEqual : FileComparerTest
        {
            [Test]
            public void WhenSourceDoesNotExist_ThenThrowException()
            {
                var sourceFile = Path.Combine(_sourceDir, FileName2);
                var destinationFile = CreateDestinationFile(FileName1).FullName;

                Assert.Throws<FileNotFoundException>(() => Act(sourceFile, destinationFile));
            }

            [Test]
            public void WhenDestinationDoesNotExist_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1).FullName;
                var destinationFile = Path.Combine(_destinationDir, FileName2);

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceAndDestinationAreEqualSize_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1, 1).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceIsBigger_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1, 2).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceIsSmaller_ThenReturnFalse()
            {
                var sourceFile = CreateSourceFile(FileName1, 1).FullName;
                var destinationFile = CreateDestinationFile(FileName2, 2).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.False);
            }

            private static bool Act(string sourceFile, string destinationFile)
            {
                return FileComparer.IsSourceBiggerOrEqual(sourceFile, destinationFile);
            }
        }

        [TestFixture]
        public class IsSourceModifiedMoreRecently : FileComparerTest
        {
            [Test]
            public void WhenSourceDoesNotExist_ThenThrowException()
            {
                var sourceFile = Path.Combine(_sourceDir, FileName2);
                var destinationFile = CreateDestinationFile(FileName1).FullName;

                Assert.Throws<FileNotFoundException>(() => Act(sourceFile, destinationFile));
            }

            [Test]
            public void WhenDestinationDoesNotExist_ThenReturnTrue()
            {
                var sourceFile = CreateSourceFile(FileName1).FullName;
                var destinationFile = Path.Combine(_destinationDir, FileName2);

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceHasBeenModifiedMoreRecently_ThenReturnTrue()
            {
                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                PauseHalfSecond();

                var sourceFile = CreateSourceFile(FileName1, 1).FullName;
                
                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenSourceHasBeenModifiedLessRecently_ThenReturnFalse()
            {
                var sourceFile = CreateSourceFile(FileName1, 1).FullName;

                PauseHalfSecond();

                var destinationFile = CreateDestinationFile(FileName2, 1).FullName;

                var result = Act(sourceFile, destinationFile);

                Assert.That(result, Is.False);
            }

            private static bool Act(string sourceFile, string destinationFile)
            {
                return FileComparer.IsSourceModifiedMoreRecently(sourceFile, destinationFile);
            }
        }
    }
}