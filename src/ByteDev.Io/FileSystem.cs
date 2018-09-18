using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ByteDev.Io.FileCommands;

namespace ByteDev.Io
{
    public class FileSystem : IFileSystem
    {
        private FileMoveCommandFactory _fileMoveCommandFactory;
        private FileCopyCommandFactory _fileCopyCommandFactory;

        private FileMoveCommandFactory FileMoveCommandFactory
        {
            get { return _fileMoveCommandFactory ?? (_fileMoveCommandFactory = new FileMoveCommandFactory()); }
        }

        private FileCopyCommandFactory FileCopyCommandFactory
        {
            get { return _fileCopyCommandFactory ?? (_fileCopyCommandFactory = new FileCopyCommandFactory()); }
        }

        public string FirstExists(IEnumerable<string> paths)
        {
            if (paths == null)
                throw new ArgumentNullException(nameof(paths));

            if (!paths.Any())
                throw new ArgumentException("Empty path list provided.", nameof(paths));

            foreach (var path in paths)
            {
                if (Directory.Exists(path) || File.Exists(path))
                    return path;
            }

            throw new PathNotFoundException("None of the paths exist.");
        }

        #region File Operations

        public IEnumerable<string> GetFiles(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath);
        }

        public FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            return MoveFile(sourceFile.FullName, destinationFile.FullName);
        }

        public FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            var command = FileMoveCommandFactory.Create(type, sourceFile, destinationFile);

            command.Execute();

            return command.DestinationFileResult;
        }

        public FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            return CopyFile(sourceFile.FullName, destinationFile.FullName);
        }

        public FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException)
        {
            var command = FileCopyCommandFactory.Create(type, sourceFile, destinationFile);

            command.Execute();

            return command.DestinationFileResult;
        }

        public void SwapFileNames(string file1, string file2)
        {
            if (file1 == null)
                throw new ArgumentNullException(nameof(file1));

            if (file2 == null)
                throw new ArgumentNullException(nameof(file2));

            string file1Temp = file1 + "." + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLower();

            File.Move(file1, file1Temp);

            try
            {
                File.Move(file2, file1);
            }
            catch (FileNotFoundException)
            {
                File.Move(file1Temp, file1);
            }
            
            File.Move(file1Temp, file2);
        }

        public void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2)
        {
            if(fileInfo1 == null)
                throw new ArgumentNullException(nameof(fileInfo1));

            if (fileInfo2 == null)
                throw new ArgumentNullException(nameof(fileInfo2));

            SwapFileNames(fileInfo1.FullName, fileInfo2.FullName);
        }

        #endregion

        #region Directory operations

        public void DeleteDirectory(string path)
        {
            try
            {
                EmptyDirectory(path);
                Directory.Delete(path);
            }
            catch (DirectoryNotFoundException)
            {
            }
        }

        public void EmptyDirectory(string path)
        {
            new DirectoryInfo(path).Empty();
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        #endregion
    }
}
