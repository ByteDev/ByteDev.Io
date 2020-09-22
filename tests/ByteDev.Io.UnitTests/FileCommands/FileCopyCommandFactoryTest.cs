using System;
using ByteDev.Io.FileCommands;
using ByteDev.Io.FileCommands.FileCopyCommands;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests.FileCommands
{
    [TestFixture]
    public class FileCopyCommandFactoryTest
    {
        private FileCopyCommandFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileCopyCommandFactory();
        }

        [TestFixture]
        public class Create : FileCopyCommandFactoryTest
        {
            [Test]
            public void WhenTypeNotHandled_ThenThrowException()
            {
                Assert.Throws<InvalidOperationException>(() => _sut.Create((FileOperationBehaviourType)int.MaxValue, "sourceFile", "destinationFile"));
            }

            [TestCase(FileOperationBehaviourType.DestExistsDoNothing, typeof(FileCopyExistsDoNothingCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsOverwrite, typeof(FileCopyExistsOverwriteCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsRenameWithNumber, typeof(FileCopyExistsRenameWithNumberCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsThrowException, typeof(FileCopyExistsThrowExceptionCommand))]
            [TestCase(FileOperationBehaviourType.SourceSizeGreaterOverwrite, typeof(FileCopySourceSizeGreaterOverwriteCommand))]
            [TestCase(FileOperationBehaviourType.SourceIsNewerOverwrite, typeof(FileCopySourceIsNewerOverwriteCommand))]
            public void WhenTypeHandled_ThenReturnCorrectCommandType(FileOperationBehaviourType type, Type expectedCommandType)
            {
                var result = _sut.Create(type, "sourceFile", "destinationFile");

                Assert.That(result.GetType(), Is.EqualTo(expectedCommandType));
            }
        }
    }
}