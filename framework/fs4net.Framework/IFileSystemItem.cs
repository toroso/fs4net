using System;

namespace fs4net.Framework
{
    public interface IFileSystemItem<T> where T: IFileSystemItem<T>
    {
        string PathAsString { get; }
        Func<string, string> PathWasher { get; } // TODO: Move to some kind of internal interface?
        T AsCanonical();
    }

    public static class FileSystemItemExtensions
    {
        internal static string AsLowerCaseCanonicalString<T>(this IFileSystemItem<T> me)
            where T : IFileSystemItem<T>
        {
            return me.AsCanonical().PathAsString.ToLower();
        }
    }
}