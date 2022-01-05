using System;
using System.IO;

namespace ByteDev.Io.Locking
{
    /// <summary>
    /// Represents a way to manage the locking of files.
    /// </summary>
    public static class FileLocker
    {
        /// <summary>
        /// Locks a file. This method will create a corresponding .lock file if it does not exist.
        /// </summary>
        /// <param name="filePath">File to lock.</param>
        /// <returns>File lock info.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="filePath" /> is null or empty.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        /// <exception cref="T:System.InvalidOperationException">Lock file cannot be created as it already exists.</exception>
        public static FileLockInfo Lock(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path was null or empty.");

            return Lock(new FileInfo(filePath));
        }

        /// <summary>
        /// Locks a file. This method will create a corresponding .lock file if it does not exist.
        /// </summary>
        /// <param name="fileInfo">File to lock.</param>
        /// <returns>File lock info.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo" /> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        /// <exception cref="T:System.InvalidOperationException">Lock file cannot be created as it already exists.</exception>
        public static FileLockInfo Lock(FileInfo fileInfo)
        {
            if (fileInfo == null) 
                throw new ArgumentNullException(nameof(fileInfo));

            ThrowIfNotExists(fileInfo);
            
            var fileLockInfo = new FileLockInfo(fileInfo);

            if (fileLockInfo.LockFile.Exists)
                throw new InvalidOperationException($"Lock file: '{fileLockInfo.LockFile.FullName}' cannot be created as it already exists.");

            CreateLockFile(fileLockInfo.LockFile.FullName);
            
            return fileLockInfo;
        }

        /// <summary>
        /// Determines if a file is locked using <see cref="T:ByteDev.Io.Locking.FileLocker" />.
        /// </summary>
        /// <param name="filePath">File to check.</param>
        /// <returns>True if the file is locked; otherwise false.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="filePath" /> was null or empty.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        public static bool IsLocked(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path was null or empty.");

            return IsLocked(new FileInfo(filePath));
        }

        /// <summary>
        /// Determines if a file is locked using <see cref="T:ByteDev.Io.Locking.FileLocker" />.
        /// </summary>
        /// <param name="fileInfo">File to check.</param>
        /// <returns>True if the file is locked; otherwise false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo" /> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        public static bool IsLocked(FileInfo fileInfo)
        {
            if (fileInfo == null) 
                throw new ArgumentNullException(nameof(fileInfo));

            ThrowIfNotExists(fileInfo);

            var fileLockInfo = new FileLockInfo(fileInfo);

            return fileLockInfo.LockFile.Exists;
        }

        /// <summary>
        /// Unlocks a file. This method will delete a corresponding .lock file if it exists.
        /// </summary>
        /// <param name="filePath">File to unlock.</param>
        /// <exception cref="T:System.ArgumentException"><paramref name="filePath" /> was null or empty.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        public static void Unlock(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path was null or empty.");

            Unlock(new FileInfo(filePath));
        }

        /// <summary>
        /// Unlocks a file. This method will delete a corresponding .lock file if it exists.
        /// </summary>
        /// <param name="fileInfo">File to unlock.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo" /> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">File does not exist.</exception>
        public static void Unlock(FileInfo fileInfo)
        {
            if (fileInfo == null) 
                throw new ArgumentNullException(nameof(fileInfo));

            ThrowIfNotExists(fileInfo);

            var fileLockInfo = new FileLockInfo(fileInfo);

            fileLockInfo.LockFile.DeleteIfExists();
        }
        
        private static void CreateLockFile(string filePath)
        {
            using (new FileStream(filePath, FileMode.CreateNew))
            {
                // Creates empty file, IOException thrown if file already exists
            }
        }

        private static void ThrowIfNotExists(FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
                throw new FileNotFoundException("File does not exist.", fileInfo.FullName);
        }
    }
}