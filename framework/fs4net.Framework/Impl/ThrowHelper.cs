using System;

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
    }
}