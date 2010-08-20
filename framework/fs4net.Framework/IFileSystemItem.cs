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
        // The AddMilliseconds(1) is a compensation for a defect in the .NET framework:
        // It's ok to pass DateTime(1601, 1, 1) as a parameter, but it has no effect.
        internal static DateTime MinimumDate = new DateTime(1601, 1, 1).AddMilliseconds(1).ToLocalTime();

        internal static string Combine(string lhs, string rhs)
        {
            string lhsWithoutEndingBackslash = lhs.EndsWith(@"\") ? lhs.Substring(0, lhs.Length - 1) : lhs;
            string rhsWithoutLeadingBackslash = rhs.StartsWith(@"\") ? rhs.Substring(1) : rhs;
            return lhsWithoutEndingBackslash + @"\" + rhsWithoutLeadingBackslash;
        }
    }
 }