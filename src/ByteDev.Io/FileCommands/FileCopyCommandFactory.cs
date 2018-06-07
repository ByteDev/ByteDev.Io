using System;
using ByteDev.Io.FileCommands.FileCopyCommands;

namespace ByteDev.Io.FileCommands
{
    internal class FileCopyCommandFactory
    {
        public FileCommand Create(FileOperationBehaviourType type, string sourceFile, string destinationFile)
        {
            switch (type)
            {
                case FileOperationBehaviourType.DestExistsThrowException:
                    return new FileCopyExistsThrowExceptionCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsDoNothing:
                    return new FileCopyExistsDoNothingCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsOverwrite:
                    return new FileCopyExistsOverwriteCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.DestExistsRenameWithNumber:
                    return new FileCopyExistsRenameWithNumberCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.SourceSizeGreaterOverwrite:
                    return new FileCopySourceSizeGreaterOverwriteCommand(sourceFile, destinationFile);

                case FileOperationBehaviourType.SourceIsNewerOverwrite:
                    return new FileCopySourceIsNewerOverwriteCommand(sourceFile, destinationFile);

                default:
                    throw new InvalidOperationException($"Unhandled {nameof(FileOperationBehaviourType)}: {type}.");
            }
        }
    }
}