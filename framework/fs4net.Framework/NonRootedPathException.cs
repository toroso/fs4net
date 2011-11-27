using System;
using System.IO;
using System.Runtime.Serialization;

namespace fs4net.Framework
{
    /// <summary>
    /// The exception that is thrown when an attempt is made to create a rooted path descriptor from a non-rooted path.
    /// </summary>
    // TODO: Should this really be an IOException? The IOExceptions are a bit f*cked up... they can be anything from normal usage exceptions to programming errors.
    [Serializable]
    public sealed class NonRootedPathException : IOException
    {
        public NonRootedPathException() { }

        public NonRootedPathException(String message, Exception innerException) : base(message, innerException)
        {
        }

        private NonRootedPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NonRootedPathException(string message) : base(message)
        {
        }
    }
}