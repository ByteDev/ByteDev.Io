using ByteDev.Strings;

namespace ByteDev.Io
{
    internal static class StringExtensions
    {
        public static string AddFileExtensionDotPrefix(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            return source.EnsureStartsWith(".");
        }
    }
}