using System;
using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopySourceSizeGreaterOverwriteCommand : FileCommand
    {
        public FileCopySourceSizeGreaterOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileComparer.IsSourceBigger(SourceFile, DestinationFile))
            {
                FileDelete(DestinationFile);
                FileCopy();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
            else
            {
                throw new InvalidOperationException($"Error moving source file. Destination file: '{DestinationFile}' exists and is larger than Source file: '{SourceFile}'.");
            }
        }
    }
}