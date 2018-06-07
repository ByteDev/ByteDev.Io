using System;
using System.IO;

namespace ByteDev.Io.FileCommands
{
    /// <summary>
    /// Represents a file move or file copy command.
    /// </summary>
    internal abstract class FileCommand
    {
        protected string SourceFile { get; }
        protected string DestinationFile { get; }

        protected FileCommand(string sourceFile, string destinationFile)
        {
            if(string.IsNullOrEmpty(sourceFile))
                throw new ArgumentException("Source file was null or empty.", nameof(sourceFile));

            if (string.IsNullOrEmpty(destinationFile))
                throw new ArgumentException("Destination file was null or empty.", nameof(destinationFile));
            
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

        public FileInfo DestinationFileResult { get; protected set; }
    
        public abstract void Execute();

        protected void FileMove()
        {
            FileMove(SourceFile, DestinationFile);
        }

        protected void FileMove(string sourceFile, string destinationFile)
        {
            File.Move(sourceFile, destinationFile);
        }

        protected void FileCopy()
        {
            FileCopy(SourceFile, DestinationFile);
        }

        protected void FileCopy(string sourceFile, string destinationFile)
        {
            File.Copy(sourceFile, destinationFile);
        }

        protected bool FileExists(string file)
        {
            return File.Exists(file);
        }

        protected bool FileNotExists(string file)
        {
            return !File.Exists(file);
        }

        protected void FileDelete(string file)
        {
            File.Delete(file);
        }
    }
}