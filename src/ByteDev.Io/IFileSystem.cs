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
        /// Get the first part of the path that exists moving up through the parent parts.
        /// </summary>
        /// <param name="path">Path to use.</param>
        /// <returns>Path that exists.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="path" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="path" /> is empty.</exception>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException">No part of <paramref name="path" /> exists.</exception>
        string GetPathExists(string path);

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
        /// <exception cref="T:ByteDev.Io.PathNotFoundException"><paramref name="path" /> does not exist.</exception>
        bool IsDirectory(string path);

        /// <summary>
        /// Returns the path of the first file or directory that exists. If no matching path can
        /// be found then an exception will be thrown.
        /// </summary>
        /// <param name="paths">Collection of paths (to files or directories).</param>
        /// <returns>String of first path that exists.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="paths" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="paths" /> is empty.</exception>
        /// <exception cref="T:ByteDev.Io.PathNotFoundException">None of the paths exist.</exception>
        string FirstExists(IEnumerable<string> paths);

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        /// <summary>
        /// Move <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to move.</param>
        /// <param name="destinationFile">Destination to move the file to.</param>
        /// <param name="type">File operation behaviour to use when moving the file.</param>
        /// <returns>File info of the moved file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        
        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        /// <summary>
        /// Copy <paramref name="sourceFile" /> to <paramref name="destinationFile" />.
        /// </summary>
        /// <param name="sourceFile">Source file to copy.</param>
        /// <param name="destinationFile">Destination to copy the file to.</param>
        /// <param name="type">File operation behaviour to use when copying the file.</param>
        /// <returns>File info of the copied file.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sourceFile" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="destinationFile" /> is null.</exception>
        FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        
        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="fileInfo1">First file.</param>
        /// <param name="fileInfo2">Second file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo1" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="fileInfo2" /> is null.</exception>
        void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2);

        /// <summary>
        /// Swaps the file names of two files.
        /// </summary>
        /// <param name="filePath1">File path of first file.</param>
        /// <param name="filePath2">File path of second file.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePath1" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="filePath2" /> is null.</exception>
        void SwapFileNames(string filePath1, string filePath2);
    }
}