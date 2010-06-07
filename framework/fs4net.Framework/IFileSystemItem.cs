using System;

namespace fs4net.Framework
{
    public interface IFileSystemItem<T> where T: IFileSystemItem<T>
    {
        string PathAsString { get; }
        Func<string, string> PathWasher { get; } // TODO: Move to some kind of internal interface?
        T AsCanonical();
    }

    internal static class PathUtils
    {
        internal static string Combine(string lhs, string rhs)
        {
            string lhsWithoutEndingBackslash = lhs.EndsWith(@"\") ? lhs.Substring(0, lhs.Length - 1) : lhs;
            string rhsWithoutLeadingBackslash = rhs.StartsWith(@"\") ? rhs.Substring(1) : rhs;
            return lhsWithoutEndingBackslash + @"\" + rhsWithoutLeadingBackslash;
        }
    }
 }