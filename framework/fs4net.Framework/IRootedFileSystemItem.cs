using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public interface IRootedFileSystemItem<T> : IFileSystemItem<T> where T : IRootedFileSystemItem<T>
    {
        IFileSystem FileSystem { get; }
    }

    public static class RootedFileSystemItemExtensions
    {
        /// <summary>
        /// Returns the drive that the denoted item is located on. If the path is relative it returns the drive of the
        /// current directory.
        /// This property succeeds whether the file exists or not.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">If the file's path is relative and concatenated
        /// with the current directory it exceeds the system-defined maximum length.</exception>
        public static Drive Drive<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return new Drive(me.InternalFileSystem(), CanonicalPathBuilder.GetDriveName(me.PathAsString));
        }

        internal static bool IsFile<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return me.InternalFileSystem().IsFile(me.CanonicalPathAsString());
        }

        internal static bool IsDirectory<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return me.InternalFileSystem().IsDirectory(me.CanonicalPathAsString());
        }

        internal static RootedCanonicalPath CanonicalPathAsString<T>(this IFileSystemItem<T> me) where T : IFileSystemItem<T>
        {
            return new RootedCanonicalPath(me.AsCanonical().PathAsString);
        }

        internal static IInternalFileSystem InternalFileSystem<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return ((IInternalFileSystem)me.FileSystem);
        }
    }
}