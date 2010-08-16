using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public static RootedDirectory Append<T>(this IRootedDirectory<T> lhs, RelativeDirectory rhs)
            where T : IRootedDirectory<T>
        {
            return new RootedDirectory(lhs.InternalFileSystem(), PathUtils.Combine(lhs.PathAsString, rhs.PathAsString), lhs.PathWasher);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile Append<TDir, TFile>(this IRootedDirectory<TDir> lhs, IRelativeFile<TFile> rhs)
            where TDir : IRootedDirectory<TDir>
            where TFile : IRelativeFile<TFile>
        {
            return new RootedFile(lhs.InternalFileSystem(), PathUtils.Combine(lhs.PathAsString, rhs.PathAsString), lhs.PathWasher);
        }

        /// <summary>
        /// Returns a relative descriptor containing the name of the leaf folder of this path.
        /// Example: LeafFolder("c:\my\path\to") => "to".
        /// If this path has no leaf folder, e.g. it denotes a drive, this method returns an empty descriptor.
        /// </summary>
        // TODO: Exceptions, Allow empty RelativeDirectories
        public static RelativeDirectory LeafFolder<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            var canonicalPath = me.CanonicalPathAsString().FullPath;
            var lastBackslashIndex = canonicalPath.LastIndexOf('\\');
//            if (lastBackslashIndex == -1) return RelativeDirectory.Empty;
            return RelativeDirectory.FromString(canonicalPath.Substring(lastBackslashIndex + 1));
        }

        /// <summary>
        /// Tests whether the directory exists. Returns true if a directory with the given name exists. If a file with
        /// the given name exists it returns false.
        /// </summary>
        public static bool Exists<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            return me.IsDirectory();
        }

        /// <summary>
        /// Returns the date and time the directory was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        public static DateTime LastModified<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyDenotesExistingDirectory("get last modified time");
            return me.InternalFileSystem().GetDirectoryLastModified(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Returns the date and time the directory was last accessed.
        /// </summary>
        /// TODO: Exceptions!
        public static DateTime LastAccessed<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyDenotesExistingDirectory("get last accessed time");
            return me.InternalFileSystem().GetDirectoryLastAccessed(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Creates the directory denoted by this descriptor. It creats the leaf folder as well as any non-existin
        /// parent folders. If the directory already exists this method does nothing.
        /// </summary>
        /// TODO: Exceptions!
        public static void Create<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
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
        public static void DeleteRecursively<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
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
                fileSystem.DeleteDirectory(path, true);
            }
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">If the file descriptor is relative and concatenated
        /// with the current directory it exceeds the system-defined maximum length.</exception>
        /// <returns>
        /// True if the file no longer exists. That is, the file was either deleted, or it
        /// did not exist to start with. If the file descriptor denotes a directory this method
        /// returns true.
        /// </returns>
        /// TODO: Exceptions
        public static bool TryDeleteRecursively<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
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
                TryDeleteDirectory(fileSystem, path, true);
            }
            return !me.Exists();
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor.
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the file was either deleted, or it
        /// did not exist to start with. If the file descriptor denotes a directory this method
        /// returns true.
        /// </returns>
        /// TODO: Exceptions
        public static bool TryDeleteIfEmpty<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            if (me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                TryDeleteDirectory(fileSystem, path, false);
            }
            return !me.Exists();
        }

        private static void TryDeleteDirectory(IInternalFileSystem fileSystem, RootedCanonicalPath path, bool recursive)
        {
            try
            {
                fileSystem.DeleteDirectory(path, recursive);
            }
// ReSharper disable EmptyGeneralCatchClause
            // DirectoryNotFoundException, ArgumentException, NotSupportedException, IOException, UnauthorizedAccessException...
            catch { } // To fulfil the nothrow contract...
// ReSharper restore EmptyGeneralCatchClause
        }

        /// <summary>
        /// Returns all files that exist in this directory.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedFile> Files<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            return me.Files(file => true);
        }

        /// <summary>
        /// Returns all files that exist in this directory and that match the given predicate.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedFile> Files<T>(this IRootedDirectory<T> me, Func<RootedFile, bool> predicate)
            where T : IRootedDirectory<T>
        {
            VerifyDenotesExistingDirectory(me, "get all files");

            return me.InternalFileSystem().GetFilesInDirectory(me.CanonicalPathAsString()).Where(predicate);
        }

        /// <summary>
        /// Returns all directories that exist in this directory.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedDirectory> Directories<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            return me.Directories(directory => true);
        }

        /// <summary>
        /// Returns all directories that exist in this directory and that match the given predicate.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedDirectory> Directories<T>(this IRootedDirectory<T> me, Func<RootedDirectory, bool> predicate)
            where T : IRootedDirectory<T>
        {
            VerifyDenotesExistingDirectory(me, "get all directories");

            return me.InternalFileSystem().GetDirectoriesInDirectory(me.CanonicalPathAsString()).Where(predicate);
        }

        private static void VerifyDenotesExistingDirectory<T>(this IRootedDirectory<T> me, string operation)
            where T : IRootedDirectory<T>
        {
            if (me.IsDirectory() == false)
            {
                if (me.IsFile())
                {
                    ThrowFileNotFound(me, operation, "it denotes a file");
                }
                ThrowFileNotFound(me, operation, "it does not exist");
            }
        }

        private static void ThrowFileNotFound<T>(IRootedDirectory<T> forFile, string operation, string reason)
            where T : IRootedDirectory<T>
        {
            string msg = string.Format("Can't {0} for directory '{1}' since {2}.", operation, forFile.PathAsString, reason);
            throw new FileNotFoundException(msg, forFile.PathAsString);
        }
    }
}