using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveExistsOverwriteCommand : FileCommand
    {
        public FileMoveExistsOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            FileDelete(DestinationFile);
            FileMove();

            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}