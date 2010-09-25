using System;
using System.IO;
using System.Runtime.Serialization;

namespace fs4net.Framework
{
    [Serializable]
    public sealed class InvalidPathException : IOException
    {
        public InvalidPathException() { }

        public InvalidPathException(String message, Exception innerException) : base(message, innerException)
        {
        }

        private InvalidPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidPathException(string message) : base(message)
        {
        }
    }
}