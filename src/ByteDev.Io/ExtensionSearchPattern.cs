using System;

namespace ByteDev.Io
{
    internal static class ExtensionSearchPattern
    {
        public static string Create(string searchPattern)
        {
            if (searchPattern == null)
                throw new ArgumentNullException(nameof(searchPattern));

            if (searchPattern.StartsWith("*."))
            {
                return searchPattern;
            }

            if (searchPattern.StartsWith("*") || searchPattern.StartsWith("."))
            {
                return "*." + searchPattern.Substring(1);
            }

            return "*." + searchPattern;
        }
    }
}