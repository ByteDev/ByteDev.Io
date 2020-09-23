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

            var filePath = source.FullName;
            var dirPath = Path.GetDirectoryName(filePath);

            if (dirPath == null)
                throw new InvalidOperationException($"Directory path in '{filePath}' could not be determined.");

            while (File.Exists(filePath))
            {
                var fileName = Path.GetFileName(filePath);

                filePath = Path.Combine(dirPath, GetNextFileNameWithNumber(fileName));
            }

            return new FileInfo(filePath);
        }

        /// <summary>
        /// Indicates whether the file has an extension.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <returns>True if the file has an extension; otherwise returns false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static bool HasExtension(this FileInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return !string.IsNullOrEmpty(Path.GetExtension(source.FullName));
        }

        /// <summary>
        /// Gets the file's extension.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="includeDotPrefix">Indicates if the "." extension prefix should be returned if an extension exists.</param>
        /// <returns>File extension as a string.</returns>
        public static string GetExtension(this FileInfo source, bool includeDotPrefix = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var extension = Path.GetExtension(source.FullName);

            if (includeDotPrefix || string.IsNullOrEmpty(extension))
            {
                return extension;
            }

            return extension.Substring(1);
        }

        /// <summary>
        /// Add a file extension to the file. If the file already has an extension then
        /// an exception is thrown.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
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
        /// Adds or renames the file's extension.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="newExtension">New extension.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="newExtension" /> is null.</exception>
        public static void AddOrRenameExtension(this FileInfo source, string newExtension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (newExtension == null)
                throw new ArgumentNullException(nameof(newExtension));

            var newPath = Path.Combine(source.DirectoryName, Path.GetFileNameWithoutExtension(source.FullName) + AddExtensionDotPrefix(newExtension));

            source.MoveTo(newPath);
        }

        /// <summary>
        /// Remove the file's extension if it has one.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        public static void RemoveExtension(this FileInfo source)
        {
            AddOrRenameExtension(source, string.Empty);
        }

        /// <summary>
        /// Indicates whether the file is (probably) binary or not. Implementation checks the first
        /// 8000 characters for the NUL character.
        /// </summary>
        /// <param name="source">The file to check.</param>
        /// <returns>True if the file is (probably) binary; otherwise returns false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static bool IsBinary(this FileInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            const int charsToCheck = 8000;
            const char nulChar = '\0';

            using (var streamReader = new StreamReader(source.FullName))
            {
                for (var i = 0; i < charsToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                        return false;

                    if ((char) streamReader.Read() == nulChar)
                    {
                        Console.WriteLine($"i: {i}");
                        return true;
                    }
                }
            }

            return false;
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
