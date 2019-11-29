using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.Reflection.Assembly" />.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Retrieves the name of the file from the assembly manifest.
        /// </summary>
        /// <param name="source">Assembly to perform the operation on.</param>
        /// <param name="fileName">File name to search for.</param>
        /// <returns>Resource name from the assembly manifest.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">Embedded file <paramref name="fileName" /> could not be found in the assembly.</exception>
        public static string GetManifestResourceName(this Assembly source, string fileName)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            string name = source.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(name))
                throw new FileNotFoundException($"Embedded file '{fileName}' could not be found in assembly '{source.FullName}'.", fileName);

            return name;
        }
    }
}