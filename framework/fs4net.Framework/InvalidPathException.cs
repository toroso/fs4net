using System.IO;

namespace fs4net.Framework
{
    public class InvalidPathException : IOException
    {
        internal InvalidPathException(string message) : base(message)
        {
        }
    }
}