using System;
using ByteDev.Io.FileCommands;
using ByteDev.Io.FileCommands.FileMoveCommands;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests.FileCommands
{
    [TestFixture]
    public class FileMoveCommandFactoryTest
    {
        private FileMoveCommandFactory _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new FileMoveCommandFactory();
        }

        [TestFixture]
        public class Create : FileMoveCommandFactoryTest
        {
            [Test]
            public void WhenTypeNotHandled_ThenThrowException()
            {
                Assert.Throws<InvalidOperationException>(() => _sut.Create((FileOperationBehaviourType)int.MaxValue, "sourceFile", "destinationFile"));
            }

            [TestCase(FileOperationBehaviourType.DestExistsDoNothing, typeof(FileMoveExistsDoNothingCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsOverwrite, typeof(FileMoveExistsOverwriteCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsRenameWithNumber, typeof(FileMoveExistsRenameWithNumberCommand))]
            [TestCase(FileOperationBehaviourType.DestExistsThrowException, typeof(FileMoveExistsThrowExceptionCommand))]
            [TestCase(FileOperationBehaviourType.SourceSizeGreaterOverwrite, typeof(FileMoveSourceSizeGreaterOverwriteCommand))]
            [TestCase(FileOperationBehaviourType.SourceIsNewerOverwrite, typeof(FileMoveSourceIsNewerOverwriteCommand))]
            public void WhenTypeHandled_ThenReturnCorrectCommandType(FileOperationBehaviourType type, Type expectedCommandType)
            {
                var result = _sut.Create(type, "sourceFile", "destinationFile");

                Assert.That(result.GetType(), Is.EqualTo(expectedCommandType));
            }
        }
    }
}