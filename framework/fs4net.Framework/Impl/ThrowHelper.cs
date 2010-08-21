using System;
using System.IO;

namespace fs4net.Framework.Impl
{
    internal class ThrowHelper
    {
        internal static void ThrowIfNull<T>(T parameter, string parameterName) where T: class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        internal static void ThrowIfEmpty(string parameter, string errorMessage)
        {
            if (parameter == string.Empty)
            {
                throw new ArgumentException(errorMessage);
            }
        }

        internal static Func<Exception> CreateIOException(string template, params object[] args)
        {
            return () => new IOException(string.Format(template, args));
        }

        internal static Func<Exception> CreateDirectoryNotFoundException(string template, params object[] args)
        {
            return () => new DirectoryNotFoundException(string.Format(template, args));
        }

        public static Func<Exception> CreateFileNotFoundException(string fileName, string template, params object[] args)
        {
            return () => new FileNotFoundException(string.Format(template, args), fileName);
        }

        public static Func<Exception> CreateArgumentException(string template, params object[] args)
        {
            return () => new ArgumentException(string.Format(template, args));
        }
    }
}