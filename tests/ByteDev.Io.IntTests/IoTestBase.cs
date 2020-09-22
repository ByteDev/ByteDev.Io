using System.IO;
using System.Threading;

namespace ByteDev.Io.IntTests
{
    public abstract class IoTestBase : IoBase
    {
        protected IoTestBase() : base(@"C:\Temp\ByteDev.Common.IntTests.Io\")
        {
        }

        protected void AppendCharToFile(string filePath)
        {
            using (var streamWriter = File.AppendText(filePath))
            {
                streamWriter.WriteLine("a");
            }
        }

        protected void PauseHalfSecond()
        {
            Thread.Sleep(500);
        }
    }
}