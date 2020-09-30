namespace ByteDev.Io
{
    internal static class StringExtensions
    {
        private const string WindowsEndLine = "\r\n";
        private const string UnixEndLine = "\n";

        public static string GetEndLineChars(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (source.EndsWith(WindowsEndLine))
                return WindowsEndLine;

            if (source.EndsWith(UnixEndLine))
                return UnixEndLine;

            return string.Empty;
        }

        public static string RemoveEndLineChars(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (source.EndsWith(WindowsEndLine))
                return source.Substring(0, source.Length - WindowsEndLine.Length);

            if (source.EndsWith(UnixEndLine))
                return source.Substring(0, source.Length - UnixEndLine.Length);

            return source;
        }

        public static string AddExtensionDotPrefix(this string source)
        {
            if (!string.IsNullOrEmpty(source) && !source.StartsWith("."))
                return "." + source;

            return source;
        }
    }
}