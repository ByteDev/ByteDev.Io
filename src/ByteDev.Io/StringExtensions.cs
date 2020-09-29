namespace ByteDev.Io
{
    internal static class StringExtensions
    {
        public static string GetEndLineChars(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (source.EndsWith("\r\n"))        // Windows
                return "\r\n";

            if (source.EndsWith("\n"))          // Unix
                return "\n";

            return string.Empty;
        }
    }
}