using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ByteDev.Testing.Builders;
using ByteDev.Testing.NUnit;
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
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.txt")).Build();

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
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.txt")).Build();

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
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest1")).Build();
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DirTest2")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test2.txt")).Build();

                var sut = CreateSut();

                sut.DeleteIfExists();

                AssertDir.NotExists(sut);
            }
        }

        [TestFixture]
        public class DeleteIfEmpty : DirectoryInfoExtensionsTests
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

                Assert.Throws<DirectoryNotFoundException>(() => sut.DeleteIfEmpty());
            }

            [Test]
            public void WhenDirectoryIsNotEmpty_ThenDoNotDelete()
            {
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var sut = CreateSut();

                sut.DeleteIfEmpty();

                AssertDir.ContainsFiles(sut, 1);
            }

            [Test]
            public void WhenDirectoryIsEmpty_ThenDelete()
            {
                var sut = CreateSut();

                sut.Empty();

                sut.DeleteIfEmpty();

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
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test2.txt")).Build();

                var sut = CreateSut();

                sut.DeleteFiles();

                AssertDir.HasNoFiles(sut);
            }

            [Test]
            public void WhenExtensionSpecified_ThenDeleteOnlyFilesWithExtension()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test2.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test3.log")).Build();

                var sut = CreateSut();

                sut.DeleteFiles("txt");

                AssertDir.ContainsFiles(sut, 1);
            }

            [Test]
            public void WhenExtensionIsEmpty_ThenDeleteOnlyFilesWithNoExtension()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test1")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test2")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test3.log")).Build();

                var sut = CreateSut();

                sut.DeleteFiles("");

                AssertDir.ContainsFiles(sut, 1);
            }
        }

        [TestFixture]
        public class DeleteFilesExcept : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenContainsFilesNotOnExceptList_ThenDeleteFiles()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test2.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test3.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test4.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test5.txt")).Build();

                var testDir = DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest1")).Build();

                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"DirTest1\Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"DirTest1\Test2.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"DirTest1\Test3.txt")).Build();

                var sut = CreateSut();

                sut.DeleteFilesExcept(new List<string> { "Test1.txt", "Test2.txt" }, false);

                AssertDir.ContainsFiles(sut, 2);
                AssertDir.ContainsFiles(testDir, 3);
            }
        }

        [TestFixture]
        public class DeleteFilesExcept_Recursive : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenContainsFilesNotOnExceptList_ThenDeleteFiles()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test2.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test3.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test4.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("Test5.txt")).Build();

                var testDir = DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest1")).Build();

                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"DirTest1\Test1.txt")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"DirTest1\Test3.txt")).Build();

                var sut = CreateSut();

                sut.DeleteFilesExcept(new List<string> { "Test1.txt", "Test2.txt" }, true);

                AssertDir.ContainsFiles(sut, 2);
                AssertDir.ContainsFiles(testDir, 1);
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
                DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest1")).Build();
                DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("DirTest2")).Build();

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
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public void WhenMatchInBase_ThenDelete()
            {
                var dirToNotDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
                AssertDir.Exists(dirToNotDelete);
            }

            [Test]
            public void WhenCaseInsensitiveMatchInBase_ThenDelete()
            {
                var dirToNotDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "DIRtoDELETE")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
                AssertDir.Exists(dirToNotDelete);
            }

            [Test]
            public void WhenMatchingDirHasFiles_ThenDelete()
            {
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();

                var dirToDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(dirToDelete.FullName, "test.txt")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(1));
                AssertDir.NotExists(dirToDelete);
            }

            [Test]
            public void WhenMatchesInSubDir_ThenDelete()
            {
                var dirToNotDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();
                var dirToDelete1 = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete")).Build();

                var dirToDelete2 = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(dirToNotDelete.FullName, "dirToDelete")).Build();

                var result = CreateSut().DeleteDirectoriesWithName("dirToDelete");

                Assert.That(result, Is.EqualTo(2));
                AssertDir.NotExists(dirToDelete1);
                AssertDir.NotExists(dirToDelete2);
            }
        }

        [TestFixture]
        public class DeleteEmptyDirectories : DirectoryInfoExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenContainsEmptyDirectories_ThenDeleteEmptyDirectories()
            {
                var dirToNotDelete = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToNotDelete")).Build();
                
                var dirToDelete1 = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete1")).Build();
                var dirToDelete2 = DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "dirToDelete2")).Build();

                FileBuilder.InFileSystem.WithPath(Path.Combine(dirToNotDelete.FullName, "test.txt")).Build();

                CreateSut().DeleteEmptyDirectories();

                AssertDir.Exists(dirToNotDelete);
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
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest0.txt")).WithSize(0).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public void WhenFileHasSizeGreaterThanZero_ThenReturnFileSize()
            {
                const long fileSize = 1;

                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest1.txt")).WithSize(fileSize).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(fileSize));
            }

            [Test]
            public void WhenThreeFilesExist_ThenReturnSumOfFileSizes()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest12.txt")).WithSize(20).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(35));
            }

            [Test]
            public void WhenDirectoryHasFilesAndSubDirectoriesWithFiles_ThenReturnSumOfAllFileSizes()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();

                DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir1")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"Dir1\Dir1GetSizeTest1.txt")).WithSize(20).Build();

                var sut = CreateSut();

                var result = sut.GetSize();

                Assert.That(result, Is.EqualTo(15));
            }

            [Test]
            public void WhenDirectoryHasFilesAndSubDirectoriesWithFiles_AndInclude_ThenReturnSumOfAllFileSizes()
            {
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest10.txt")).WithSize(5).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath("GetSizeTest11.txt")).WithSize(10).Build();

                DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir1")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"Dir1\Dir1GetSizeTest1.txt")).WithSize(20).Build();

                DirectoryBuilder.InFileSystem.WithPath(GetAbsolutePath("Dir2")).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"Dir2\Dir2GetSizeTest1.txt")).WithSize(30).Build();
                FileBuilder.InFileSystem.WithPath(GetAbsolutePath(@"Dir2\Dir2GetSizeTest2.txt")).WithSize(35).Build();

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
                FileBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "Test1.txt")).Build();

                var sut = new DirectoryInfo(WorkingDir);

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }

            [Test]
            public void WhenHasDirectory_ThenReturnFalse()
            {
                DirectoryBuilder.InFileSystem.WithPath(Path.Combine(WorkingDir, "NewFolder")).Build();

                var sut = new DirectoryInfo(WorkingDir);

                var result = sut.IsEmpty();

                Assert.That(result, Is.False);
            }
        }
    }
}
