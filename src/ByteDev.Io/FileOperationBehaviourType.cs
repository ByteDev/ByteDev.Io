namespace ByteDev.Io
{
    public enum FileOperationBehaviourType
    {
        /// <summary>
        /// If destination file already exists then exception is thrown.
        /// </summary>
        DestExistsThrowException = 0,

        /// <summary>
        /// If destination file already exists then do nothing.
        /// </summary>
        DestExistsDoNothing = 1,

        /// <summary>
        /// If destination file already exists then it is overwritten.
        /// </summary>
        DestExistsOverwrite = 2,

        /// <summary>
        /// If destinatoin file already exists then rename to file with number.
        /// For example if "test.txt" existed then the destination file would be come "test (2).txt"
        /// </summary>
        DestExistsRenameWithNumber = 3,

        /// <summary>
        /// If source file's size is greater than destination file then overwrite destination file.
        /// </summary>
        SourceSizeGreaterOverwrite = 4,

        /// <summary>
        /// If source file's date time modified is newer than destination file then overwrite destination file.
        /// </summary>
        SourceIsNewerOverwrite = 5
        
    }
}