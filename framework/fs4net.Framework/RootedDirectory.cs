using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public sealed class RootedDirectory : IRootedDirectory<RootedDirectory>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _rootedPath;
        private readonly Func<string, string> _pathWasher;
        private readonly string _canonicalFullPath;

        public RootedDirectory(IInternalFileSystem fileSystem, string rootedPath, Func<string, string> pathWasher)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            _rootedPath = pathWasher(rootedPath);
            _pathWasher = pathWasher;
            _canonicalFullPath = new CanonicalPathBuilder(_rootedPath).BuildForRootedDirectory();
        }


        #region Public Interface

        /// <summary>
        /// Returns the FileSystem with which this descriptor is associated.
        /// </summary>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Returns the path that this descriptor represent as a string. It is returned on the same format as it was
        /// created with. This means that it can contain redundant parts such as ".", ".." inside paths and multiple
        /// "\". To remove such redundant parts, use the AsCanonical() factory method.
        /// This property succeeds whether the file exists or not.
        /// </summary>
        public string PathAsString
        {
            get { return _rootedPath; }
        }

        public Func<string, string> PathWasher
        {
            get { return _pathWasher; }
        }

        /// <summary>
        /// Returns a descriptor where the PathAsString property returns the path on canonical form. A canonical
        /// descriptor does not contain any redundant names, which means that ".", ".." and extra "\" have been removed
        /// from the string that the descriptor was created with.
        /// This method succeeds whether the file exists or not.
        /// </summary>
        public RootedDirectory AsCanonical() // TODO? Make into extension method and add a Clone() method?
        {
            return new RootedDirectory(_fileSystem, _canonicalFullPath, PathWasher);
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

        public static bool operator ==(RootedDirectory left, RootedDirectory right)
        {
            return Equals(left, right);
        }

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
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't create the directory '{0}' since it denotes a file.", me.PathAsString));
            if (!me.Exists())
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
        public static void DeleteRecursively(this RootedDirectory me)
        {
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't delete the directory '{0}' since it does not exist.", me.PathAsString));

            me.Delete(true);
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor.
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the directory was either deleted, or it
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
        /// Deletes the directory denoted by this descriptor.
        /// </summary>
        /// TODO: Exceptions
        public static void DeleteIfEmpty(this RootedDirectory me)
        {
            me.VerifyIsNotAFile(ThrowHelper.IOException("Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't delete the directory '{0}' since it does not exist.", me.PathAsString));
            me.VerifyIsEmpty(ThrowHelper.IOException("Can't delete the directory '{0}' since it's not empty.", me));

            me.Delete(false);
        }

        /// <summary>
        /// Tries to delete the directory denoted by this descriptor.
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the directory was either deleted, or it
        /// did not exist to start with. If the descriptor denotes a file this method returns true.
        /// </returns>
        public static bool TryDeleteIfEmpty(this RootedDirectory me)
        {
            if (me.Exists())
            {
                if (!me.Empty()) return false;
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
//            try
            {
                fileSystem.DeleteDirectory(path, recursive);
            }
// ReSharper disable EmptyGeneralCatchClause
            // DirectoryNotFoundException, ArgumentException, NotSupportedException, IOException, UnauthorizedAccessException...
//            catch { } // To fulfil the nothrow contract...
// ReSharper restore EmptyGeneralCatchClause
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
            destination.ParentDirectory().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't move the directory since the destination's parent directory '{0}' does not exist.", destination.ParentDirectory()));
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