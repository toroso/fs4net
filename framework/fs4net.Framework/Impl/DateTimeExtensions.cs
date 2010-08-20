using System;

namespace fs4net.Framework.Impl
{
    internal static class DateTimeExtensions
    {
        public static bool IsBefore(this DateTime me, DateTime at)
        {
            return me < at;
        }
    }
}