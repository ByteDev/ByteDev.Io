using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    public class FileInfoExtensionsTests
    {
        [TestFixture]
        public class GetNextAvailableFileName : FileInfoExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.GetNextAvailableFileName(null));
            }
        }

        [TestFixture]
        public class HasExtension : FileInfoExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.HasExtension(null));
            }

            [Test]
            public void WhenFileHasExtension_ThenReturnTrue()
            {
                var sut = new FileInfo(@"C:\Temp\Test.txt");

                var result = sut.HasExtension();

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenFileHasNoExtension_ThenReturnFalse()
            {
                var sut = new FileInfo(@"C:\Temp\Test");

                var result = sut.HasExtension();

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class GetExtension : FileInfoExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.GetExtension(null));
            }

            [Test]
            public void WhenFileHasNoExtension_ThenReturnEmpty()
            {
                var sut = new FileInfo(@"C:\Temp\Test");

                var result = sut.GetExtension();

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void WhenFileHasExtension_ThenReturnExtension()
            {
                var sut = new FileInfo(@"C:\Temp\Test.txt");

                var result = sut.GetExtension();

                Assert.That(result, Is.EqualTo(".txt"));
            }

            [Test]
            public void WhenFileHasExtension_AndNotIncludeDotPrefix_ThenReturnExtension()
            {
                var sut = new FileInfo(@"C:\Temp\Test.txt");

                var result = sut.GetExtension(false);

                Assert.That(result, Is.EqualTo("txt"));
            }

            [Test]
            public void WhenFileHasNoExtension_AndNotIncludeDotPrefix_ThenReturnEmpty()
            {
                var sut = new FileInfo(@"C:\Temp\Test");

                var result = sut.GetExtension(false);

                Assert.That(result, Is.Empty);
            }
        }
        
        [TestFixture]
        public class AddExtension : FileInfoExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.AddExtension(null, ".txt"));
            }

            [Test]
            public void WhenExtensionIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => new FileInfo(@"C:\").AddExtension(null));
            }
        }

        [TestFixture]
        public class RenameExtension : FileInfoExtensionsTests
        {
            [Test]
            public void WhenIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.RenameExtension(null, ".txt"));
            }

            [Test]
            public void WhenNewExtensionIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => new FileInfo(@"C:\").RenameExtension(null));
            }
        }

        [TestFixture]
        public class DeleteLines : FileInfoExtensionsTests
        {
            [Test]
            public void WhenSourceIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.DeleteLines(null, new List<int>(), @"C:\Test.txt"));
            }

            [Test]
            public void WhenLineNumbersIsNull_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                Assert.Throws<ArgumentNullException>(() => sut.DeleteLines(null, @"C:\Test.txt"));
            }

            [Test]
            public void WhenHasLineNumerLessThanOne_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                Assert.Throws<ArgumentOutOfRangeException>(() => sut.DeleteLines(new List<int> { 1, 2, 0 }, @"C:\Temp\NewFile.txt"));
            }

            [Test]
            public void WhenOriginalFileAndTargetAreSamePath_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                var ex = Assert.Throws<ArgumentException>(() => sut.DeleteLines(new List<int> { 1 }, sut.FullName));
                Assert.That(ex.Message, Is.EqualTo("Source and target file paths are the same."));
            }
        }

        [TestFixture]
        public class ReplaceLines : FileInfoExtensionsTests
        {
            [Test]
            public void WhenSourceIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => FileInfoExtensions.ReplaceLines(null, new Dictionary<int, string>(), @"C:\Test.txt"));
            }

            [Test]
            public void WhenNewLinesIsNull_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                Assert.Throws<ArgumentNullException>(() => sut.ReplaceLines(null, @"C:\Test.txt"));
            }

            [Test]
            public void WhenHasLineNumerLessThanOne_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                var newLines = new Dictionary<int, string>
                {
                    { 1, "New Line 1" },
                    { 0, "New Line 0" }
                };

                Assert.Throws<ArgumentOutOfRangeException>(() => sut.ReplaceLines(newLines, @"C:\Temp\NewFile.txt"));
            }

            [Test]
            public void WhenOriginalFileAndTargetAreSamePath_ThenThrowException()
            {
                var sut = new FileInfo(@"C:\Temp\Output.txt");

                var newLines = new Dictionary<int, string>
                {
                    { 1, "New Line 1" }
                };

                var ex = Assert.Throws<ArgumentException>(() => sut.ReplaceLines(newLines, sut.FullName));
                Assert.That(ex.Message, Is.EqualTo("Source and target file paths are the same."));
            }
        }
    }
}