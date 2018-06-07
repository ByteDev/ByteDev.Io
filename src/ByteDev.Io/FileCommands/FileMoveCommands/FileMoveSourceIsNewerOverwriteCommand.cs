using System;
using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveSourceIsNewerOverwriteCommand : FileCommand
    {
        public FileMoveSourceIsNewerOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileComparer.IsSourceModifiedMoreRecently(SourceFile, DestinationFile))
            {
                FileDelete(DestinationFile);
                FileMove();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
            else
            {
                throw new InvalidOperationException($"Error moving source file. Destination file: '{DestinationFile}' exists and is newer than Source file: '{SourceFile}'");
            }
        }
    }
}