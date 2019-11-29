using System;
using System.Globalization;
using System.IO;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents a binary file size.
    /// </summary>
    public class FileSize
    {
        /// <summary>
        /// Size multiplier type.
        /// </summary>
        public enum MultiplierType
        {
            /// <summary>
            /// Multiplier of 1024.
            /// </summary>
            BinaryMultiplier = 1024,
            /// <summary>
            /// Multiplier of 1000.
            /// </summary>
            DecimalMultiplier = 1000
        }

        private readonly MultiplierType _multiplier;
        private string _readableSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.FileSize" /> class.
        /// </summary>
        /// <param name="filePath">File path.</param>
        /// <param name="multiplier">Size multiplier type.</param>
        public FileSize(string filePath, MultiplierType multiplier = MultiplierType.BinaryMultiplier) : this(new FileInfo(filePath), multiplier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.FileSize" /> class.
        /// </summary>
        /// <param name="fileInfo">File info.</param>
        /// <param name="multiplier">Size multiplier type.</param>
        public FileSize(FileInfo fileInfo, MultiplierType multiplier = MultiplierType.BinaryMultiplier) : this(fileInfo.Length, multiplier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.FileSize" /> class.
        /// </summary>
        /// <param name="numberOfBytes">File size in bytes.</param>
        /// <param name="multiplier">Size multiplier type.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="numberOfBytes" /> must be zero or more.</exception>
        public FileSize(long numberOfBytes, MultiplierType multiplier = MultiplierType.BinaryMultiplier)
        {
            if (numberOfBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBytes), "Number of bytes must be zero or more");
            
            TotalBytes = numberOfBytes;
            _multiplier = multiplier;
        }

        /// <summary>
        /// File size in bytes.
        /// </summary>
        public long TotalBytes { get; }

        /// <summary>
        /// File size in Kilobytes.
        /// </summary>
        public long TotalKiloBytes => TotalBytes / (int)_multiplier;

        /// <summary>
        /// File size in Megabytes.
        /// </summary>
        public long TotalMegaBytes => TotalBytes / (int)_multiplier / (int)_multiplier;

        /// <summary>
        /// Get readable string of the size. For example "1 B", "32 KB", "128 GB" etc.
        /// </summary>
        public string ReadableSize => _readableSize ?? (_readableSize = GetReadableSize(TotalBytes));

        private string GetReadableSize(long size)
        {
            if (size < 1)
                return "0 B";

            string[] suffixes = {"B", "KB", "MB", "GB", "TB", "PB"};

            int place = Convert.ToInt32(Math.Floor(Math.Log(size, (int)_multiplier)));
            double num = Math.Round(size / Math.Pow((int)_multiplier, place), 1);

            return num.ToString(CultureInfo.InvariantCulture) + " " + suffixes[place];
        }
    }
}
