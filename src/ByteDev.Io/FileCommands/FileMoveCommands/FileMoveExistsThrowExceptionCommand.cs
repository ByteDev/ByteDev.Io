using System.IO;

namespace ByteDev.Io.FileCommands.FileMoveCommands
{
    internal class FileMoveExistsThrowExceptionCommand : FileCommand
    {
        public FileMoveExistsThrowExceptionCommand(string sourceFile, string destinationFile) : base(sourceFile, destinationFile)
        {
        }
        
        public override void Execute()
        {
            FileMove();
            DestinationFileResult = new FileInfo(DestinationFile);
        }
    }
}