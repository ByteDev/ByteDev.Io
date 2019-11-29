using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.FileInfo" />.
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Gets the next available file name, Windows Explorer style.
        /// For example: "Test.txt", "Test (2).txt", "Test (3).txt" etc.
        /// </summary>
        /// <param name="source">File info to perform the operation on.</param>
        /// <returns>The next available file name.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static FileInfo GetNextAvailableFileName(this FileInfo source)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            var path = source.FullName;
            var dir = Path.GetDirectoryName(path);

            while (File.Exists(path))
            {
                var fileName = Path.GetFileName(path);

                path = Path.Combine(dir, GetNextFileNameWithNumber(fileName));
            }

            return new FileInfo(path);
        }

        /// <summary>
        /// Rename the file extension.
        /// </summary>
        /// <param name="source">The file to rename.</param>
        /// <param name="newExtension">New extension.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="newExtension" /> is null.</exception>
        public static void RenameExtension(this FileInfo source, string newExtension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (newExtension == null)
                throw new ArgumentNullException(nameof(newExtension));

            var newPath = Path.Combine(source.DirectoryName, Path.GetFileNameWithoutExtension(source.FullName) + AddExtensionDotPrefix(newExtension));

            source.MoveTo(newPath);
        }

        /// <summary>
        /// Remove the file extension.
        /// </summary>
        /// <param name="source">The file to remove extension.</param>
        public static void RemoveExtension(this FileInfo source)
        {
            RenameExtension(source, string.Empty);
        }

        /// <summary>
        /// Add a file extension to a file that does not have an extension.
        /// </summary>
        /// <param name="source">The file to add the extension to.</param>
        /// <param name="extension">The extension to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="extension" /> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">File already has an extension.</exception>
        public static void AddExtension(this FileInfo source, string extension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            if (source.HasExtension())
                throw new InvalidOperationException("File already has an extension.");

            source.MoveTo(source.FullName + AddExtensionDotPrefix(extension));
        }

        /// <summary>
        /// Indicates whether the file has an extension.
        /// </summary>
        /// <param name="source">The file to check.</param>
        /// <returns>True if the file has an extension; otherwise returns false.</returns>
        public static bool HasExtension(this FileInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return !string.IsNullOrEmpty(Path.GetExtension(source.FullName));
        }

        private static string AddExtensionDotPrefix(string extension)
        {
            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
                return "." + extension;

            return extension;
        }

        private static string GetNextFileNameWithNumber(string fileName)
        {
            var fileExt = Path.GetExtension(fileName);
            var fileNameNoExten = Path.GetFileNameWithoutExtension(fileName);

            const string pattern = @" \([\d]+\)$";

            var match = Regex.Match(fileNameNoExten, pattern);
            var newNumber = 2;

            if (!string.IsNullOrEmpty(match.Value))
            {
                var value = match.Value.Trim().Replace("(", string.Empty).Replace(")", string.Empty);
                newNumber = int.Parse(value) + 1;
            }

            var newName = Regex.Replace(fileNameNoExten, pattern, string.Empty);

            return $"{newName} ({newNumber})" + fileExt;
        }
    }
}
