using System;
using System.Globalization;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents a binary file size
    /// </summary>
    public class FileSize
    {
        public enum MultiplierType
        {
            /// <summary>
            /// Multiplier of 1024
            /// </summary>
            BinaryMultiplier = 1024,
            /// <summary>
            /// Multiplier of 1000
            /// </summary>
            DecimalMultiplier = 1000
        }

        private readonly long _totalBytes;
        private readonly MultiplierType _multiplier;
        private string _readableSize;

        public FileSize(long numberOfBytes, MultiplierType multiplier = MultiplierType.BinaryMultiplier)
        {
            if (numberOfBytes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfBytes), "Number of bytes must be zero or more");
            }
            _totalBytes = numberOfBytes;
            _multiplier = multiplier;
        }

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long TotalBytes
        {
            get { return _totalBytes; }
        }

        /// <summary>
        /// File size in Kilobytes
        /// </summary>
        public long TotalKiloBytes
        {
            get { return _totalBytes / (int)_multiplier; }
        }

        /// <summary>
        /// File size in Megabytes
        /// </summary>
        public long TotalMegaBytes
        {
            get { return _totalBytes / (int)_multiplier / (int)_multiplier; }
        }

        /// <summary>
        /// Get readable string of the size.
        /// Example: "1 B", "32 KB", "128 GB" etc.
        /// </summary>
        public string ReadableSize
        {
            get { return _readableSize ?? (_readableSize = GetReadableSize(_totalBytes)); }
        }

        private string GetReadableSize(long size)
        {
            if (size < 1)
            {
                return "0 B";
            }

            string[] suffixes = {"B", "KB", "MB", "GB", "TB", "PB"};
            int place = Convert.ToInt32(Math.Floor(Math.Log(size, (int)_multiplier)));
            double num = Math.Round(size / Math.Pow((int)_multiplier, place), 1);

            return num.ToString(CultureInfo.InvariantCulture) + " " + suffixes[place];
        }
    }
}
