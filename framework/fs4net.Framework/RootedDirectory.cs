using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a path to a directory that starts with a drive. It does not guarantee that the directory exists, but
    /// rather exposes methods for operating on the path such as creating, modifying and enumerating directory
    /// content.
    /// </summary>
    public sealed class RootedDirectory : IRootedDirectory<RootedDirectory>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _canonicalFullPath;

        /// <summary>
        /// Initializes a new instance of the class on the specified path. The path may not end with a backslash.
        /// </summary>
        /// <param name="fileSystem">The FileSystem with which this descriptor is associated.</param>
        /// <param name="rootedPath">A string specifying the path that the class should encapsulate.</param>
        /// <param name="logger">A logger where to this descriptor reports any abnormalities.</param>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.NonRootedPathException">The specified path is relative or empty.</exception>
        public RootedDirectory(IInternalFileSystem fileSystem, string rootedPath, ILogger logger)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            PathAsString = rootedPath;
            Logger = logger;
            _canonicalFullPath = new CanonicalPathBuilder(PathAsString).BuildForRootedDirectory();
        }


        #region Public Interface

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public string PathAsString { get; private set; }

        public ILogger Logger { get; private set; }

        public RootedDirectory AsCanonical() // TODO? Make into extension method and add a Clone() method?
        {
            return new RootedDirectory(_fileSystem, _canonicalFullPath, Logger);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedDirectory operator +(RootedDirectory left, RelativeDirectory right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(RootedDirectory left, RelativeFile right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(RootedDirectory left, FileName right)
        {
            return left.Append(right);
        }

        #endregion // Public Interface

        #region Value Object

        /// <summary>
        /// Determines whether the specified RootedDirectory denotes the same path as the current RootedDirectory. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public bool Equals<T>(IRootedDirectory<T> other) where T : IRootedDirectory<T>
        {
            return this.DenotesSamePathAs(other);
        }

        public override bool Equals(object obj)
        {
            return this.DenotesSamePathAs(obj);
        }

        public override int GetHashCode()
        {
            return this.InternalGetHashCode();
        }

        /// <summary>
        /// Determines whether the left RootedDirectory denotes the same path as the right RootedDirectory. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator ==(RootedDirectory left, RootedDirectory right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the left RootedDirectory denotes a different path than the right RootedDirectory. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator !=(RootedDirectory left, RootedDirectory right)
        {
            return !Equals(left, right);
        }

        #endregion // Value Object

        #region Debugging

        public override string ToString()
        {
            return PathAsString;
        }

        #endregion Debugging
    }

    public static class RootedDirectoryExtensions
    {
        /// <summary>
        /// Creates the directory denoted by this descriptor. It creates the leaf folder as well as any non-existing
        /// parent folders. If the directory already exists this method does nothing.
        /// </summary>
        /// TODO: Exceptions!
        public static void Create(this RootedDirectory me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't create the directory '{0}' since it denotes a file.", me.PathAsString));
            if (!me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                fileSystem.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Deletes the directory denoted by this descriptor including all files and directories contained in the
        /// directory. If the directory does not exists this method does nothing.
        /// </summary>
        /// TODO: Revise these exceptions! More specific?
        /// <exception cref="System.IO.IOException">There is an open handle on the directory or on one of its files,
        /// and the operating system is Windows XP or earlier; A file is denoted by this directory descriptor; The
        /// directory is not empty; The directory is the application's current working directory; The directory is
        /// read-only.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it
        /// is on an unmapped drive).</exception>
        public static void DeleteRecursively(this RootedDirectory me)
        {
            me.Drive().VerifyExists(ThrowHelper.DirectoryNotFoundException("Can't delete the directory '{0}' since the drive it is located on does not exist.", me.PathAsString));
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            if (me.Exists())
            {
                me.Delete(true);
            }
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor including all files and directories contained in
        /// the directory. If the directory does not exists this method does nothing.
        /// </summary>
        /// <returns>
        /// True if the directory no longer exists. That is, the directory was either deleted, or it
        /// did not exist to start with. If the descriptor denotes a file this method returns true.
        /// </returns>
        public static bool TryDeleteRecursively(this RootedDirectory me)
        {
            if (me.Exists())
            {
                me.TryDelete(true);
            }
            return !me.Exists();
        }

        /// <summary>
        /// Deletes the directory denoted by this descriptor. If the directory does not exists this method does
        /// nothing.
        /// </summary>
        /// TODO: Exceptions
        public static void DeleteIfEmpty(this RootedDirectory me)
        {
            me.Drive().VerifyExists(ThrowHelper.DirectoryNotFoundException("Can't delete the directory '{0}' since the drive it is located on does not exist.", me.PathAsString));
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsEmpty(ThrowHelper.IOException("Can't delete the directory '{0}' since it's not empty.", me));

            if (me.Exists())
            {
                me.Delete(false);
            }
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor. If the directory does not exists this method does
        /// nothing.
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the directory was either deleted, or it
        /// did not exist to start with. If the descriptor denotes a file this method returns true.
        /// </returns>
        public static bool TryDeleteIfEmpty(this RootedDirectory me)
        {
            if (me.Exists())
            {
                if (!me.IsEmpty()) return false;
                me.TryDelete(false);
            }
            return !me.Exists();
        }

        private static void Delete(this RootedDirectory me, bool recursive)
        {
            me.InternalFileSystem().DeleteDirectory(me.CanonicalPathAsString(), recursive);
        }

        private static void TryDelete(this RootedDirectory me, bool recursive)
        {
            var fileSystem = me.InternalFileSystem();
            var path = me.CanonicalPathAsString();
            try
            {
                fileSystem.DeleteDirectory(path, recursive);
            }
            //DirectoryNotFoundException, ArgumentException, NotSupportedException, IOException, UnauthorizedAccessException...
            catch (Exception ex) // To fulfil the nothrow contract...
            {
                me.Logger.LogSwallowedException(string.Format("Exception swallowed in Directory.TryDelete('{0}', recursive:{1})", me.PathAsString, recursive), ex);
            }
        }

        /// <summary>
        /// Moves the directory and its contents to a new location. After the move, the source directory will have the
        /// name specified by the destination parameter.
        /// </summary>
        /// TODO: Exceptions
        public static void MoveTo(this RootedDirectory me, RootedDirectory destination)
        {
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyOnSameDriveAs(destination, ThrowHelper.IOException("Can't move the directory '{0}' to '{1}' since they are located on different drives.", me, destination));
            me.VerifyIsNotAFile(ThrowHelper.DirectoryNotFoundException("Can't move the directory '{0}' since it denotes a file.", me));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't move the directory '{0}' since it does not exist.", me));
            destination.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't move the directory since the destination's parent directory '{0}' does not exist.", destination.Parent()));
            destination.VerifyIsNotAFile(ThrowHelper.IOException("Can't move the directory to the destination '{0}' since a file with that name already exists.", destination));
            destination.VerifyIsNotADirectory(ThrowHelper.IOException("Can't move the directory to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.IOException("Can't move the directory '{0}' since the source and destination denotes the same directory.", destination));
            me.VerifyIsNotAParentOf(destination, ThrowHelper.IOException("Can't move the directory to the destination '{0}' since it is located inside the source directory.", destination));

            var src = me.CanonicalPathAsString();
            var dst = destination.CanonicalPathAsString();
            var fileSystem = me.InternalFileSystem();
            fileSystem.MoveDirectory(src, dst);
        }
    }
}