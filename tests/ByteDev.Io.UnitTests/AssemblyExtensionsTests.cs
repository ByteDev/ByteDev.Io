using System;
using System.Reflection;
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
                Assembly sut = null;

                Assert.Throws<ArgumentNullException>(() => sut.GetManifestResourceName("SomeFile.txt"));
            }
        }
    }
}