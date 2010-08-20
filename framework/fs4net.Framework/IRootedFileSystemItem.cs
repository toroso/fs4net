using System;
using System.IO;
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
        /// Returns the drive that the denoted item is located on.
        /// This property succeeds whether the denoted item exists or not.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">If the file's path is relative and concatenated
        /// with the current directory it exceeds the system-defined maximum length.</exception>
        public static Drive Drive<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return new Drive(me.InternalFileSystem(), CanonicalPathBuilder.GetDriveName(me.PathAsString));
        }

        /// <summary>
        /// Returns the parent directory of the denoted item.
        /// </summary>
        public static RootedDirectory ParentDirectory<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            // TODO: Throw if there is no parent...?
            return new RootedDirectory(me.InternalFileSystem(), Path.GetDirectoryName(me.PathAsString), me.PathWasher);
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

        internal static void VerifyDateTime(DateTime at, string operation, string itemType)
        {
            if (at.IsBefore(PathUtils.MinimumDate))
            {
                throw new ArgumentOutOfRangeException("at", string.Format("Can't {0} to '{1}' since it's not valid for a {2}.", operation, at, itemType));
            }
        }
    }
}