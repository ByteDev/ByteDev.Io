using System.IO;
using System.Reflection;
using ByteDev.Io.IntTests.TestFiles;
using ByteDev.Testing.Nunit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class AssemblyEmbeddedResourceTests : IoTestBase
    {
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
                var sut = AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>(TestFileNames.ExistingEmbeddedFile);

                Assert.That(sut.Assembly, Is.EqualTo(typeof(AssemblyEmbeddedResourceTests).Assembly));
                Assert.That(sut.FileName, Is.EqualTo(TestFileNames.ExistingEmbeddedFile));
                Assert.That(sut.ResourceName, Is.EqualTo($"ByteDev.Io.IntTests.TestFiles.{TestFileNames.ExistingEmbeddedFile}"));
            }

            [Test]
            public void WhenEmbeddedFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>(TestFileNames.NotExistingEmbeddedFile));
            }

            [Test]
            public void WhenAssemblyHasNoEmbeddedFiles_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResource>(TestFileNames.ExistingEmbeddedFile));
            }

            [Test]
            public void WhenFileIsContentFile_ThenThrowException()
            {
                Assert.Throws<FileNotFoundException>(() => AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResource>(TestFileNames.ExistingContentFile));
            }
        }

        [TestFixture]
        public class Save : AssemblyEmbeddedResourceTests
        {
            [Test]
            public void WhenNoFileExistsOnDisk_ThenSaveToDisk()
            {
                var sut = AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>(TestFileNames.ExistingEmbeddedFile);

                var fileInfo = sut.Save(Path.Combine(WorkingDir, sut.FileName));

                AssertFile.Exists(fileInfo);
            }

            [Test]
            public void WhenFileAlreadyExists_ThenThrowException()
            {
                var saveFilePath = Path.Combine(WorkingDir, TestFileNames.ExistingEmbeddedFile);

                FileTestBuilder.InFileSystem.WithFilePath(saveFilePath).Build();

                var sut = AssemblyEmbeddedResource.CreateFromAssemblyContaining<AssemblyEmbeddedResourceTests>(TestFileNames.ExistingEmbeddedFile);

                Assert.Throws<IOException>(() => sut.Save(saveFilePath));
            }
        }
    }
}