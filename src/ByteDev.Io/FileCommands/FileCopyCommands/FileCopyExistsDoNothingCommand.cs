using System.IO;

namespace ByteDev.Io.FileCommands.FileCopyCommands
{
    internal class FileCopyExistsDoNothingCommand : FileCommand
    {
        public FileCopyExistsDoNothingCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileNotExists(DestinationFile))
            {
                FileCopy();
            }

            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}