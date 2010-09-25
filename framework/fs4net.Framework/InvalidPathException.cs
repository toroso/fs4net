using System;
using System.IO;

namespace fs4net.Framework
{
    [Serializable]
    public class InvalidPathException : IOException
    {
        public InvalidPathException() { }

        internal InvalidPathException(string message) : base(message)
        {
        }
    }
}