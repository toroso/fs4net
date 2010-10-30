using System;
using System.Collections.Generic;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal static class PathUtils
    {
        // The AddMilliseconds(1) is a compensation for a defect in the .NET framework:
        // It's ok to pass DateTime(1601, 1, 1) as a parameter, but it has no effect.
        internal static DateTime MinimumDate = new DateTime(1601, 1, 1).AddMilliseconds(1).ToLocalTime();

        internal static string Combine(string left, string right)
        {
            string leftWithoutEndingBackslash = left.EndsWith(@"\") ? left.Substring(0, left.Length - 1) : left;
            string rightWithoutLeadingBackslash = right.StartsWith(@"\") ? right.Substring(1) : right;
            return leftWithoutEndingBackslash + @"\" + rightWithoutLeadingBackslash;
        }

        public static string MakeRelativeFrom(string to, string from)
        {
            var toFolders = to.Split(new[] { '\\' });
            var fromFolders = from.RemoveEndingBackslash().Split(new[] { '\\' });
            IEnumerable<string> baseRelativeFrom = fromFolders.SkipFirstEqualTo(toFolders).Select(each => "..");
            IEnumerable<string> toRelativeBase = toFolders.SkipFirstEqualTo(fromFolders).Select(each => each);
            return baseRelativeFrom.Concat(toRelativeBase).MergeToPath();
        }

        internal static string RemoveEndingBackslash(this string me)
        {
            return me.EndsWith(@"\") ? me.Substring(0, me.Length - 1) : me;
        }

        private static IEnumerable<T> SkipFirstEqualTo<T>(this IEnumerable<T> me, IEnumerable<T> other)
        {
            using (IEnumerator<T> meEnum = me.GetEnumerator(), otherEnum = other.GetEnumerator())
            {
                bool meHasMore = false;
                bool done = false;
                while (!done)
                {
                    meHasMore = meEnum.MoveNext();
                    bool otherHasMore = otherEnum.MoveNext();
                    done = !(meHasMore && otherHasMore && meEnum.Current.Equals(otherEnum.Current));
                }
                if (meHasMore)
                {
                    yield return meEnum.Current;
                    while (meEnum.MoveNext())
                    {
                        yield return meEnum.Current;
                    }
                }
            }
        }
    }
}