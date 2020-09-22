using System.IO;
using System.Reflection;
using ByteDev.Io.FileCommands.FileCopyCommands;
using ByteDev.Testing.Nunit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.FileCommands.FileCopyCommands
{
    [TestFixture]
    public class FileCopyExistsThrowExceptionCommandTest : FileCommandTestBase
    {
        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

            SetupSourceDir();
            SetupDestinationDir();
        }

        [Test]
        public void WhenSourceFileExists_ThenCopyFile()
        {
            var sourceFile = CreateSourceFile(FileName1);
            
            var result = Act(sourceFile.FullName, Path.Combine(DestinationDir, FileName1));

            AssertFile.Exists(sourceFile);
            AssertFile.Exists(result);
        }

        [Test]
        public void WhenSourceFileDoesNotExist_ThenThrowException()
        {
            Assert.Throws<FileNotFoundException>(() => Act(Path.Combine(SourceDir, FileName1), Path.Combine(DestinationDir, FileName1)));
        }

        [Test]
        public void WhenDestinationFileAlreadyExists_ThenThrowException()
        {
            CreateSourceFile(FileName1);
            CreateDestinationFile(FileName1);

            var ex = Assert.Throws<IOException>(() => Act(Path.Combine(SourceDir, FileName1), Path.Combine(DestinationDir, FileName1)));

            Assert.That(ex.Message, Does.Contain("already exists").IgnoreCase);
        }

        private FileInfo Act(string sourceFile, string destinationFile)
        {
            var command = new FileCopyExistsThrowExceptionCommand(sourceFile, destinationFile);

            command.Execute();

            return command.DestinationFileResult;
        }

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }
    }
}