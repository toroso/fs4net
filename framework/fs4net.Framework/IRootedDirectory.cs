using fs4net.Framework.Impl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fs4net.Framework
{
    public interface IRootedDirectory<T> : IRootedFileSystemItem<T>, IDirectory<T> where T: IRootedDirectory<T>
    {
    }

    public static class RootedDirectoryInterfaceExtensions
    {
        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedDirectory Append<T>(this IRootedDirectory<T> left, RelativeDirectory right)
            where T : IRootedDirectory<T>
        {
            return new RootedDirectory(left.InternalFileSystem(), PathUtils.Combine(left.PathAsString, right.PathAsString), left.Logger);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile Append<TDir, TFile>(this IRootedDirectory<TDir> left, IRelativeFile<TFile> right)
            where TDir : IRootedDirectory<TDir>
            where TFile : IRelativeFile<TFile>
        {
            return new RootedFile(left.InternalFileSystem(), PathUtils.Combine(left.PathAsString, right.PathAsString), left.Logger);
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
            ThrowHelper.ThrowIfNull(me, "me");
            var asCanonical = me.AsCanonical();
            if (asCanonical.Equals(asCanonical.Drive())) return RelativeDirectory.FromString(string.Empty);

            var canonicalPath = asCanonical.PathAsString;
            var lastBackslashIndex = canonicalPath.LastIndexOf('\\');
            return RelativeDirectory.FromString(canonicalPath.Substring(lastBackslashIndex + 1));
        }

        public static RelativeDirectory RelativeFrom<T, TOther>(this IRootedDirectory<T> me, IRootedDirectory<TOther> other)
            where T : IRootedDirectory<T>
            where TOther : IRootedDirectory<TOther>
        {
            me.VerifyOnSameDriveAs(other, ThrowHelper.ArgumentException("Can't find a relative path since '{0}' and '{1}' have different drives.", me.PathAsString, other.PathAsString));

            var relative = PathUtils.MakeRelativeFrom(me.CanonicalPathAsString().FullPath, other.CanonicalPathAsString().FullPath);
            return RelativeDirectory.FromString(other.PathAsString.EndsWith(@"\") ? relative + @"\" : relative);
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
        public static DateTime LastWriteTime<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyIsNotAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last write time for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last write time for directory '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetDirectoryLastWriteTime(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the date and time the directory was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the time value is outside the range of dates or
        /// times permitted for this operation.</exception>
        public static void SetLastWriteTime<T>(this IRootedDirectory<T> me, DateTime at)
            where T : IRootedDirectory<T>
        {
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set modified date", "directory");
            me.VerifyIsNotAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last write time for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last write time for directory '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetDirectoryLastWriteTime(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Returns the date and time the directory was last accessed.
        /// </summary>
        /// TODO: Exceptions!
        public static DateTime LastAccessTime<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyIsNotAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last access time for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last access time for directory '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetDirectoryLastAccessTime(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the date and time the directory was last accessed.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the time value is outside the range of dates or
        /// times permitted for this operation.</exception>
        public static void SetLastAccessTime<T>(this IRootedDirectory<T> me, DateTime at)
            where T : IRootedDirectory<T>
        {
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set last accessed date", "directory");
            me.VerifyIsNotAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last access time for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last access time for directory '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetDirectoryLastAccessTime(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Returns true if there are no files or folders in the given directory.
        /// </summary>
        public static bool IsEmpty<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            return !(me.Files().Any() || me.Directories().Any());
        }

        /// <summary>
        /// Returns all files that exist in this directory.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedFile> Files<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyIsNotAFile(ThrowHelper.IOException(me.PathAsString, "Can't get all files for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException(me.PathAsString, "Can't get all files for directory '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFilesInDirectory(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Returns all directories that exist in this directory.
        /// </summary>
        /// TODO: Exceptions
        public static IEnumerable<RootedDirectory> Directories<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyIsNotAFile(ThrowHelper.IOException(me.PathAsString, "Can't get all directories for directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException(me.PathAsString, "Can't get all directories for directory '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetDirectoriesInDirectory(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the application's current working directory to the given directory.
        /// </summary>
        /// TODO: Exceptions
        public static void SetAsCurrent<T>(this IRootedDirectory<T> me)
            where T : IRootedDirectory<T>
        {
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException(me.PathAsString, "Can't set directory '{0}' as current since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetAsCurrent(me.CanonicalPathAsString());
        }
    }

    internal static class RootedDirectoryInterfaceVerifications
    {
        internal static void VerifyIsEmpty<T>(this IRootedDirectory<T> me, Func<Exception> createException)
            where T : IRootedDirectory<T>
        {
            if (!me.IsEmpty())
            {
                throw createException();
            }
        }
    }
}