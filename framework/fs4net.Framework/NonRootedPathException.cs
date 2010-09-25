using System;
using System.IO;

namespace fs4net.Framework
{
    // TODO: Should this really be an IOException? The IOExceptions are a bit f*cked up... they can be anything from normal usage exceptions to programming errors.
    [Serializable]
    public sealed class NonRootedPathException : IOException
    {
        public NonRootedPathException() { }

        public NonRootedPathException(string message) : base(message)
        {
        }
    }
}