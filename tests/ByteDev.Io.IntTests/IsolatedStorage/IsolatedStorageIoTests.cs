using System;
using System.IO.IsolatedStorage;
using ByteDev.Io.IsolatedStorage;
using NUnit.Framework;

namespace ByteDev.Io.IntTests.IsolatedStorage
{
    [TestFixture]
    [NonParallelizable]
    public class IsolatedStorageIoTests
    {
        private IsolatedStorageIo _sut;
        private IsolatedStorageFileName _fileName;

        [SetUp]
        public void SetUp()
        {
            _sut = new IsolatedStorageIo(IsolatedStorageFileType.UserStoreForApplication);

            _fileName = new IsolatedStorageFileName("IsolatedStorageIoTests", new Version(1, 0), ".txt");

            _sut.Delete(_fileName);
        }

        [TestFixture]
        public class Write : IsolatedStorageIoTests
        {
            [Test]
            public void WhenFileDoesNotExist_ThenWriteToIsolatedStorage()
            {
                _sut.Write(_fileName, "Test data");

                Assert.That(_sut.Exists(_fileName), Is.True); 
            }

            [Test]
            public void WhenFileExists_ThenOverWrite()
            {
                _sut.Write(_fileName, "Test data 1");

                _sut.Write(_fileName, "Test data 2");

                var data = _sut.Read(_fileName);

                Assert.That(data, Is.EqualTo("Test data 2"));
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

            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.Throws<IsolatedStorageException>(() => _ = _sut.Read(_fileName));
            }
        }

        [TestFixture]
        public class Exists : IsolatedStorageIoTests
        {
            [Test]
            public void WhenFileExists_ThenReturnTrue()
            {
                _sut.Write(_fileName, "some data");

                var result = _sut.Exists(_fileName);

                Assert.That(result, Is.True);
            }

            [Test]
            public void WhenFileDoesNotExist_ThenReturnFalse()
            {
                var result = _sut.Exists(_fileName);

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class Delete : IsolatedStorageIoTests
        {
            [Test]
            public void WhenFileExists_ThenDelete()
            {
                _sut.Write(_fileName, "some data");

                _sut.Delete(_fileName);

                Assert.That(_sut.Exists(_fileName), Is.False);
            }

            [Test]
            public void WhenFileDoesNotExist_ThenDoNothing()
            {
                Assert.DoesNotThrow(() => _sut.Delete(_fileName));
            }
        }
    }
}