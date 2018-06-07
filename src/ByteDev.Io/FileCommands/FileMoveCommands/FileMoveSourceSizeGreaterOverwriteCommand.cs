using System;
using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveSourceSizeGreaterOverwriteCommand : FileCommand
    {
        public FileMoveSourceSizeGreaterOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileComparer.IsSourceBigger(SourceFile, DestinationFile))
            {
                FileDelete(DestinationFile);
                FileMove();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
            else
            {
                throw new InvalidOperationException($"Error moving source file. Destination file: '{DestinationFile}' exists and is larger than Source file: '{SourceFile}'.");
            }
        }
    }
}