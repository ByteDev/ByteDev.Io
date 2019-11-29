using System;
using System.IO;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents a comparer of files.
    /// </summary>
    public class FileComparer
    {
        /// <summary>
        /// Indicates whether <paramref name="sourceFile" /> is bigger than <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Path to the source file.</param>
        /// <param name="destinationFile">Path to the destination file.</param>
        /// <returns>True if <paramref name="sourceFile" /> is bigger than <paramref name="destinationFile" />; otherwise returns false.</returns>
        public static bool IsSourceBigger(string sourceFile, string destinationFile)
        {
            var sourceSize = GetFileSizeInBytes(sourceFile);
            var destinationSize = GetFileSizeInBytesSafe(destinationFile);

            return sourceSize > destinationSize;
        }

        /// <summary>
        /// Indicates whether <paramref name="sourceFile" /> is bigger or equal than <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Path to the source file.</param>
        /// <param name="destinationFile">Path to the destination file.</param>
        /// <returns>True if <paramref name="sourceFile" /> is bigger than or equal to <paramref name="destinationFile" />; otherwise returns false.</returns>
        public static bool IsSourceBiggerOrEqual(string sourceFile, string destinationFile)
        {
            var sourceSize = GetFileSizeInBytes(sourceFile);
            var destinationSize = GetFileSizeInBytesSafe(destinationFile);

            return sourceSize >= destinationSize;
        }

        /// <summary>
        /// Indicates whether <paramref name="sourceFile" /> is bigger or equal than <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Path to the source file.</param>
        /// <param name="destinationFile">Path to the destination file.</param>
        /// <returns>True if <paramref name="sourceFile" /> is modified more recently than <paramref name="destinationFile" />; otherwise return false.</returns>
        /// <exception cref="T:System.IO.FileNotFoundException"> source file does not exist.</exception>
        public static bool IsSourceModifiedMoreRecently(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found.", sourceFile);

            var sourceModified = GetModifiedDateTime(sourceFile);
            var destinationModified = GetModifiedDateTime(destinationFile);

            return sourceModified > destinationModified;
        }

        private static DateTime GetModifiedDateTime(string path)
        {
            return new FileInfo(path).LastWriteTime;
        }

        private static long GetFileSizeInBytes(string path)
        {
            return new FileInfo(path).Length;
        }

        private static long GetFileSizeInBytesSafe(string destinationFile)
        {
            try
            {
                return GetFileSizeInBytes(destinationFile);
            }
            catch (FileNotFoundException)
            {
                return -1;
            }
        }
    }
}