using System;
using System.IO;
using System.Runtime.Serialization;

namespace fs4net.Framework
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to create a path descriptor with a path that is invalid in
    /// some way.
    /// </summary>
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