using System.Collections.Generic;
using System.IO;

namespace ByteDev.Io
{
    public interface IFileSystem
    {
        string FirstExists(IEnumerable<string> paths);

        FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2);
        void SwapFileNames(string file1, string file2);

        int DeleteDirectoriesWithName(DirectoryInfo basePath, string directoryName);
        int DeleteDirectoriesWithName(string basePath, string directoryName);
    }
}