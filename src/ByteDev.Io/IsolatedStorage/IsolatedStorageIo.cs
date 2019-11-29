using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;

namespace ByteDev.Io.IsolatedStorage
{
    /// <summary>
    /// Represents wrapper for isolated storage operations.
    /// </summary>
    public class IsolatedStorageIo
    {
        /// <summary>
        /// Indicates whether the isolated storage file exists.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>True if the file exists; otherwise returns false.</returns>
        public bool Exists(IsolatedStorageFileName fileName)
        {
            using (var file = GetStorageFile())
            {
                return file.FileExists(fileName.Name);
            }
        }

        /// <summary>
        /// Deletes a file from isolated storage.
        /// </summary>
        /// <param name="fileName">File name of file to delete.</param>
        public void Delete(IsolatedStorageFileName fileName)
        {
            using (var file = GetStorageFile())
            {
                file.DeleteFile(fileName.Name);
            }
        }

        /// <summary>
        /// Writes XML to a isolated storage file.
        /// </summary>
        /// <param name="fileName">File name of file to write to.</param>
        /// <param name="xmlDocument">XML document to write to isolated storage.</param>
        public void Write(IsolatedStorageFileName fileName, XmlDocument xmlDocument)
        {
            Write(fileName, xmlDocument.OuterXml);
        }

        /// <summary>
        /// Writes string to a isolated storage file.
        /// </summary>
        /// <param name="fileName">File name of file to write to.</param>
        /// <param name="content">String to write to the file.</param>
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

        /// <summary>
        /// Read the contents of a isolated storage file as XML.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>XML from the file.</returns>
        public XmlDocument ReadAsXml(IsolatedStorageFileName fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Read(fileName));
            return xmlDoc;
        }

        /// <summary>
        /// Read the contents of a isolated storage file.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>Content from the file.</returns>
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