using System;
using System.Runtime.Serialization;

namespace ByteDev.Io
{
    /// <summary>
    /// The exception that is thrown when a file or directory is not found. 
    /// </summary>
    [Serializable]
    public class PathNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.PathNotFoundException" /> class.
        /// </summary>
        public PathNotFoundException() : this("Unable to find the specified file or directory.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.PathNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PathNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.PathNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>       
        public PathNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Io.PathNotFoundException" /> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected PathNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}