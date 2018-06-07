using System.IO;
using System.Text;

namespace ByteDev.Io
{
    public static class StreamExtensions
    {
        public static string ReadAsString(this Stream source, bool tryStartFromBeginning = true)
        {
            using (var ms = ReadAsStream(source, tryStartFromBeginning))
            {
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public static MemoryStream ReadAsStream(this Stream source, bool tryStartFromBeginning = true)
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