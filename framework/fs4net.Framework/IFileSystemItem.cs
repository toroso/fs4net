using System;

namespace fs4net.Framework
{
    public interface IFileSystemItem<T> where T: IFileSystemItem<T>
    {
        string PathAsString { get; }
        Func<string, string> PathWasher { get; } // TODO: Move to some kind of internal interface?
        T AsCanonical();
    }
}