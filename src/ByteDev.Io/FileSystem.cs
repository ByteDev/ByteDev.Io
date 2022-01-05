using System;
using System.Collections.Generic;
using System.IO;
using ByteDev.Io.FileCommands;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents file system operations.
    /// </summary>
    public class FileSystem : IFileSystem
    {
        private FileMoveCommandFactory _fileMoveCommandFactory;
        private FileCopyCommandFactory _fileCopyCommandFactory;

        private FileMoveCommandFactory FileMoveCommandFactory => _fileMoveCommandFactory ?? (_fileMoveCommandFactory = new FileMoveCommandFactory());

        private FileCopyCommandFactory FileCopyCommandFactory => _fileCopyCommandFactory ?? (_fileCopyCommandFactory = new FileCopyCommandFactory());

        /// <summary>
        /// Get the first part of the path that exists moving up through the parent parts.
        /// </summary>
        /// <param name="path">Path to use.</param>
        /// <returns>Path that exists.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="path" /> is empty.</exception>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException">No part of <paramref name="path" /> exists.</exception>
        public string GetPathExists(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
                return path;

            var dir = new DirectoryInfo(path);

            while (dir.Parent != null && !dir.Parent.Exists)
            {
                dir = dir.Parent;
            }

            if (dir.Parent != null)
                return dir.Parent.FullName;

            // Drive only now
            if (dir.Exists)
                return dir.FullName;

            throw new PathNotFoundException("No part of the path exists.");
        }

        /// <summary>
        /// Indicates whether <paramref name="path" /> is a file.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if <paramref name="path" /> is a file; otherwise returns false.</returns>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException"><paramref name="path" /> does not exist.</exception>
        public bool IsFile(string path)
        {
            return !IsDirectory(path);
        }

        /// <summary>
        /// Indicates whether <paramref name="path" /> is a directory.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if <paramref name="path" /> is a directory; otherwise returns false.</returns>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException"><paramref name="path" /> does not exist.</exception>
        public bool IsDirectory(string path)
        {
            try
            {
                return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
            }
            catch (FileNotFoundException ex)
            {
                throw new PathNotFoundException($"Unable to find the path '{path}'.", ex);
            }
        }

        /// <summary>
        /// Returns the path of the first file or directory that exists. If no matching path can
        /// be found then an exception will be thrown.
        /// </summary>
        /// <param name="paths">Collection of paths (to files or directories).</param>
        /// <returns>String of first path that exists.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="paths" /> is null.</exception>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException">None of the paths exist.</exception>
        public string FirstExists(IEnumerable<string> paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            foreach (var path in paths)
            {
                if (Directory.Exists(path) || File.Exists(path))
                    return path;
            }

            throw new PathNotFoundException("None of the paths exist.");
        }

        /// <summary>
        /// Returns a collection containing details of which paths exist and which do not.
        /// </summary>
        /// <param name="paths">Collection of paths (to files or directories).</param>
        /// <returns>Collection of ExistsInfo objects containing details of which paths exist and which do not.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="paths" /> is null.</exception>
        public IList<ExistsInfo> Exists(IEnumerable<string> paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            var existsInfos = new List<ExistsInfo>();

            foreach (var path in paths)
            {
                existsInfos.Add(new ExistsInfo
                {
                    Path = path,
                    Exists = Directory.Exists(path) || File.Exists(path)
                });
            }

            return existsInfos;
        }

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        public FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            if(sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (destinationFile == null)
                throw new ArgumentNullException(nameof(destinationFile));

            return MoveFile(sourceFile.FullName, destinationFile.FullName);
        }

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        public FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            if (sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (destinationFile == null)
                throw new ArgumentNullException(nameof(destinationFile));

            var command = FileMoveCommandFactory.Create(type, sourceFile, destinationFile);

            command.Execute();

            return command.DestinationFileResult;
        }

        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        public FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            if(sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (destinationFile == null)
                throw new ArgumentNullException(nameof(destinationFile));

            return CopyFile(sourceFile.FullName, destinationFile.FullName);
        }
        
        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        public FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            if(sourceFile == null)
                throw new ArgumentNullException(nameof(sourceFile));

            if (destinationFile == null)
                throw new ArgumentNullException(nameof(destinationFile));

            var command = FileCopyCommandFactory.Create(type, sourceFile, destinationFile);

            command.Execute();

            return command.DestinationFileResult;
        }

        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="fileInfo1">First file.</param>
        /// <param name="fileInfo2">Second file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo1" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo2" /> is null.</exception>
        public void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2)
        {
            if(fileInfo1 == null)
                throw new ArgumentNullException(nameof(fileInfo1));

            if (fileInfo2 == null)
                throw new ArgumentNullException(nameof(fileInfo2));

            SwapFileNames(fileInfo1.FullName, fileInfo2.FullName);
        }

        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="filePath1">File path of first file.</param>
        /// <param name="filePath2">File path of second file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePath1" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePath2" /> is null.</exception>
        public void SwapFileNames(string filePath1, string filePath2)
        {
            if (filePath1 == null)
                throw new ArgumentNullException(nameof(filePath1));

            if (filePath2 == null)
                throw new ArgumentNullException(nameof(filePath2));

            var filePath1Temp = RenameFileToTemp(filePath1);

            try
            {
                File.Move(filePath2, filePath1);
            }
            catch (FileNotFoundException)
            {
                File.Move(filePath1Temp, filePath1);    // Rename temp file back cos file2 does not exist
            }
            
            File.Move(filePath1Temp, filePath2);
        }

        private static string RenameFileToTemp(string filePath)
        {
            string filePathTemp = filePath + "." + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();

            File.Move(filePath, filePathTemp);

            return filePathTemp;
        }
    }
}
