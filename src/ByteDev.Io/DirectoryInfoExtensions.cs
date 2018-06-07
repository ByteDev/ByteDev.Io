using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ByteDev.Io
{
    public static class DirectoryInfoExtensions
    {
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo source, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException(nameof(extensions));

            var delimitedList = new StringBuilder();

            foreach (var exten in extensions)
            {
                if (delimitedList.ToString() != string.Empty)
                {
                    delimitedList.Append('|');
                }

                delimitedList.Append(exten.StartsWith(".") ? exten.Substring(1) : exten);
            }

            return GetFilesByExtensions(source, delimitedList.ToString());
        }

        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo source, string fileExtensions)
        {
            return source.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                .Where(file => Regex.IsMatch(file.Name, $@"^.+\.({fileExtensions})$", RegexOptions.IgnoreCase));
        }

        public static IEnumerable<FileInfo> GetImageFiles(this DirectoryInfo source)
        {
            const string imageExtenions = @"jpg|jpeg|gif|png|bmp|tif|tiff";

            return GetFilesByExtensions(source, imageExtenions);
        }

        public static IEnumerable<FileInfo> GetVideoFiles(this DirectoryInfo source)
        {
            const string videoContainerExtensions = @"avi|mkv|mp4|m4v|mov|qt|flv|swf|wmv|asf|mpg|mpeg|vob" +
                                                    @"264|3g2|3gp|arf|asx|bik|dash|dat|dvr|h264|m2t|m2ts|mod|mts|ogv|rmvb|tod|tp|ts|webm";

            return GetFilesByExtensions(source, videoContainerExtensions);
        }

        public static IEnumerable<FileInfo> GetAudioFiles(this DirectoryInfo source)
        {
            const string audioContainerExtensions = @"mp3|flac|mpa|wav|m4a|aif|ogg|wma|cda|mid|midi";

            return GetFilesByExtensions(source, audioContainerExtensions);
        }

        /// <summary>
        /// Delete all files and directories
        /// </summary>
        public static void Empty(this DirectoryInfo source)
        {
            DeleteFiles(source);
            DeleteDirectories(source);
        }

        /// <summary>
        /// Delete all directories
        /// </summary>
        public static void DeleteDirectories(this DirectoryInfo source)
        {
            foreach (var dir in source.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Delete all files
        /// </summary>
        public static void DeleteFiles(this DirectoryInfo source)
        {
            foreach (var file in source.GetFiles())
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Delete all files in the directory with particular extension
        /// </summary>
        public static void DeleteFiles(this DirectoryInfo source, string extension)
        {
            foreach (var file in source.GetFiles("*." + extension))
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Recursively create directory
        /// </summary>
        /// <param name="source">Folder path to create.</param>
        public static void CreateDirectory(this DirectoryInfo source)
        {
            if (source.Parent != null) 
                CreateDirectory(source.Parent);

            if (!source.Exists) 
                source.Create();
        }

        /// <summary>
        /// Gets the total size of the directory and its subdirectories
        /// </summary>
        public static long GetSize(this DirectoryInfo source)
        {
            // Go through all files adding their size
            var size = source.GetFiles().Sum(fileInfo => fileInfo.Exists ? fileInfo.Length : 0);

            // Go through all subdirectories adding their size
            size += source.GetDirectories().Sum(dirInfo => dirInfo.Exists ? dirInfo.GetSize() : 0);

            return size;
        }
    }
}