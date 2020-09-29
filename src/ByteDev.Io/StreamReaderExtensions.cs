using System;
using System.IO;
using System.Text;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.StreamReader" />.
    /// </summary>
    public static class StreamReaderExtensions
    {
        private const char EndOfFile = '\uffff';

        /// <summary>
        /// Reads a line, similar to ReadLine method, but keeps any
        /// new line characters (e.g. "\r\n" or "\n").
        /// </summary>
        /// <param name="source">Strean reader to perform the operation on.</param>
        /// <returns>Lines including any end line characters.</returns>
        public static string ReadLineKeepNewLineChars(this StreamReader source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            char ch = (char)source.Read();

            if (ch == EndOfFile)
                return null;

            var sb = new StringBuilder();

            while (ch !=  EndOfFile)
            {
                sb.Append(ch);

                if (ch == '\n')
                    break;

                ch = (char) source.Read();
            }

            return sb.ToString();
        }
    }
}