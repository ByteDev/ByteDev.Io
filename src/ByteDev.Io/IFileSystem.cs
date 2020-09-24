using System.Collections.Generic;
using System.IO;

namespace ByteDev.Io
{
    /// <summary>
    /// Represents an interface for file system operations.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Indicates whether <paramref name="path" /> is a file.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if <paramref name="path" /> is a file; otherwise returns false.</returns>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException"><paramref name="path" /> does not exist.</exception>
        bool IsFile(string path);

        /// <summary>
        /// Indicates whether <paramref name="path" /> is a directory.
        /// </summary>
        /// <param name="path">Path to check.</param>
        /// <returns>True if <paramref name="path" /> is a directory; otherwise returns false.</returns>
        bool IsDirectory(string path);

        /// <summary>
        /// Returns the path of the first file that exists.
        /// </summary>
        /// <param name="paths">Collection of paths.</param>
        /// <returns>String of first path that exists.</returns>
        string FirstExists(IEnumerable<string> paths);

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        
        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        
        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="fileInfo1">Name of first file.</param>
        /// <param name="fileInfo2">Name of second file.</param>
        void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2);

        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="filePath1">Name of first file.</param>
        /// <param name="filePath2">Name of second file.</param>
        void SwapFileNames(string filePath1, string filePath2);
    }
}