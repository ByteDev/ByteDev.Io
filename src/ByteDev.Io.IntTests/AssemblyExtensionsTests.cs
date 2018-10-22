using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
        private const string ExistingEmbeddedFile = "EmbeddedResource1.txt";
        private const string NotExistingEmbeddedFile = "SomeFileNotExist.txt";

        [TestFixture]
        public class GetManifestResourceName : AssemblyExtensionsTests
        {
            [Test]
            public void WhenEmbeddedFileExists_ThenReturnResourceName()
            {
                var result = CreateSut().GetManifestResourceName(ExistingEmbeddedFile);

                Assert.That(result, Is.EqualTo($"ByteDev.Io.IntTests.AssemblyTestFiles.{ExistingEmbeddedFile}"));
            }

            [Test]
            public void WhenEmbeddedFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => CreateSut().GetManifestResourceName(NotExistingEmbeddedFile));
            }

            private static Assembly CreateSut()
            {
                return typeof(AssemblyExtensionsTests).Assembly;
            }
        }
    }
}