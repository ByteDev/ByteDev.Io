using System;
using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopySourceIsNewerOverwriteCommand : FileCommand
    {
        public FileCopySourceIsNewerOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileComparer.IsSourceModifiedMoreRecently(SourceFile, DestinationFile))
            {
                FileDelete(DestinationFile);
                FileCopy();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
            else
            {
                throw new InvalidOperationException($"Error moving source file. Destination file: '{DestinationFile}' exists and is newer than Source file: '{SourceFile}'");
            }
        }
    }
}