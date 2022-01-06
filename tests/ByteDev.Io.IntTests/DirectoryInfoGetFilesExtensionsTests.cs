using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
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

        private DirectoryInfo CreateSut()
        {
            return CreateSut(WorkingDir);
        }

        private DirectoryInfo CreateSut(string path)
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

                var result = CreateSut().GetImageFiles();

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

                var result = CreateSut().GetImageFiles().ToList();

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
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.text")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.gif")).Build();

                var result = CreateSut().GetFilesByExtensions(".txt", "text");

                Assert.That(result.Count(), Is.EqualTo(2));
            }
        }

        [TestFixture]
        public class GetLastModifiedFile : DirectoryInfoGetFilesExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenDirectoryIsEmpty_ThenReturNull()
            {
                var result = CreateSut().GetLastModifiedFile();

                Assert.That(result, Is.Null);
            }

            [Test]
            public void WhenDirectoryHasNoFiles_ThenReturnNull()
            {
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "SubDir")).Build();

                var result = CreateSut().GetLastModifiedFile();

                Assert.That(result, Is.Null);
            }

            [Test]
            public void WhenDirectoryContainsOneFile_ThenReturnFile()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var result = CreateSut().GetLastModifiedFile();

                Assert.That(result.Name, Is.EqualTo("Test1.txt"));
            }

            [Test]
            public void WhenDirectoryContainsTwoFiles_ThenReturnLastModified()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                Thread.Sleep(1000);
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.txt")).Build();
                
                var result = CreateSut().GetLastModifiedFile();

                Assert.That(result.Name, Is.EqualTo("Test2.txt"));
            }

            [Test]
            public void WhenOlderFileIsModified_ThenReturnOlderFile()
            {
                var f1 = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                var f2 = FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.txt")).Build();
                
                using (var writer = f1.AppendText())
                {
                    writer.WriteLine("appended text");
                }
                
                var result = CreateSut().GetLastModifiedFile();

                Assert.That(result.FullName, Is.EqualTo(f1.FullName));
            }
        }
    }
}