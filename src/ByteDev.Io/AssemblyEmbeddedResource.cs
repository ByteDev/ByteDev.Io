using System.IO;
using System.Reflection;

namespace ByteDev.Io
{
    public class AssemblyEmbeddedResource
    {
        public Assembly Assembly { get; }
        public string ResourceName { get; }
        public string FileName { get; }

        private AssemblyEmbeddedResource(Assembly assembly,
            string resourceName,
            string fileName)
        {
            Assembly = assembly;
            ResourceName = resourceName;
            FileName = fileName;
        }

        public static AssemblyEmbeddedResource CreateFromAssemblyContaining<T>(string fileName)
        {
            return CreateFromAssembly(typeof(T).Assembly, fileName);
        }

        public static AssemblyEmbeddedResource CreateFromAssembly(Assembly assembly, string fileName)
        {
            var resourceName = assembly.GetManifestResourceName(fileName);

            return new AssemblyEmbeddedResource(assembly, resourceName, fileName);
        }

        public FileInfo Save(string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                using (var stream = Assembly.GetManifestResourceStream(ResourceName))
                {
                    if (stream == null)
                    {
                        return new FileInfo(filePath);
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }

            return new FileInfo(filePath);
        }
    }
}