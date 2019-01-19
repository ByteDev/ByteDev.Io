using System;

namespace ByteDev.Io.IsolatedStorage
{
    public class IsolatedStorageFileName
    {
        public IsolatedStorageFileName(string appName, Version version) : this(appName, version, string.Empty)
        {
        }

        public IsolatedStorageFileName(string appName, Version version, string fileType)
        {
            AppName = appName;
            Version = version;
            FileType = fileType;
        }

        public Version Version { get; }

        public string FileType { get; }

        public string AppName { get; }

        public string Name => $"{AppName}_{Version.Major}.{Version.Minor}{GetExtension(FileType)}";

        public override string ToString()
        {
            return Name;
        }

        private static string GetExtension(string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
                return string.Empty;

            if (fileType.Substring(0, 1) != ".")
            {
                fileType = "." + fileType;
            }
            return fileType;
        }
    }
}