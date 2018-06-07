using System;
using ByteDev.Io.FileCommands.FileMoveCommands;

namespace ByteDev.Io.FileCommands
{
    internal class FileMoveCommandFactory
    {
        public FileCommand Create(FileOperationBehaviourType type, string sourceFile, string destinationFile)
        {
            switch (type)
            {
                case FileOperationBehaviourType.DestExistsThrowException:
                    return new FileMoveExistsThrowExceptionCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsDoNothing:
                    return new FileMoveExistsDoNothingCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsOverwrite:
                    return new FileMoveExistsOverwriteCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsRenameWithNumber:
                    return new FileMoveExistsRenameWithNumberCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.SourceSizeGreaterOverwrite:
                    return new FileMoveSourceSizeGreaterOverwriteCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.SourceIsNewerOverwrite:
                    return new FileMoveSourceIsNewerOverwriteCommand(sourceFile, destinationFile);

                default:
                    throw new InvalidOperationException($"Unhandled {nameof(FileOperationBehaviourType)}: {type}.");
            }
        }
    }
}