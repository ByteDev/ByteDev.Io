using System.IO;

namespace ByteDev.Io.Locking
{
    /// <summary>
    /// Represents info about a file and its corresponding lock file.
    /// </summary>
    public class FileLockInfo
    {
        private const string LockFileExtension = ".lock";
        
        /// <summary>
        /// Original file.
        /// </summary>
        public FileInfo File { get; }

        /// <summary>
        /// Lock file corresponding to the original file.
        /// </summary>
        public FileInfo LockFile { get; }

        internal FileLockInfo(FileInfo fileInfo)
        {
            File = fileInfo;
            LockFile = new FileInfo(fileInfo.FullName + LockFileExtension);
        }
    }
}