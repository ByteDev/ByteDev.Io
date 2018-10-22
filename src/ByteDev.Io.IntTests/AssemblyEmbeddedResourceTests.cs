using System.IO;
using System.Reflection;
using ByteDev.Testing.Nunit;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class AssemblyEmbeddedResourceTests : IoTestBase
    {
        private const string ExistingEmbeddedFile = "EmbeddedResource1.txt";

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            EmptyWorkingDir();
        }

        [TestFixture]
        public class CreateFromAssemblyContaining : AssemblyEmbeddedResourceTests
        {
            [Test]
            public void WhenEmbeddedFileExists_ThenSetProperties()
            {
                var sut = AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>(ExistingEmbeddedFile);

                Assert.That(sut.Assembly, Is.EqualTo(typeof(AssemblyEmbeddedResourceTests).Assembly));
                Assert.That(sut.FileName, Is.EqualTo(ExistingEmbeddedFile));
                Assert.That(sut.ResourceName, Is.EqualTo("ByteDev.Io.IntTests.AssemblyTestFiles.EmbeddedResource1.txt"));
            }

            [Test]
            public void WhenAssemblyDoesNotHaveTheEmbeddedFile_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>("SomeFileNotExist.txt"));
            }

            [Test]
            public void WhenAssemblyHasNoEmbeddedFiles_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResource>(ExistingEmbeddedFile));
            }

            [Test]
            public void WhenFileIsContentFile_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResource>("ContentFile1.txt"));
            }
        }

        [TestFixture]
        public class Save : AssemblyEmbeddedResourceTests
        {
            [Test]
            public void WhenNoFileExistsOnDisk_ThenSaveToDisk()
            {
                var sut = AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>("EmbeddedResource1.txt");

                var fileInfo = sut.Save(Path.Combine(WorkingDir, sut.FileName));

                AssertFile.Exists(fileInfo);
            }
        }
    }
}