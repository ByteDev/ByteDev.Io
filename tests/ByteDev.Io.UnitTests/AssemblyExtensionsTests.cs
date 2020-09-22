using System;
using NUnit.Framework;

namespace ByteDev.Io.UnitTests
{
    [TestFixture]
    public class AssemblyExtensionsTests
    {
        [TestFixture]
        public class GetManifestResourceName : AssemblyExtensionsTests
        {
            [Test]
            public void WhenAssemblyIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => AssemblyExtensions.GetManifestResourceName(null, "SomeFile.txt"));
            }
        }
    }
}