using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopyExistsThrowExceptionCommand : FileCommand
    {
        public FileCopyExistsThrowExceptionCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            FileCopy();
            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}