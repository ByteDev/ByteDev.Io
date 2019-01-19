using System;
using ByteDev.Io.IsolatedStorage;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.IsolatedStorage
{
    [TestFixture]
    public class IsolatedStorageIoTests
    {
        private IsolatedStorageIo _sut;
        private IsolatedStorageFileName _fileName;

        [SetUp]
        public void SetUp()
        {
            _sut = new IsolatedStorageIo();

            _fileName = new IsolatedStorageFileName("IsolatedStorageIoTests", new Version(1, 0), "txt");

            _sut.Delete(_fileName);
        }

        [TestFixture]
        public class Write : IsolatedStorageIoTests
        {
            [Test]
            public void WhenFileNameIsNotNull_ThenWriteToIsolatedStorage()
            {
                const string content = "Test data";

                _sut.Write(_fileName, content);

                Assert.That(_sut.Exists(_fileName), Is.True); 
            }
        }

        [TestFixture]
        public class Read : IsolatedStorageIoTests
        {
            [Test]
            public void WhenFileExists_ThenReadContents()
            {
                const string content = "Test data";

                _sut.Write(_fileName, content);

                var result = _sut.Read(_fileName);

                Assert.That(result, Is.EqualTo(content));
            }
        }
    }
}