using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Xml;
using System.Xml.Linq;

namespace ByteDev.Io.IsolatedStorage
{
    /// <summary>
    /// Represents wrapper for isolated storage operations.
    /// </summary>
    public class IsolatedStorageIo : IIsolatedStorageIo
    {
        private readonly IsolatedStorageFileType _isolatedStorageFileType;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.IsolatedStorage.IsolatedStorageIo" /> class.
        /// </summary>
        /// <param name="isolatedStorageFileType">A type of isolated storage area that contains files and directories.</param>
        public IsolatedStorageIo(IsolatedStorageFileType isolatedStorageFileType)
        {
            _isolatedStorageFileType = isolatedStorageFileType;
        }

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
            using(IsolatedStorageFile file = GetStorageFile())
            {
                using (var stream = new IsolatedStorageFileStream(fileName.Name, FileMode.Create, file))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        writer.Write(content);
                    }
                }
            }
        }

        /// <summary>
        /// Reads the contents of an isolated storage file as XML and returns as an XmlDocument.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>XmlDocument.</returns>
        public XmlDocument ReadAsXmlDoc(IsolatedStorageFileName fileName)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Read(fileName));
            return xmlDoc;
        }

        /// <summary>
        /// Reads the contents of an isolated storage file as XML and returns as an XDocument.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>XDocument.</returns>
        public XDocument ReadAsXDoc(IsolatedStorageFileName fileName)
        {
            return XDocument.Parse(Read(fileName));
        }

        /// <summary>
        /// Read the contents of a isolated storage file.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>Content from the file.</returns>
        public string Read(IsolatedStorageFileName fileName)
        {
            using (IsolatedStorageFile file = GetStorageFile())
            {
                using (var stream = new IsolatedStorageFileStream(fileName.Name, FileMode.Open, file))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        private IsolatedStorageFile GetStorageFile()
        {
            switch (_isolatedStorageFileType)
            {
                case IsolatedStorageFileType.MachineStoreForApplication:
                    return IsolatedStorageFile.GetMachineStoreForApplication();

                case IsolatedStorageFileType.MachineStoreForAssembly:
                    return IsolatedStorageFile.GetMachineStoreForAssembly();

                case IsolatedStorageFileType.MachineStoreForDomain:
                    return IsolatedStorageFile.GetMachineStoreForDomain();

                case IsolatedStorageFileType.UserStoreForApplication:
                    return IsolatedStorageFile.GetUserStoreForApplication();

                case IsolatedStorageFileType.UserStoreForAssembly:
                    return IsolatedStorageFile.GetUserStoreForAssembly();

                case IsolatedStorageFileType.UserStoreForDomain:
                    return IsolatedStorageFile.GetUserStoreForDomain();

                default:
                    throw new InvalidOperationException("Unhandled isolated storage file type.");
            }
        }
    }
}