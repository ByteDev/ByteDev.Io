using System;
using System.IO;
using System.Linq;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.DirectoryInfo" />.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Recursively create directory path. If any part of the path exists
        /// then no exception will be thrown.
        /// </summary>
        /// <param name="source">Directory path to create.</param>
        public static void CreateDirectory(this DirectoryInfo source)
        {
            if (source.Parent != null) 
                CreateDirectory(source.Parent);

            if (!source.Exists) 
                source.Create();
        }

        /// <summary>
        /// Delete all files and directories.
        /// </summary>
        /// <param name="source">The directory to empty.</param>
        public static void Empty(this DirectoryInfo source)
        {
            DeleteFiles(source);
            DeleteDirectories(source);
        }

        /// <summary>
        /// Delete all files in the source directory.
        /// </summary>
        /// <param name="source">The directory to delete all files from.</param>
        public static void DeleteFiles(this DirectoryInfo source)
        {
            foreach (var file in source.GetFiles())
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Delete all files in the directory with particular extension.
        /// </summary>
        /// <param name="source">The directory to delete all files with the extension from.</param>
        /// <param name="extension">File extension search pattern.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="extension" /> is null.</exception>
        public static void DeleteFiles(this DirectoryInfo source, string extension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var file in source.GetFiles(ExtensionSearchPattern.Create(extension)))
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Delete all directories in the source directory.
        /// </summary>
        /// <param name="source">The directory to delete all directories from.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static void DeleteDirectories(this DirectoryInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var dir in source.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Deletes all directories and sub directories with name <paramref name="directoryName" />.
        /// </summary>
        /// <param name="source">Base directory path.</param>
        /// <param name="directoryName">Name of directories to delete.</param>
        /// <returns>Count of directories deleted.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="directoryName" /> was null or empty.</exception>
        public static int DeleteDirectoriesWithName(this DirectoryInfo source, string directoryName)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            if(string.IsNullOrEmpty(directoryName))
                throw new ArgumentException("Directory name to delete was null or empty.");

            DirectoryInfo[] subDirs = source.GetDirectories(directoryName, SearchOption.AllDirectories);

            foreach (var dir in subDirs)
            {
                Directory.Delete(dir.FullName, true);
            }

            return subDirs.Length;
        }

        /// <summary>
        /// Retrieves the total size of all the directory's files and optionally it's subdirectories.
        /// </summary>
        /// <param name="source">Directory to retrieve the size on.</param>
        /// <param name="includeSubDirectories">True include all subdirectories; false do not.</param>
        /// <returns>Size of <paramref name="source" /> in bytes.</returns>
        public static long GetSize(this DirectoryInfo source, bool includeSubDirectories = false)
        {
            // Go through all files adding their size
            var size = source.GetFiles().Sum(fileInfo => fileInfo.Exists ? fileInfo.Length : 0);

            if (includeSubDirectories)
            {
                // Go through all subdirectories adding their size
                size += source.GetDirectories().Sum(dirInfo => dirInfo.Exists ? dirInfo.GetSize() : 0);
            }

            return size;
        }

        /// <summary>
        /// Indicates whether the directory is empty or not.
        /// </summary>
        /// <param name="source">Directory to perform the operation on.</param>
        /// <returns>True if empty; otherwise false.</returns>
        public static bool IsEmpty(this DirectoryInfo source)
        {
            return !source.EnumerateFileSystemInfos().Any();
        }
    }
}