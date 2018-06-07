using System.Threading.Tasks;

namespace ByteDev.Io
{
    public interface IAssemblyFileReader
    {
        Task<string> ReadFileAsync(string fileName);
    }
}