using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopyExistsRenameWithNumberCommand : FileCommand
    {
        public FileCopyExistsRenameWithNumberCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileExists(DestinationFile))
            {
                var newDestinationFile = new FileInfo(DestinationFile).GetNextAvailableFileName();
                FileCopy(SourceFile, newDestinationFile.FullName);
                DestinationFileResult = newDestinationFile;
            }
            else
            {
                FileCopy();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
        }
    }
}