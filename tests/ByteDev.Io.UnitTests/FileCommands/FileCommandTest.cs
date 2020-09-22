using System;
using ByteDev.Io.FileCommands.FileCopyCommands;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests.FileCommands
{
    [TestFixture]
    public class FileCommandTest
    {
        private const string Source = "source.txt";
        private const string Destination = "destination.txt";

        [Test]
        public void WhenSourceIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new FileCopyExistsThrowExceptionCommand(null, Destination));
        }

        [Test]
        public void WhenSourceIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new FileCopyExistsThrowExceptionCommand(string.Empty, Destination));
        }

        [Test]
        public void WhenDestinationIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new FileCopyExistsThrowExceptionCommand(Source, string.Empty));
        }

        [Test]
        public void WhenDestinationIsEmpty_ThenThrowException()
        {
            Assert.Throws<ArgumentException>(() => new FileCopyExistsThrowExceptionCommand(Source, string.Empty));
        }

    }
}