namespace ByteDev.Io
{
    /// <summary>
    /// Represents information on whether a particular file or directory exists.
    /// </summary>
    public class ExistsInfo
    {
        /// <summary>
        /// Path of the file or directory.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// Indicates if the file or directory exists.
        /// </summary>
        public bool Exists { get; internal set;  }
    }
}