using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Testing.Builders;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class DirectoryInfoGetFilesExtensionsTests : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        private DirectoryInfo Createsut()
        {
            return Createsut(WorkingDir);
        }

        private DirectoryInfo Createsut(string path)
        {
            return new DirectoryInfo(path);
        }

        [TestFixture]
        public class GetImageFiles : DirectoryInfoGetFilesExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenDirDoesNotExist_ThenThrowException()
            {
                var path = @"C:\" + Path.GetRandomFileName();

                var sut = new DirectoryInfo(path);

                Assert.Throws<DirectoryNotFoundException>(() => sut.GetImageFiles());
            }

            [Test]
            public void WhenDirHasNoImages_ThenReturnEmpty()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "NotImage.txt")).Build();

                var result = Createsut().GetImageFiles();

                Assert.That(result.Count(), Is.EqualTo(0));
            }

            [Test]
            public void WhenDirHasImages_AndNonImage_ThenReturnTheImages()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "NotImage.txt")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Image.jpg")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Image.gif")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Image.png")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Image.jpeg")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Image.bmp")).Build();

                var result = Createsut().GetImageFiles().ToList();

                Assert.That(result.Count, Is.EqualTo(5));
            }
        }

        [TestFixture]
        public class GetFilesByExtensions : DirectoryInfoGetFilesExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenTwoTextFilesExist_ThenReturnTwoTextFiles()
            {
                EmptyWorkingDir();

                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.text")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.gif")).Build();

                var result = Createsut().GetFilesByExtensions(".txt", "text");

                Assert.That(result.Count(), Is.EqualTo(2));
            }
        }
    }
}