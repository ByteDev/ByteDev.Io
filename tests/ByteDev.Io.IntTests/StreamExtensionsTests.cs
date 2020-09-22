using System.IO;
using System.Reflection;
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
        public class ReadAsString : StreamExtensionsTests
        {
            [SetUp]
            public void SetUp()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());

                var dirInfo = DirectoryTestBuilder.InFileSystem.WithPath(WorkingDir).Build();

                dirInfo.Empty();
            }

            [Test]
            public void WhenFileContainsText_ThenReturnText()
            {
                const string text = "test text";
                var filePath = Path.Combine(WorkingDir, "test1.txt");

                FileTestBuilder.InFileSystem.WithText(text).WithFilePath(filePath).Build();

                var result = File.Open(filePath, FileMode.Open).ReadAsString();

                Assert.That(result, Is.EqualTo(text));
            }
        }
    }
}