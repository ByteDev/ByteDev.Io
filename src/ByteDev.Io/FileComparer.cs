using System;
using System.IO;

namespace ByteDev.Io
{
    public class FileComparer
    {
        public static bool IsSourceBigger(string sourceFile, string destinationFile)
        {
            var sourceSize = GetFileSizeInBytes(sourceFile);
            var destinationSize = GetFileSizeInBytesSafe(destinationFile);

            return sourceSize > destinationSize;
        }

        public static bool IsSourceBiggerOrEqual(string sourceFile, string destinationFile)
        {
            var sourceSize = GetFileSizeInBytes(sourceFile);
            var destinationSize = GetFileSizeInBytesSafe(destinationFile);

            return sourceSize >= destinationSize;
        }

        public static bool IsSourceModifiedMoreRecently(string sourceFile, string destinationFile)
        {
            CheckSourceExists(sourceFile);

            var sourceModified = GetModifiedDateTime(sourceFile);
            var destinationModified = GetModifiedDateTime(destinationFile);

            return sourceModified > destinationModified;
        }

        private static void CheckSourceExists(string sourceFile)
        {
            if (!File.Exists(sourceFile))
                throw new FileNotFoundException("Source file not found.", sourceFile);
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