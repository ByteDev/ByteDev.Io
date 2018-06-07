using System.IO;
using System.Linq;
using System.Reflection;
using ByteDev.Testing.Nunit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class DirectoryInfoExtensionsTest : IoTestBase
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
        public class GetImageFiles : DirectoryInfoExtensionsTest
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
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "NotImage.txt")).Build();

                var result = Createsut().GetImageFiles();

                Assert.That(result.Count(), Is.EqualTo(0));
            }

            [Test]
            public void WhenDirHasImages_AndNonImage_ThenReturnTheImages()
            {
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "NotImage.txt")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Image.jpg")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Image.gif")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Image.png")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Image.jpeg")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Image.bmp")).Build();

                var result = Createsut().GetImageFiles().ToList();

                Assert.That(result.Count, Is.EqualTo(5));
            }
        }

        [TestFixture]
        public class GetFilesByExtensions : DirectoryInfoExtensionsTest
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

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test2.text")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.gif")).Build();

                var result = Createsut().GetFilesByExtensions(".txt", "text");

                Assert.That(result.Count(), Is.EqualTo(2));
            }
        }

        [TestFixture]
        public class Empty : DirectoryInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenThereAreTwoFilesAndTwoDirectories_ThenDeleteAllFilesAndDirectories()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test2.txt")).Build();

                var sut = Createsut();

                sut.Empty();

                AssertDir.IsEmpty(sut);
            }
        }
        
        [TestFixture]
        public class DeleteDirectories : DirectoryInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenThereAreTwoDirectories_ThenDeleteAllDirectories()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePathFor("DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePathFor("DirTest2")).Build();

                var sut = Createsut();

                sut.DeleteDirectories();

                AssertDir.HasNoDirectories(sut);
            }
        }

        [TestFixture]
        public class DeleteFiles : DirectoryInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenThereAreTwoFiles_ThenDeleteAllFiles()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("Test2.txt")).Build();

                var sut = Createsut();

                sut.DeleteFiles();

                AssertDir.HasNoFiles(sut);
            }

            [Test]
            public void WhenExtensionSpecified_ThenDeleteOnlyFilesWithExtension()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("Test2.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("Test3.log")).Build();

                var sut = Createsut();

                sut.DeleteFiles("txt");

                AssertDir.ContainsFiles(sut, 1);
            }
        }

        [TestFixture]
        public class GetSize : DirectoryInfoExtensionsTest
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenFileHasSizeZero_ThenReturnZero()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest0.txt")).WithSize(0).Build();

                var sut = Createsut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public void WhenFileHasSizeGreaterThanZero_ThenReturnFileSize()
            {
                const long fileSize = 1;

                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest1.txt")).WithSize(fileSize).Build();

                var sut = Createsut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(fileSize));
            }

            [Test]
            public void WhenThreeFilesExist_ThenReturnSumOfFileSizes()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest10.txt")).WithSize(5).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest11.txt")).WithSize(10).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest12.txt")).WithSize(20).Build();

                var sut = Createsut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(35));
            }

            [Test]
            public void WhenDirectoryHasFilesAndSubDirectoriesWithFiles_ThenReturnSumOfAllFileSizes()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest10.txt")).WithSize(5).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor("GetSizeTest11.txt")).WithSize(10).Build();

                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePathFor("Dir1")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor(@"Dir1\Dir1GetSizeTest1.txt")).WithSize(20).Build();

                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePathFor("Dir2")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor(@"Dir2\Dir2GetSizeTest1.txt")).WithSize(30).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePathFor(@"Dir2\Dir2GetSizeTest2.txt")).WithSize(35).Build();

                var sut = Createsut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(100));
            }
        }

        [TestFixture]
        public class CreateDirectory : DirectoryInfoExtensionsTest
        {
            private DirectoryInfo _sut;

            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ThenCreateDirectoy()
            {
                var path = GetAbsolutePathFor("CD1");
                _sut = Createsut(path);

                _sut.CreateDirectory();

                AssertDir.Exists(path);
            }

            [Test]
            public void WhenDirectoryHasParentDirectoryThatDoesNotExist_ThenCreateDirectorysRecursively()
            {
                var path = GetAbsolutePathFor(@"CDx\CDy\CDz");
                _sut = Createsut(path);

                _sut.CreateDirectory();

                AssertDir.Exists(path);
            }
        }
    }
}
