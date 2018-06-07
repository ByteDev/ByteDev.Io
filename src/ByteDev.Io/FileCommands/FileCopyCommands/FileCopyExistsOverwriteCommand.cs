using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopyExistsOverwriteCommand : FileCommand
    {
        public FileCopyExistsOverwriteCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            FileDelete(DestinationFile);
            FileCopy();

            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}