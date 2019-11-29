using System.IO;
using System.Text;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.Stream" />.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Read the stream as a UTF8 encoded string.
        /// </summary>
        /// <param name="source">The stream to read.</param>
        /// <param name="tryStartFromBeginning">Indicates whether to read from the beginning of the stream if possible.</param>
        /// <returns>A UTF8 encoded string from the stream.</returns>
        public static string ReadAsString(this Stream source, bool tryStartFromBeginning = true)
        {
            using (var ms = ReadAsMemoryStream(source, tryStartFromBeginning))
            {
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Read the stream as a memory stream.
        /// </summary>
        /// <param name="source">The stream to read.</param>
        /// <param name="tryStartFromBeginning">Indicates whether to read from the beginning of the stream if possible.</param>
        /// <returns>A memory stream from the stream.</returns>
        public static MemoryStream ReadAsMemoryStream(this Stream source, bool tryStartFromBeginning = true)
        {
            if (tryStartFromBeginning && source.CanSeek)
                source.Position = 0;

            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}