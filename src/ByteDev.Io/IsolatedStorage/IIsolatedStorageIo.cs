using System.Xml;
using System.Xml.Linq;

namespace ByteDev.Io.IsolatedStorage
{
    public interface IIsolatedStorageIo
    {
        /// <summary>
        /// Indicates whether the isolated storage file exists.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>True if the file exists; otherwise returns false.</returns>
        bool Exists(IsolatedStorageFileName fileName);

        /// <summary>
        /// Deletes a file from isolated storage. If the file does not exist
        /// then no exception will be thrown.
        /// </summary>
        /// <param name="fileName">File name of file to delete.</param>
        void Delete(IsolatedStorageFileName fileName);

        /// <summary>
        /// Writes XML document to a isolated storage file. If file already exists then it 
        /// will be overwritten.
        /// </summary>
        /// <param name="fileName">File name of file to write to.</param>
        /// <param name="xmlDocument">XML document to write to isolated storage.</param>
        void Write(IsolatedStorageFileName fileName, XmlDocument xmlDocument);

        /// <summary>
        /// Writes string to a isolated storage file. If file already exists then it 
        /// will be overwritten.
        /// </summary>
        /// <param name="fileName">File name of file to write to.</param>
        /// <param name="content">String to write to the file.</param>
        void Write(IsolatedStorageFileName fileName, string content);

        /// <summary>
        /// Reads the contents of an isolated storage file as XML and returns as an XmlDocument.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>XmlDocument.</returns>
        XmlDocument ReadAsXmlDoc(IsolatedStorageFileName fileName);

        /// <summary>
        /// Reads the contents of an isolated storage file as XML and returns as an XDocument.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>XDocument.</returns>
        XDocument ReadAsXDoc(IsolatedStorageFileName fileName);

        /// <summary>
        /// Read the contents of a isolated storage file.
        /// </summary>
        /// <param name="fileName">File name of file to read from.</param>
        /// <returns>Content from the file.</returns>
        string Read(IsolatedStorageFileName fileName);
    }
}