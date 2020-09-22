using System;
using System.IO;
using System.Reflection;
using ByteDev.Io.FileCommands.FileCopyCommands;
using ByteDev.Testing.Nunit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.FileCommands.FileCopyCommands
{
    [TestFixture]
    public class FileCopySourceIsNewerOverwriteCommandTest : FileCommandTestBase
    {
        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

            SetupSourceDir();
            SetupDestinationDir();
        }

        [Test]
        public void WhenDestinationFileDoesNotExist_ThenCopyFile()
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
        public void WhenSourceFileIsNewer_ThenCopyFile()
        {
            var sourceFile = CreateSourceFile(FileName1);
            var destinationFile = CreateDestinationFile(FileName1);

            PauseHalfSecond();

            AppendCharToFile(sourceFile.FullName);

            var result = Act(sourceFile.FullName, destinationFile.FullName);

            AssertFile.Exists(sourceFile);
            AssertFile.Exists(result);
        }

        [Test]
        public void WhenSourceFileIsOlder_ThenThrowException()
        {
            var sourceFile = CreateSourceFile(FileName1);
            var destinationFile = CreateDestinationFile(FileName1);

            PauseHalfSecond();

            AppendCharToFile(destinationFile.FullName);

            Assert.Throws<InvalidOperationException>(() => Act(sourceFile.FullName, destinationFile.FullName));
        }
        
        private FileInfo Act(string sourceFile, string destinationFile)
        {
            var command = new FileCopySourceIsNewerOverwriteCommand(sourceFile, destinationFile);

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