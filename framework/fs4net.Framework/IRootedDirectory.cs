using System;
using System.IO;

namespace fs4net.Framework
{
    public interface IRootedDirectory<T> : IRootedFileSystemItem<T> where T: IRootedDirectory<T>
    {
    }

    public static class RootedDirectoryExtensions
    {
        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedDirectory Append<T>(this IRootedDirectory<T> lhs, RelativeDirectory rhs) where T : IRootedDirectory<T>
        {
            return new RootedDirectory(lhs.InternalFileSystem(), PathUtils.Combine(lhs.PathAsString, rhs.PathAsString), lhs.PathWasher);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile Append<T>(this IRootedDirectory<T> lhs, RelativeFile rhs) where T : IRootedDirectory<T>
        {
            return new RootedFile(lhs.InternalFileSystem(), PathUtils.Combine(lhs.PathAsString, rhs.PathAsString), lhs.PathWasher);
        }

        /// <summary>
        /// Tests whether the directory exists. Returns true if a directory with the given name exists. If a file with
        /// the given name exists it returns false.
        /// </summary>
        public static bool Exists<T>(this IRootedDirectory<T> me) where T : IRootedDirectory<T>
        {
            return me.IsDirectory();
        }

        /// <summary>
        /// Returns the date and time the directory was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        public static DateTime LastModified<T>(this IRootedDirectory<T> me) where T : IRootedDirectory<T>
        {
            if (me.IsDirectory() == false)
            {
                if (me.IsFile())
                {
                    throw new FileNotFoundException(string.Format("Can't get last modified time for directory '{0}' since it denotes a file.", me.PathAsString));
                }
                throw new FileNotFoundException(string.Format("Can't get last modified time for directory '{0}' since it does not exist.", me.PathAsString));
            }
            return me.InternalFileSystem().GetDirectoryLastModified(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Creates the directory denoted by this descriptor. It creats the leaf folder as well as any non-existin
        /// parent folders. If the directory already exists this method does nothing.
        /// </summary>
        /// TODO: Exceptions!
        public static void Create<T>(this IRootedDirectory<T> me) where T : IRootedDirectory<T>
        {
            if (me.IsFile())
            {
                throw new IOException(string.Format("Can't create the directory '{0}' since it denotes a file.", me.PathAsString));
            }
            if (! me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                fileSystem.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Deletes the directory denoted by this descriptor. If the directory does not exists this method does
        /// nothing.
        /// </summary>
        /// TODO: Revise these exceptions! More specific?
        /// <exception cref="System.IO.IOException">There is an open handle on the directory or on one of its files,
        /// and the operating system is Windows XP or earlier; A file is denoted by this directory descriptor; The
        /// directory is not empty; The directory is the application's current working directory; The directory is
        /// read-only.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it
        /// is on an unmapped drive).</exception>
        public static void Delete<T>(this IRootedDirectory<T> me) where T : IRootedDirectory<T>
        {
            if (me.IsFile())
            {
                throw new IOException(string.Format("Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            }
            if (me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                //if (fileSystem.DirectoryInUse(path))
                //{
                //    // TODO: Better/more specifiec exception?
                //    throw new IOException(string.Format("Can't delete directory '{0}' since it's in use.", me.PathAsString));
                //}
                // Attributes: Archive, ReadOnly, Hidden, System, Device, ...
                //if (fileSystem.GetAttributes(path) == FileAttributes.ReadOnly)
                //{
                //    throw new UnauthorizedAccessException(string.Format("Can't delete read-only directory '{0}'.", me.PathAsString));
                //}
                //if (fileSystem.IsReady(me.DriveName()) == false)
                //{
                //    throw new DirectoryNotFoundException(string.Format("Can't delete the directory '{0}' since the drive is not ready.", me.PathAsString));
                //}
                // TODO: Check so that it's empty
                fileSystem.DeleteDirectory(path);
            }
        }

        /// <summary>
        /// Tries to deletes the directory denoted by this descriptor.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">If the file descriptor is relative and concatenated
        /// with the current directory it exceeds the system-defined maximum length.</exception>
        /// <returns>
        /// True if the file no longer exists. That is, the file was either deleted, or it
        /// did not exist to start with. If the file descriptor denotes a directory this method
        /// returns true.
        /// </returns>
        public static bool TryDelete<T>(this IRootedDirectory<T> me) where T : IRootedDirectory<T>
        {
            if (me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                //if (fileSystem.IsDirectoryInUse(path))
                //{
                //    return false;
                //}
                //if (fileSystem.GetAttributes(path) == FileAttributes.ReadOnly)
                //{
                //    return false;
                //}
                //if (fileSystem.IsReady(me.DriveName()))
                //{
                //    return false;
                //}
                try
                {
                    fileSystem.DeleteDirectory(path);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch { } // To fulfil the nothrow contract...
                // ReSharper restore EmptyGeneralCatchClause
            }
            return me.Exists();
        }
    }
}