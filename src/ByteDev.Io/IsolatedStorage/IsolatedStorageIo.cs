using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;

namespace ByteDev.Io.IsolatedStorage
{
    public class IsolatedStorageIo
    {
        public bool Exists(IsolatedStorageFileName fileName)
        {
            using (var file = GetStorageFile())
            {
                return file.FileExists(fileName.Name);
            }
        }

        public void Delete(IsolatedStorageFileName fileName)
        {
            using (var file = GetStorageFile())
            {
                file.DeleteFile(fileName.Name);
            }
        }

        public void Write(IsolatedStorageFileName fileName, XmlDocument xmlDocument)
        {
            Write(fileName, xmlDocument.OuterXml);
        }

        public void Write(IsolatedStorageFileName fileName, string content)
        {
            IsolatedStorageFile file = GetStorageFile();
            
            using (var stream = new IsolatedStorageFileStream(fileName.Name, FileMode.Create, file))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(content);
                }
            }
        }

        public XmlDocument ReadAsXml(IsolatedStorageFileName fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Read(fileName));
            return xmlDoc;
        }

        public string Read(IsolatedStorageFileName fileName)
        {
            IsolatedStorageFile file = GetStorageFile();

            using (var stream = new IsolatedStorageFileStream(fileName.Name, FileMode.Open, file))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static IsolatedStorageFile GetStorageFile()
        {
            return IsolatedStorageFile.GetUserStoreForApplication();
        }
    }
}