using System;
using System.IO;

namespace fs4net.Framework.Impl
{
    internal static class ThrowHelper
    {
        internal static void ThrowIfNull<T>(T parameter, string parameterName) where T: class
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        internal static Func<Exception> IOException(string template, params object[] args)
        {
            return () => new IOException(string.Format(template, args));
        }

        internal static Func<Exception> DirectoryNotFoundException(string template, params object[] args)
        {
            return () => new DirectoryNotFoundException(string.Format(template, args));
        }

        public static Func<Exception> FileNotFoundException(string fileName, string template, params object[] args)
        {
            return () => new FileNotFoundException(string.Format(template, args), fileName);
        }

        public static Func<Exception> ArgumentException(string template, params object[] args)
        {
            return () => new ArgumentException(string.Format(template, args));
        }

        public static Func<Exception> UnauthorizedAccessException(string template, params object[] args)
        {
            return () => new UnauthorizedAccessException(string.Format(template, args));
        }

        public static Func<Exception> InvalidPathException(string template, params object[] args)
        {
            return () => new InvalidPathException(string.Format(template, args));
        }
    }
}