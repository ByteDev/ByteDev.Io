using System;
using System.Runtime.Serialization;

namespace ByteDev.Io
{
    [Serializable]
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : this("Unable to find the specified file or directory.")
        {
        }

        public PathNotFoundException(string message) : base(message)
        {
        }

        public PathNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected PathNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}