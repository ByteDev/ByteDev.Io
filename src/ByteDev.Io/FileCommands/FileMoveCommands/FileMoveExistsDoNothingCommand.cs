using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveExistsDoNothingCommand : FileCommand
    {
        public FileMoveExistsDoNothingCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }

        public override void Execute()
        {
            if (FileNotExists(DestinationFile))
            {
                FileMove();
            }

            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}