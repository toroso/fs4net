using System;
using System.IO;
using System.Runtime.Serialization;

namespace fs4net.Framework
{
    // TODO: Should this really be an IOException? The IOExceptions are a bit f*cked up... they can be anything from normal usage exceptions to programming errors.
    [Serializable]
    public sealed class RootedPathException : IOException
    {
        public RootedPathException() { }

        public RootedPathException(String message, Exception innerException) : base(message, innerException)
        {
        }

        private RootedPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RootedPathException(string message) : base(message)
        {
        }
    }
}