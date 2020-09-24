using System.IO;
using ByteDev.Testing.Builders;

namespace ByteDev.Io.IntTests.FileCommands
{
    public abstract class FileCommandTestBase : IoTestBase
    {
        protected static string FileName1 = "Test1.txt";

        protected string SourceDir;
        protected string DestinationDir;

        protected void SetupSourceDir()
        {
            SourceDir = Path.Combine(WorkingDir, "Source");
            DirectoryBuilder.InFileSystem.WithPath(SourceDir).EmptyIfExists().Build();
        }

        protected void SetupDestinationDir()
        {
            DestinationDir = Path.Combine(WorkingDir, "Destination");
            DirectoryBuilder.InFileSystem.WithPath(DestinationDir).EmptyIfExists().Build();
        }

        protected FileInfo CreateSourceFile(string filePath, long size = 0)
        {
            return FileBuilder.InFileSystem.WithPath(Path.Combine(SourceDir, filePath)).WithSize(size).Build();
        }

        protected FileInfo CreateDestinationFile(string filePath, long size = 0)
        {
            return FileBuilder.InFileSystem.WithPath(Path.Combine(DestinationDir, filePath)).WithSize(size).Build();
        }
    }
}