using System;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.String" />.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Appends a back slash to the path if one doesn't exist.
        /// </summary>
        /// <param name="path">Path to append back slash to.</param>
        /// <returns><paramref name="path" /> with appended back slash.</returns>
        public static string AppendBackSlash(this string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path is null or empty.", nameof(path));

            if (path.Substring(path.Length - 1, 1) != @"\")
            {
                path += @"\";
            }

            return path;
        }
    }
}