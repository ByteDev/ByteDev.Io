using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveExistsRenameWithNumberCommand : FileCommand
    {
        public FileMoveExistsRenameWithNumberCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileExists(DestinationFile))
            {
                var newDestinationFile = new FileInfo(DestinationFile).GetNextAvailableFileName();
                FileMove(SourceFile, newDestinationFile.FullName);
                DestinationFileResult = newDestinationFile;
            }
            else
            {
                FileMove();
                DestinationFileResult = new FileInfo(DestinationFile);
            }
        }
    }
}