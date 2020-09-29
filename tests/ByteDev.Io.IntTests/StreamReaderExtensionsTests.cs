using System.IO;
using System.Reflection;
using ByteDev.Testing.Builders;
using NUnit.Framework;

namespace ByteDev.Io.IntTests
{
    [TestFixture]
    public class StreamReaderExtensionsTests : IoTestBase
    {
        private void SetupWorkingDir(string methodName)
        {
            SetWorkingDir(MethodBase.GetCurrentMethod().DeclaringType, methodName);
        }

        [TestFixture]
        public class ReadLineKeepNewLine : StreamReaderExtensionsTests
        {
            [SetUp]
            public void Setup()
            {
                SetupWorkingDir(MethodBase.GetCurrentMethod().DeclaringType.ToString());
            }

            [Test]
            public void WhenFileIsEmpty_ThenReturnNull()
            {
                var file = FileBuilder.InFileSystem.WithPath(GetAbsolutePath("ReadLine1.txt")).Build();

                var result = Act(file);

                Assert.That(result, Is.Null);
            }

            [Test]
            public void WhenFileHasOneLine_ThenReturnLine()
            {
                var file = FileBuilder.InFileSystem
                    .WithPath(GetAbsolutePath("ReadLine2.txt"))
                    .WithText("Line 1")
                    .Build();

                var result = Act(file);

                Assert.That(result, Is.EqualTo("Line 1"));
            }

            [Test]
            public void WhenFileHasMultipleLines_ThenReturnEachLine()
            {
                const string text = "Line 1\r\nLine 2\n\nLine 4";

                var file = FileBuilder.InFileSystem
                    .WithPath(GetAbsolutePath("ReadLine3.txt"))
                    .WithText(text)
                    .Build();

                using (var sr = new StreamReader(file.FullName))
                {
                    Assert.That(sr.ReadLineKeepNewLineChars(), Is.EqualTo("Line 1\r\n"));
                    Assert.That(sr.ReadLineKeepNewLineChars(), Is.EqualTo("Line 2\n"));
                    Assert.That(sr.ReadLineKeepNewLineChars(), Is.EqualTo("\n"));
                    Assert.That(sr.ReadLineKeepNewLineChars(), Is.EqualTo("Line 4"));
                }
            }

            private static string Act(FileInfo file)
            {
                using (var sr = new StreamReader(file.FullName))
                {
                    return sr.ReadLineKeepNewLineChars();
                }
            }
        }
    }
}