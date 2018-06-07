using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class FileSizeTest
    {
        protected FileSize CreateBinarySut(long size)
        {
            return new FileSize(size);
        }

        protected FileSize CreateDecimalSut(long size)
        {
            return new FileSize(size, FileSize.MultiplierType.DecimalMultiplier);
        }

        [TestFixture]
        public class Constructor : FileSizeTest
        {
            [Test]
            public void WhenSizeIsLessThanZero_ThenThrowException()
            {
                Assert.Throws<ArgumentOutOfRangeException>(() => CreateBinarySut(-1));
            }
        }

        [TestFixture]
        public class ReadableSize : FileSizeTest
        {
            [TestCase(0, "0 B")]
            [TestCase(1, "1 B")]
            [TestCase(1024, "1 KB")]
            [TestCase(1536, "1.5 KB")]
            [TestCase(1048576, "1 MB")]
            [TestCase(1073741824, "1 GB")]
            [TestCase(1099511627776, "1 TB")]
            [TestCase(1125899906842624, "1 PB")]
            public void WhenMultiplierIsBinary_ThenReturnRepresentation(long size, string expected)
            {
                var sut = CreateBinarySut(size);

                Assert.That(sut.ReadableSize, Is.EqualTo(expected));
            }

            [TestCase(0, "0 B")]
            [TestCase(1, "1 B")]
            [TestCase(1000, "1 KB")]
            [TestCase(1536, "1.5 KB")]
            [TestCase(1000000, "1 MB")]
            [TestCase(1000000000, "1 GB")]
            [TestCase(1000000000000, "1 TB")]
            [TestCase(1000000000000000, "1 PB")]
            public void WhenMultiplierIsDecimal_ThenReturnRepresentation(long size, string expected)
            {
                var sut = CreateDecimalSut(size);

                Assert.That(sut.ReadableSize, Is.EqualTo(expected));
            }
        }

        [TestFixture]
        public class TotalBytes : FileSizeTest
        {
            [Test]
            public void WhenBytesIsZero_ThenReturnZero()
            {
                var sut = CreateBinarySut(0);

                Assert.That(sut.TotalBytes, Is.EqualTo(0));
            }

            [Test]
            public void WhenBytesIsGreaterThanZero_ThenReturnSizeInBytes()
            {
                const long expected = 1024;

                var sut = CreateBinarySut(expected);

                Assert.That(sut.TotalBytes, Is.EqualTo(expected));
            }
        }

        [TestFixture]
        public class TotalKiloBytes : FileSizeTest
        {
            [Test]
            public void WhenBytesIsZero_ThenReturnZero()
            {
                var sut = CreateBinarySut(0);

                Assert.That(sut.TotalKiloBytes, Is.EqualTo(0));
            }

            [Test]
            public void WhenBytesIsLowerThan1024_ThenReturnZero()
            {
                var sut = CreateBinarySut(1023);

                Assert.That(sut.TotalKiloBytes, Is.EqualTo(0));
            }

            [Test]
            public void WhenBytesIs1024_ThenReturnOne()
            {
                var sut = CreateBinarySut(1024);

                Assert.That(sut.TotalKiloBytes, Is.EqualTo(1));
            }

            [Test]
            public void WhenBytesIs1025_ThenReturnOne()
            {
                var sut = CreateBinarySut(1025);

                Assert.That(sut.TotalKiloBytes, Is.EqualTo(1));
            }

            [Test]
            public void WhenBytesIs2048_ThenReturnTwo()
            {
                var sut = CreateBinarySut(2048);

                Assert.That(sut.TotalKiloBytes, Is.EqualTo(2));
            }
        }

        [TestFixture]
        public class TotalMegaBytes : FileSizeTest
        {
            [Test]
            public void WhenBytesIsZero_ThenReturnZero()
            {
                var sut = CreateBinarySut(0);

                Assert.That(sut.TotalMegaBytes, Is.EqualTo(0));
            }
            
            [Test]
            public void WhenBytesIsLowerThan1048576_ThenReturnZero()
            {
                var sut = CreateBinarySut(1048575);

                Assert.That(sut.TotalMegaBytes, Is.EqualTo(0));
            }

            [Test]
            public void WhenBytesIs1048576_ThenReturnOne()
            {
                var sut = CreateBinarySut(1048576);

                Assert.That(sut.TotalMegaBytes, Is.EqualTo(1));
            }

            [Test]
            public void WhenBytesIs1048577_ThenReturnOne()
            {
                var sut = CreateBinarySut(1048577);

                Assert.That(sut.TotalMegaBytes, Is.EqualTo(1));
            }

            [Test]
            public void WhenBytesIs2097152_ThenReturnTwo()
            {
                var sut = CreateBinarySut(2097152);

                Assert.That(sut.TotalMegaBytes, Is.EqualTo(2));
            }
        }
    }
}
