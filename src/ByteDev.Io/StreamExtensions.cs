using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="source">Stream to read.</param>
        /// <param name="tryStartFromBeginning">Indicates whether to read from the beginning of the stream if possible.</param>
        /// <returns>A UTF8 encoded string from the stream.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static string ReadAsString(this Stream source, bool tryStartFromBeginning = true)
        {
            return ReadAsString(source, Encoding.UTF8, tryStartFromBeginning);
        }

        /// <summary>
        /// Read the stream as a string.
        /// </summary>
        /// <param name="source">Stream to read.</param>
        /// <param name="encoding">Encoding to use when reading from the stream.</param>
        /// <param name="tryStartFromBeginning">Indicates whether to read from the beginning of the stream if possible.</param>
        /// <returns>Encoding string from the stream.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="encoding" /> is null.</exception>
        public static string ReadAsString(this Stream source, Encoding encoding, bool tryStartFromBeginning = true)
        {
            if (encoding == null)
                throw new ArgumentNullException(nameof(encoding));

            using (MemoryStream ms = ReadAsMemoryStream(source, tryStartFromBeginning))
            {
                return encoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Read the stream as a memory stream.
        /// </summary>
        /// <param name="source">Stream to read.</param>
        /// <param name="tryStartFromBeginning">Indicates whether to read from the beginning of the stream if possible.</param>
        /// <returns>A memory stream from the stream.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static MemoryStream ReadAsMemoryStream(this Stream source, bool tryStartFromBeginning = true)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (tryStartFromBeginning && source.CanSeek)
                source.Position = 0;

            var memoryStream = new MemoryStream();
            source.CopyTo(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Determines if the stream is empty.
        /// </summary>
        /// <param name="source">Stream to check.</param>
        /// <returns>True if the stream is empty; otherwise false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static bool IsEmpty(this Stream source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Length == 0;
        }

        /// <summary>
        /// Writes the stream to file.
        /// </summary>
        /// <param name="source">Stream to write to file.</param>
        /// <param name="filePath">File path.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task WriteToFileAsync(this Stream source, string filePath)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using (Stream file = File.Create(filePath))
            {
                source.Seek(0, SeekOrigin.Begin);
                await source.CopyToAsync(file);
            }
        }
    }

    public static class StreamHelper
    {
        public static Stream CreateStream(string data)
        {
            return CreateStream(data, Encoding.UTF8);
        }

        public static Stream CreateStream(string data, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(data);
            
            return new MemoryStream(bytes);
        }
    }
}