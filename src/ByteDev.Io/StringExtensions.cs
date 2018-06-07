using System;

namespace ByteDev.Io
{
    public static class StringExtensions
    {
        /// <summary>
        /// Appends a back slash to the path if one doesnt exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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