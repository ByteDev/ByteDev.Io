using System.IO;
using System.Reflection;
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
