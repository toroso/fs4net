using System;

namespace fs4net.Framework.Impl
{
    internal static class StringExtensions
    {
        public static bool IsEmpty(this String me)
        {
            return me.Length == 0;
        }
    }
}