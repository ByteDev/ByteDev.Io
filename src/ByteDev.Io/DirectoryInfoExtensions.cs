using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ByteDev.Io
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IO.DirectoryInfo" />.
    /// </summary>
    public static class DirectoryInfoExtensions
    {
        /// <summary>
        /// Retrieves all files with the file extensions.
        /// </summary>
        /// <param name="source">The directory to get files from.</param>
        /// <param name="extensions">File extensions.</param>
        /// <returns>Collection of files.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo source, params string[] extensions)
        {
            if (extensions == null)
                throw new ArgumentNullException(nameof(extensions));

            var delimitedStr = new StringBuilder();

            foreach (var exten in extensions)
            {
                if (delimitedStr.ToString() != string.Empty)
                    delimitedStr.Append('|');

                delimitedStr.Append(exten.StartsWith(".") ? exten.Substring(1) : exten);
            }

            return GetFilesByExtensions(source, delimitedStr.ToString());
        }

        /// <summary>
        /// Retrieves all files with the file extensions.
        /// </summary>
        /// <param name="source">The directory to get files from.</param>
        /// <param name="fileExtensions">File extensions delimited by pipe character.</param>
        /// <returns>Collection of files.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IEnumerable<FileInfo> GetFilesByExtensions(this DirectoryInfo source, string fileExtensions)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));

            return source.GetFiles("*.*", SearchOption.TopDirectoryOnly)
                .Where(file => Regex.IsMatch(file.Name, $@"^.+\.({fileExtensions})$", RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Retrieves all image files.
        /// </summary>
        /// <param name="source">The directory to get all image files from.</param>
        /// <returns>Collection of image files.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IEnumerable<FileInfo> GetImageFiles(this DirectoryInfo source)
        {
            const string imageExtenions = @"jpg|jpeg|gif|png|bmp|tif|tiff";

            return GetFilesByExtensions(source, imageExtenions);
        }

        /// <summary>
        /// Retrieves all video files.
        /// </summary>
        /// <param name="source">The directory to get all video files from.</param>
        /// <returns>Collection of video files.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IEnumerable<FileInfo> GetVideoFiles(this DirectoryInfo source)
        {
            const string videoContainerExtensions = @"avi|mkv|mp4|m4v|mov|qt|flv|swf|wmv|asf|mpg|mpeg|vob" +
                                                    @"264|3g2|3gp|arf|asx|bik|dash|dat|dvr|h264|m2t|m2ts|mod|mts|ogv|rmvb|tod|tp|ts|webm";

            return GetFilesByExtensions(source, videoContainerExtensions);
        }

        /// <summary>
        /// Retrieves all audio files.
        /// </summary>
        /// <param name="source">The directory to get all audio files from.</param>
        /// <returns>Collection of audio files.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        public static IEnumerable<FileInfo> GetAudioFiles(this DirectoryInfo source)
        {
            const string audioContainerExtensions = @"mp3|flac|mpa|wav|m4a|aif|ogg|wma|cda|mid|midi";

            return GetFilesByExtensions(source, audioContainerExtensions);
        }

        /// <summary>
        /// Delete all files and directories.
        /// </summary>
        /// <param name="source">The directory to empty.</param>
        public static void Empty(this DirectoryInfo source)
        {
            DeleteFiles(source);
            DeleteDirectories(source);
        }

        /// <summary>
        /// Delete all directories.
        /// </summary>
        /// <param name="source">The directory to delete all directories from.</param>
        public static void DeleteDirectories(this DirectoryInfo source)
        {
            foreach (var dir in source.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Delete all files.
        /// </summary>
        /// <param name="source">The directory to delete all files from.</param>
        public static void DeleteFiles(this DirectoryInfo source)
        {
            foreach (var file in source.GetFiles())
            {
                file.Delete();
            }
        }
        
        /// <summary>
        /// Delete all files in the directory with particular extension.
        /// </summary>
        /// <param name="source">The directory to delete all files with the extension from.</param>
        /// <param name="extension">File extension without period prefix.</param>
        public static void DeleteFiles(this DirectoryInfo source, string extension)
        {
            foreach (var file in source.GetFiles("*." + extension))
            {
                file.Delete();
            }
        }

        /// <summary>
        /// Recursively create directory.
        /// </summary>
        /// <param name="source">Directory path to create.</param>
        public static void CreateDirectory(this DirectoryInfo source)
        {
            if (source.Parent != null) 
                CreateDirectory(source.Parent);

            if (!source.Exists) 
                source.Create();
        }
        
        /// <summary>
        /// Retrieves the total size of the directory and its subdirectories.
        /// </summary>
        /// <param name="source">Directory to retrieve the size on.</param>
        /// <returns>Size of <paramref name="source" /> in bytes.</returns>
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