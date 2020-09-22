using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ByteDev.Testing.NUnit;
using ByteDev.Testing.TestBuilders.FileSystem;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class StreamExtensionsTests : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }
        
        [TestFixture]
        public class WriteToFile : StreamExtensionsTests
        {
            [SetUp]
            public void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

                var dirInfo = DirectoryTestBuilder.InFileSystem.WithPath(WorkingDir).Build();

                dirInfo.Empty();
            }

            [Test]
            public void WhenStreamContainsText_ThenWriteToFile()
            {
                var filePath = Path.Combine(WorkingDir, "Stream-WriteToFile1.txt");

                var sut = StreamFactory.Create("ABC");

                sut.WriteToFile(filePath);

                AssertFile.SizeEquals(filePath, 3);
            }

            [Test]
            public void WhenFileAlreadyExists_ThenOverWrite()
            {
                var filePath = Path.Combine(WorkingDir, "Stream-WriteToFile2.txt");

                FileTestBuilder.InFileSystem.WithFilePath(filePath).WithText("A").Build();

                var sut = StreamFactory.Create("ABC");

                sut.WriteToFile(filePath);

                AssertFile.SizeEquals(filePath, 3);
            }
        }

        [TestFixture]
        public class WriteToFileAsync : StreamExtensionsTests
        {
            [SetUp]
            public void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

                var dirInfo = DirectoryTestBuilder.InFileSystem.WithPath(WorkingDir).Build();

                dirInfo.Empty();
            }

            [Test]
            public async Task WhenStreamContainsText_ThenWriteToFile()
            {
                var filePath = Path.Combine(WorkingDir, "Stream-WriteToFile1.txt");

                var sut = StreamFactory.Create("ABC");

                await sut.WriteToFileAsync(filePath);

                AssertFile.SizeEquals(filePath, 3);
            }

            [Test]
            public async Task WhenFileAlreadyExists_ThenOverWrite()
            {
                var filePath = Path.Combine(WorkingDir, "Stream-WriteToFile2.txt");

                FileTestBuilder.InFileSystem.WithFilePath(filePath).WithText("A").Build();

                var sut = StreamFactory.Create("ABC");

                await sut.WriteToFileAsync(filePath);

                AssertFile.SizeEquals(filePath, 3);
            }
        }
    }
}