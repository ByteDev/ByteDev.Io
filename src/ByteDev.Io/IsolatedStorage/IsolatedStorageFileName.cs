using System;

namespace ByteDev.Io.IsolatedStorage
{
    /// <summary>
    /// Represents the name of a file in isolated storage.
    /// </summary>
    public class IsolatedStorageFileName
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.IsolatedStorage.IsolatedStorageFileName" /> class.
        /// </summary>
        /// <param name="appName">The name of the application using isolated storage.</param>
        /// <param name="version">The version of the application using isolated storage.</param>
        public IsolatedStorageFileName(string appName, Version version) : this(appName, version, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.IsolatedStorage.IsolatedStorageFileName" /> class.
        /// </summary>
        /// <param name="appName">The name of the application using isolated storage.</param>
        /// <param name="version">The version of the application using isolated storage.</param>
        /// <param name="fileExtension">The file extension of the file in isolated storage.</param>
        public IsolatedStorageFileName(string appName, Version version, string fileExtension)
        {
            AppName = appName;
            Version = version;
            FileExtension = fileExtension;
        }

        /// <summary>
        /// The name of the application using isolated storage.
        /// </summary>
        public string AppName { get; }

        /// <summary>
        /// The version of the application using isolated storage.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// The file extension of the file in isolated storage.
        /// </summary>
        public string FileExtension { get; }
        
        /// <summary>
        /// The full name of the file in isolated storage.
        /// </summary>
        public string Name => $"{AppName}_{Version.Major}.{Version.Minor}{GetExtension(FileExtension)}";

        /// <summary>
        /// Returns the full name of the file in isolated storage.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

        private static string GetExtension(string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
                return string.Empty;

            if (fileType.Substring(0, 1) != ".")
                fileType = "." + fileType;

            return fileType;
        }
    }
}