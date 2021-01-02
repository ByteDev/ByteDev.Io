using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ByteDev.Strings;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.FileInfo" />.
    /// </summary>
    public static class FileInfoExtensions
    {
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
                throw new InvalidOperationException($"File: '{source.FullName}' already has an extension.");

            source.MoveTo(source.FullName + extension.AddFileExtensionDotPrefix());
        }

        /// <summary>
        /// Deletes the file if it exists.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static void DeleteIfExists(this FileInfo source)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            try
            {
                source.Delete();
            }
            catch (FileNotFoundException)
            {
                // Do nothing if file does not exist
            }
        }

        /// <summary>
        /// Gets the file's extension.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="includeDotPrefix">Indicates if the "." extension prefix should be returned as part of the extension if an extension exists.</param>
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
        /// Gets the next available file name, Windows Explorer style.
        /// For example: "Test.txt", "Test (2).txt", "Test (3).txt" etc.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
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
        /// Determines whether the file is (probably) binary or not. Implementation checks the first
        /// 8000 characters for a given number of consecutive NUL characters.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="requiredConsecutiveNul">Number of consecutive NUL characters before the file is determined to be binary.</param>
        /// <returns>True if the file is (probably) binary; otherwise returns false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="requiredConsecutiveNul" /> must be one or more.</exception>
        public static bool IsBinary(this FileInfo source, int requiredConsecutiveNul = 1)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (requiredConsecutiveNul < 1)
                throw new ArgumentOutOfRangeException(nameof(requiredConsecutiveNul), "Required number of consecutive NULs must be one or more.");

            const int charsToCheck = 8000;
            const char nulChar = '\0';

            int nulCount = 0;

            using (var streamReader = new StreamReader(source.FullName))
            {
                for (var i = 0; i < charsToCheck; i++)
                {
                    if (streamReader.EndOfStream)
                        return false;

                    if ((char) streamReader.Read() == nulChar)
                    {
                        nulCount++;

                        // Console.WriteLine($"i: {i} ({source.Name})");

                        if (nulCount >= requiredConsecutiveNul)
                            return true;
                    }
                    else
                    {
                        nulCount = 0;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Renames the file's extension. If the file does not have an extension then one will
        /// be added.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="newExtension">New extension.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="newExtension" /> is null.</exception>
        public static void RenameExtension(this FileInfo source, string newExtension)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (newExtension == null)
                throw new ArgumentNullException(nameof(newExtension));

            var newPath = Path.Combine(source.DirectoryName, Path.GetFileNameWithoutExtension(source.FullName) + newExtension.AddFileExtensionDotPrefix());

            source.MoveTo(newPath);
        }

        /// <summary>
        /// Rename the file's extension to lower case.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static void RenameExtensionToLower(this FileInfo source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var extension = source.GetExtension();

            if (!string.IsNullOrEmpty(extension))
            {
                RenameExtension(source, extension.ToLower());
            }
        }

        /// <summary>
        /// Remove the file's extension if it has one.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static void RemoveExtension(this FileInfo source)
        {
            RenameExtension(source, string.Empty);
        }

        /// <summary>
        /// Delete a specific line from a text file saving the new content to a new target text file.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="lineNumber">Line number to delete.</param>
        /// <param name="targetFilePath">New target text file path.</param>
        /// <returns>FileInfo for the new target file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lineNumber" /> cannot be less than one.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targetFilePath" /> cannot be the same as the source path.</exception>
        public static FileInfo DeleteLine(this FileInfo source, int lineNumber, string targetFilePath)
        {
            return DeleteLines(source, new[] {lineNumber}, targetFilePath);
        }

        /// <summary>
        /// Delete specific lines from a text file saving the new content to a new target text file.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="lineNumbers">Line numbers to delete.</param>
        /// <param name="targetFilePath">New target text file path.</param>
        /// <returns>FileInfo for the new target file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="lineNumbers" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lineNumbers" /> cannot be less than one.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targetFilePath" /> cannot be the same as the source path.</exception>
        public static FileInfo DeleteLines(this FileInfo source, ICollection<int> lineNumbers, string targetFilePath)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (lineNumbers == null)
                throw new ArgumentNullException(nameof(lineNumbers));

            if (lineNumbers.Any(n => n < 1))
                throw new ArgumentOutOfRangeException(nameof(lineNumbers), "Line numbers to delete must be one or greater.");

            if (source.FullName == targetFilePath)
                throw new ArgumentException("Source and target file paths are the same.");

            var lineCount = 1;

            using (var reader = new StreamReader(source.FullName))
            {
                using (var writer = new StreamWriter(targetFilePath))
                {
                    string line;

                    while ((line = reader.ReadLineKeepNewLineChars()) != null)
                    {
                        if (!lineNumbers.Contains(lineCount))
                        {
                            writer.Write(line);
                        }

                        lineCount++;
                    }
                }
            }

            return new FileInfo(targetFilePath);
        }

        /// <summary>
        /// Delete the last line from a text file saving the new content to a new target text file.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="targetFilePath">New target text file path.</param>
        /// <returns>FileInfo for the new target file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targetFilePath" /> cannot be the same as the source path.</exception>
        public static FileInfo DeleteLastLine(this FileInfo source, string targetFilePath)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source.FullName == targetFilePath)
                throw new ArgumentException("Source and target file paths are the same.");

            using (var reader = new StreamReader(source.FullName))
            {
                using (var writer = new StreamWriter(targetFilePath))
                {
                    string line;
                    string previousEndLineChars = null;

                    while ((line = reader.ReadLineKeepNewLineChars()) != null)
                    {
                        if (!string.IsNullOrEmpty(previousEndLineChars))
                        {
                            writer.Write(previousEndLineChars);
                        }

                        if (!IsLastLine(line))
                        {
                            writer.Write(line.RemoveEndLineChars());
                        }

                        previousEndLineChars = line.GetEndLineChars();
                    }
                }
            }

            return new FileInfo(targetFilePath);
        }

        /// <summary>
        /// Replace a specific line from a text file saving the new content to a new target text file.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="lineNumber">Line number to delete.</param>
        /// <param name="newLine">Text for the new line. Any end line characters (e.g. \r\n or \n) will be removed.</param>
        /// <param name="targetFilePath">New target text file path.</param>
        /// <returns>FileInfo for the new target file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="lineNumber" /> cannot be less than one.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targetFilePath" /> cannot be the same as the source path.</exception>
        public static FileInfo ReplaceLine(this FileInfo source, int lineNumber, string newLine, string targetFilePath)
        {
            var newLines = new Dictionary<int, string>(1)
            {
                { lineNumber, newLine }
            };

            return ReplaceLines(source, newLines, targetFilePath);
        }

        /// <summary>
        /// Replace specific lines from a text file saving the new content to a new target text file.
        /// </summary>
        /// <param name="source">File to perform the operation on.</param>
        /// <param name="newLines">Dictionary of line numbers and their new line values.</param>
        /// <param name="targetFilePath">New target text file path.</param>
        /// <returns>FileInfo for the new target file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="newLines" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="newLines" /> cannot contain line numbers be less than one.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="targetFilePath" /> cannot be the same as the source path.</exception>
        public static FileInfo ReplaceLines(this FileInfo source, IDictionary<int, string> newLines, string targetFilePath)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (newLines == null)
                throw new ArgumentNullException(nameof(newLines));

            if (newLines.Keys.Any(n => n < 1))
                throw new ArgumentOutOfRangeException(nameof(newLines), "Line numbers must be one or greater.");

            if (source.FullName == targetFilePath)
                throw new ArgumentException("Source and target file paths are the same.");

            var lineCount = 1;

            using (var streamReader = new StreamReader(source.FullName))
            {
                using (var streamWriter = new StreamWriter(targetFilePath))
                {
                    string line;

                    while ((line = streamReader.ReadLineKeepNewLineChars()) != null)
                    {
                        if (newLines.ContainsKey(lineCount))
                        {
                            var newLine = newLines[lineCount].RemoveEndLineChars();

                            streamWriter.Write(newLine + line.GetEndLineChars());
                        }
                        else
                        {
                            streamWriter.Write(line);
                        }

                        lineCount++;
                    }
                }
            }

            return new FileInfo(targetFilePath);
        }

        private static bool IsLastLine(string line)
        {
            return string.IsNullOrEmpty(line.GetEndLineChars());
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
