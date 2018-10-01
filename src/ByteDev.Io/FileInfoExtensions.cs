using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ByteDev.Io
{
    public static class FileInfoExtensions
    {
        /// <summary>
        /// Gets the next available file name, Windows Explorer style.
        /// For example: "Test.txt", "Test (2).txt", "Test (3).txt" etc.
        /// </summary>
        public static FileInfo GetNextAvailableFileName(this FileInfo source)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            var path = source.FullName;
            var dir = Path.GetDirectoryName(path);
            var fileExt = Path.GetExtension(path);

            while (File.Exists(path))
            {
                var fileNameNoExten = Path.GetFileNameWithoutExtension(path);

                path = Path.Combine(dir, GetNextFileNameWithNumber(fileNameNoExten) + fileExt);
            }

            return new FileInfo(path);
        }

        public static void RenameExtension(this FileInfo source, string newExtension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (newExtension == null)
                throw new ArgumentNullException(nameof(newExtension));

            var newPath = Path.Combine(source.DirectoryName, Path.GetFileNameWithoutExtension(source.FullName) + AddExtensionDotPrefix(newExtension));

            source.MoveTo(newPath);
        }

        public static void RemoveExtension(this FileInfo source)
        {
            RenameExtension(source, string.Empty);
        }

        public static void AddExtension(this FileInfo source, string extension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (extension == null)
                throw new ArgumentNullException(nameof(extension));

            if (source.HasExtension())
                throw new InvalidOperationException("File already has a file name extension.");

            source.MoveTo(source.FullName + AddExtensionDotPrefix(extension));
        }

        public static bool HasExtension(this FileInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return !string.IsNullOrEmpty(Path.GetExtension(source.FullName));
        }

        private static string AddExtensionDotPrefix(string extension)
        {
            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
            {
                return "." + extension;
            }
            return extension;
        }

        private static string GetNextFileNameWithNumber(string fileNameNoExten)
        {
            const string pattern = @" \([\d]+\)$";

            var match = Regex.Match(fileNameNoExten, pattern);
            var newNumber = 2;

            if (!string.IsNullOrEmpty(match.Value))
            {
                var value = match.Value.Trim().Replace("(", string.Empty).Replace(")", string.Empty);
                newNumber = int.Parse(value) + 1;
            }

            var newName = Regex.Replace(fileNameNoExten, pattern, string.Empty);

            return $"{newName} ({newNumber})";
        }
    }
}
