using System.Collections.Generic;
using System.IO;

namespace ByteDev.Io
{
    public interface IFileSystem
    {
        FileSize GetFileSize(FileInfo fileInfo);
        FileSize GetFileSize(string filePath);

        FileInfo MoveFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        FileInfo MoveFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        FileInfo CopyFile(FileInfo sourceFile, FileInfo destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);
        FileInfo CopyFile(string sourceFile, string destinationFile, FileOperationBehaviourType type = FileOperationBehaviourType.DestExistsThrowException);

        void SwapFileNames(FileInfo fileInfo1, FileInfo fileInfo2);
        void SwapFileNames(string file1, string file2);

        void DeleteDirectory(string path);
        void EmptyDirectory(string path);
        IEnumerable<string> GetDirectories(string path);
    }
}