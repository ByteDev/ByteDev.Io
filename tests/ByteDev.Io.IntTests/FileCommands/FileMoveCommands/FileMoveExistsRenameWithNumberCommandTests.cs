using System.IO;
using System.Reflection;
using ByteDev.Io.FileCommands.FileMoveCommands;
using ByteDev.Testing.NUnit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.FileCommands.FileMoveCommands
{
    [TestFixture]
    public class FileMoveExistsRenameWithNumberCommandTests : FileCommandTestBase
    {
        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

            SetupSourceDir();
            SetupDestinationDir();
        }

        [Test]
        public void WhenDestinationFileDoesNotExist_ThenMoveFile()
        {
            var sourceFile = CreateSourceFile(FileName1);

            var result = Act(sourceFile.FullName, Path.Combine(DestinationDir, FileName1));

            AssertFile.NotExists(sourceFile);
            AssertFile.Exists(result);
        }

        [Test]
        public void WhenDestinationFileExists_ThenMoveAndRename()
        {
            var origFileName = "Test1.txt";
            var newFileName = "Test1 (2).txt";

            var sourceFile = CreateSourceFile(origFileName, 1);
            var destinationFile = CreateDestinationFile(origFileName, 10);

            var result = Act(sourceFile.FullName, destinationFile.FullName);

            Assert.That(result.Name, Is.EqualTo(newFileName));

            AssertFile.NotExists(sourceFile);
            AssertFile.SizeEquals(destinationFile, 10);
            AssertFile.SizeEquals(result.FullName, 1);
        }

        [Test]
        public void WhenDestinationFile_AndNextFileExists_ThenMoveAndRename()
        {
            var origFileName = "Test1.txt";
            var destFile2 = "Test1 (2).txt";
            var newFileName = "Test1 (3).txt";

            var sourceFileInfo = CreateSourceFile(origFileName, 1);
            var destFile1Info = CreateDestinationFile(origFileName, 10);
            var destFile2Info = CreateDestinationFile(destFile2, 20);

            var result = Act(sourceFileInfo.FullName, destFile1Info.FullName);

            Assert.That(result.Name, Is.EqualTo(newFileName));

            AssertFile.NotExists(sourceFileInfo);
            AssertFile.SizeEquals(destFile1Info, 10);
            AssertFile.SizeEquals(destFile2Info, 20);
            AssertFile.SizeEquals(result.FullName, 1);
        }

        private FileInfo Act(string sourceFile, string destinationFile)
        {
            var command = new FileMoveExistsRenameWithNumberCommand(sourceFile, destinationFile);

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