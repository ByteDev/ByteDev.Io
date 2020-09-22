using System.IO;
using System.Reflection;
using ByteDev.Io.IntTests.TestFiles;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
        [TestFixture]
        public class GetManifestResourceName : AssemblyExtensionsTests
        {
            [Test]
            public void WhenEmbeddedFileExists_ThenReturnResourceName()
            {
                var result = CreateSut().GetManifestResourceName(TestFileNames.ExistingEmbeddedFile);

                Assert.That(result, Is.EqualTo($"ByteDev.Io.IntTests.TestFiles.{TestFileNames.ExistingEmbeddedFile}"));
            }

            [Test]
            public void WhenEmbeddedFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => CreateSut().GetManifestResourceName(TestFileNames.NotExistingEmbeddedFile));
            }

            private static Assembly CreateSut()
            {
                return typeof(AssemblyExtensionsTests).Assembly;
            }
        }
    }
}