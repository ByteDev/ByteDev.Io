using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class ExtensionSearchPatternTests
    {
        [Test]
        public void WhenIsNull_ThenThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => ExtensionSearchPattern.Create(null));
        }

        [Test]
        public void WhenIsWellFormed_ThenReturnSameString()
        {
            var result = ExtensionSearchPattern.Create("*.txt");

            Assert.That(result, Is.EqualTo("*.txt"));
        }

        [TestCase("", "*.")]
        [TestCase("txt", "*.txt")]
        [TestCase(".txt", "*.txt")]
        [TestCase("*txt", "*.txt")]
        public void WhenIsNotWellFormed_ThenReturnWellFormed(string searchPattern, string expected)
        {
            var result = ExtensionSearchPattern.Create(searchPattern);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}