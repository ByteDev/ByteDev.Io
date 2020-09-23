using System;
using System.IO;
using ByteDev.Testing.TestBuilders.FileSystem;

namespace ByteDev.Io.IntTests
{
    public abstract class IoBase
    {
        protected DirectoryInfo DirectoryDoesNotExist = new DirectoryInfo(@"C:\463f8c76c7dd4f97b53cd1ec21fb6b9a");

        private readonly string _intTestsRootDirectory;

        protected IoBase(string intTestsRootDirectory)
        {
            _intTestsRootDirectory = intTestsRootDirectory;
        }

        /// <summary>
        /// Working directory to be used by the method under test
        /// </summary>
        protected string WorkingDir { get; private set; }

        /// <summary>
        /// Set the method under test's working directory for IO
        /// integration tests
        /// </summary>
        /// <param name="type">Type under test</param>
        /// <param name="methodName">FullName of method under test</param>
        protected void SetWorkingDir(Type type, string methodName)
        {
            WorkingDir = Path.Combine(_intTestsRootDirectory, type.Name, GetShortMethodName(methodName));
            CreateOrEmptyWorkingDir();
        }

        protected void EmptyWorkingDir()
        {
            if(string.IsNullOrEmpty(WorkingDir))
            {
                throw new InvalidOperationException("Working directory has not been set");                
            }
            CreateOrEmptyWorkingDir().Empty();
        }

        protected DirectoryInfo CreateOrEmptyWorkingDir()
        {
            return DirectoryTestBuilder.InFileSystem.WithPath(WorkingDir).EmptyIfExists().Build();
        }

        protected string GetAbsolutePath(string path)
        {
            return Path.Combine(WorkingDir, path);
        }

        private static string GetShortMethodName(string methodName)
        {
            var plusPos = methodName.LastIndexOf("+", StringComparison.Ordinal);

            if (plusPos < 0)
            {
                return methodName;
            }
            return methodName.Substring(plusPos + 1);
        }
    }
}
