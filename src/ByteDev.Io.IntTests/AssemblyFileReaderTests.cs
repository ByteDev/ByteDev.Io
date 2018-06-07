using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class AssemblyFileReaderTests : IoTestBase
    {
        private AssemblyFileReader _sut;

        private void SetupWorkingDir(string methodName)
        {
            var type = MethodBase.GetCurrentMethod().DeclaringType;
            SetWorkingDir(type, methodName);
        }

        [SetUp]
        public void SetUp()
        {
            SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

            _sut = new AssemblyFileReader(GetType().Assembly);
        }

        [TestFixture]
        public class ReadFileAsync : AssemblyFileReaderTests
        {
            [Test]
            public async Task WhenReadingExistingEmbeddedFile_ThenReturnAsString()
            {
                var result = await ActAsync("EmbeddedResource1.txt");

                Assert.That(result, Is.EqualTo("Embedded Resource 1"));
            }

            [Test]
            public void WhenEmbeddedFileDoesNotExist_ThenThrowException()
            {
                Assert.ThrowsAsync<FileNotFoundException>(() => ActAsync("not-exist.txt"));
            }

            [Test]
            public void WhenReadingExistingContentFile_ThenThrowException()
            {
                Assert.ThrowsAsync<FileNotFoundException>(() => ActAsync("ContentFile1.txt"));
            }

            [Test]
            public void WhenAssemblyHasNotEmbeddedFiles_ThenThrowException()
            {
                var assembly = Assembly.GetAssembly(typeof(AssemblyFileReader));

                var sut = new AssemblyFileReader(assembly);

                Assert.ThrowsAsync<FileNotFoundException>(() => sut.ReadFileAsync("not-exist.txt"));
            }

            private Task<string> ActAsync(string fileName)
            {
                return _sut.ReadFileAsync(fileName);
            }
        }
    }
}