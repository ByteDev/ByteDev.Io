using System.IO;
using System.Text;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents a factory for creating streams.
    /// </summary>
    public static class StreamFactory
    {
        /// <summary>
        /// Create a memory stream containing UTF8 encoded string data.
        /// </summary>
        /// <param name="data">UTF8 encoded string contents.</param>
        /// <returns>Stream containing the data.</returns>
        public static Stream Create(string data)
        {
            return Create(data, Encoding.UTF8);
        }

        /// <summary>
        /// Creates a memory stream containing string data.
        /// </summary>
        /// <param name="data">String contents.</param>
        /// <param name="encoding">Encoding of <paramref name="data" />.</param>
        /// <returns>Stream containing the data.</returns>
        public static Stream Create(string data, Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(data);

            return Create(bytes);
        }

        /// <summary>
        /// Creates a memory stream containing the byte array of data.
        /// </summary>
        /// <param name="data">Byte array contents</param>
        /// <returns>Stream containing the data.</returns>
        public static Stream Create(byte[] data)
        {
            return new MemoryStream(data);
        }
    }
}