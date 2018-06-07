using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ByteDev.Io
{
    public class AssemblyFileReader : IAssemblyFileReader
    {
        private readonly Assembly _assembly;

        public AssemblyFileReader(Assembly assembly)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public async Task<string> ReadFileAsync(string fileName)
        {
            var resourceName = GetManifestResourceName(fileName, _assembly);

            using (var stream = _assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        private static string GetManifestResourceName(string fileName, Assembly assembly)
        {
            string name = assembly.GetManifestResourceNames().SingleOrDefault(n => n.EndsWith(fileName));
            
            if (string.IsNullOrEmpty(name))
            {
                throw new FileNotFoundException($"Embedded file '{fileName}' could not be found in assembly '{assembly.FullName}'.", fileName);
            }

            return name;
        }
    }
}