using System.IO;
using System.Reflection;
using ByteDev.Testing.NUnit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class DirectoryInfoExtensionsTests : IoTestBase
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
        public class CreateDirectory : DirectoryInfoExtensionsTests
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
                var path = GetAbsolutePath("CD1");
                
                _sut = CreateSut(path);

                _sut.CreateDirectory();

                AssertDir.Exists(path);
            }

            [Test]
            public void WhenDirectoryHasParentDirectoryThatDoesNotExist_ThenCreateDirectorysRecursively()
            {
                var path = GetAbsolutePath(@"CDx\CDy\CDz");

                _sut = CreateSut(path);

                _sut.CreateDirectory();

                AssertDir.Exists(path);
            }
        }

        [TestFixture]
        public class Empty : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ThenThrowException()
            {
                var sut = DirectoryDoesNotExist;

                Assert.Throws<DirectoryNotFoundException>(() => sut.Empty());
            }

            [Test]
            public void WhenThereAreFilesAndDirectories_ThenDeleteAllFilesAndDirectories()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test2.txt")).Build();

                var sut = CreateSut();

                sut.Empty();

                AssertDir.IsEmpty(sut);
            }
        }

        [TestFixture]
        public class EmptyIfExists : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ThenDoNothing()
            {
                var sut = DirectoryDoesNotExist;

                Assert.DoesNotThrow(() => sut.EmptyIfExists());
            }

            [Test]
            public void WhenContainsFilesAndDirectories_ThenDeleteAllFilesAndDirectories()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test2.txt")).Build();

                var sut = CreateSut();

                sut.EmptyIfExists();

                AssertDir.IsEmpty(sut);
            }
        }

        [TestFixture]
        public class DeleteIfExists : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenDirectoryDoesNotExist_ThenDoNothing()
            {
                var sut = DirectoryDoesNotExist;

                sut.DeleteIfExists();
            }

            [Test]
            public void WhenContainsFilesAndDirectories_ThenDelete()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test2.txt")).Build();

                var sut = CreateSut();

                sut.DeleteIfExists();

                AssertDir.NotExists(sut);
            }
        }

        [TestFixture]
        public class DeleteFiles : DirectoryInfoExtensionsTests
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
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test2.txt")).Build();

                var sut = CreateSut();

                sut.DeleteFiles();

                AssertDir.HasNoFiles(sut);
            }

            [Test]
            public void WhenExtensionSpecified_ThenDeleteOnlyFilesWithExtension()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test1.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test2.txt")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test3.log")).Build();

                var sut = CreateSut();

                sut.DeleteFiles("txt");

                AssertDir.ContainsFiles(sut, 1);
            }

            [Test]
            public void WhenExtensionIsEmpty_ThenDeleteOnlyFilesWithNoExtension()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test1")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test2")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("Test3.log")).Build();

                var sut = CreateSut();

                sut.DeleteFiles("");

                AssertDir.ContainsFiles(sut, 1);
            }
        }

        [TestFixture]
        public class DeleteDirectories : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenThereAreDirectories_ThenDeleteAllDirectories()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest1")).Build();
                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest2")).Build();

                var sut = CreateSut();

                sut.DeleteDirectories();

                AssertDir.HasNoDirectories(sut);
            }
        }

        [TestFixture]
        public class DeleteDirectoryWithName : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenNoMatches_ThenReturnZero()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public void WhenMatchInBase_ThenDelete()
            {
                var dirToNotDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
                AssertDir.Exists(dirToNotDelete);
            }

            [Test]
            public void WhenCaseInsensitiveMatchInBase_ThenDelete()
            {
                var dirToNotDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DIRtoDELETE")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
                AssertDir.Exists(dirToNotDelete);
            }

            [Test]
            public void WhenMatchingDirHasFiles_ThenDelete()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(dirToDelete.FullName, "test.txt")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
            }

            [Test]
            public void WhenMatchesInSubDir_ThenDelete()
            {
                var dirToNotDelete = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();
                var dirToDelete1 = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                var dirToDelete2 = DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(dirToNotDelete.FullName, "dirToDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(2));
                AssertDir.NotExists(dirToDelete1);
                AssertDir.NotExists(dirToDelete2);
            }
        }

        [TestFixture]
        public class GetSize : DirectoryInfoExtensionsTests
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
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest0.txt")).WithSize(0).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public void WhenFileHasSizeGreaterThanZero_ThenReturnFileSize()
            {
                const long fileSize = 1;

                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest1.txt")).WithSize(fileSize).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(fileSize));
            }

            [Test]
            public void WhenThreeFilesExist_ThenReturnSumOfFileSizes()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest12.txt")).WithSize(20).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(35));
            }

            [Test]
            public void WhenDirectoryHasFilesAndSubDirectoriesWithFiles_ThenReturnSumOfAllFileSizes()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();

                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir1")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath(@"Dir1\Dir1GetSizeTest1.txt")).WithSize(20).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(15));
            }

            [Test]
            public void WhenDirectoryHasFilesAndSubDirectoriesWithFiles_AndInclude_ThenReturnSumOfAllFileSizes()
            {
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();

                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir1")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath(@"Dir1\Dir1GetSizeTest1.txt")).WithSize(20).Build();

                DirectoryTestBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir2")).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath(@"Dir2\Dir2GetSizeTest1.txt")).WithSize(30).Build();
                FileTestBuilder.InFileSystem.WithFilePath(GetAbsolutePath(@"Dir2\Dir2GetSizeTest2.txt")).WithSize(35).Build();

                var sut = CreateSut();

                var result = sut.GetSize(true);

                Assert.That(result, Is.EqualTo(100));
            }
        }

        [TestFixture]
        public class IsEmpty : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
                EmptyWorkingDir();
            }

            [Test]
            public void WhenIsEmpty_ThenReturnTrue()
            {
                var sut = new DirectoryInfo(WorkingDir);

                var result = sut.IsEmpty();

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenHasFile_ThenReturnFalse()
            {
                FileTestBuilder.InFileSystem.WithFilePath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var sut = new DirectoryInfo(WorkingDir);

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenHasDirectory_ThenReturnFalse()
            {
                DirectoryTestBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "NewFolder")).Build();

                var sut = new DirectoryInfo(WorkingDir);

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }
        }
    }
}
