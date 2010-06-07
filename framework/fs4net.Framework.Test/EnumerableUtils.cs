using System;
using System.Collections.Generic;

namespace fs4net.Framework.Test
{
    public static class EnumerableUtils
    {
        public static void ForEach<T>(this IEnumerable<T> me, Action<T> action)
        {
            foreach (T each in me)
            {
                action(each);
            }
        }
    }
}