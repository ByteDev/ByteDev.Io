using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ByteDev.Io
{
    public static class AssemblyExtensions
    {
        public static string GetManifestResourceName(this Assembly assembly, string fileName)
        {
            string name = assembly.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase));

            if (string.IsNullOrEmpty(name))
            {
                throw new FileNotFoundException($"Embedded file '{fileName}' could not be found in assembly '{assembly.FullName}'.", fileName);
            }

            return name;
        }
    }
}