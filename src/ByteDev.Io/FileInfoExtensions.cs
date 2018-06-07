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
