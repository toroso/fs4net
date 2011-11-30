using System;
using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a path to a file that starts with a drive (e.g. c:, d:, \\network\drive, etc). It does not
    /// guarantee that the file exists, but rather exposes methods for operating on the path such as creating,
    /// modifying and querying properties.
    /// </summary>
    public sealed class RootedFile : IFile<RootedFile>, IRootedFileSystemItem<RootedFile>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _canonicalFullPath;

        /// <summary>
        /// Initializes a new instance of the class on the specified path.
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
        public RootedFile(IInternalFileSystem fileSystem, string rootedPath, ILogger logger)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            PathAsString = rootedPath;
            Logger = logger;
            _canonicalFullPath = new CanonicalPathBuilder(PathAsString).BuildForRootedFile();
        }

        #region Public Interface

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public string PathAsString { get; private set; }

        public ILogger Logger { get; private set; }

        public RootedFile AsCanonical()
        {
            return new RootedFile(_fileSystem, _canonicalFullPath, Logger);
        }

        #endregion // Public Interface

        #region Value Object

        /// <summary>
        /// Determines whether the specified instance denotes the same path as the current instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public bool Equals(RootedFile other)
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
        /// Determines whether the left instance denotes the same path as the right instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator ==(RootedFile left, RootedFile right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the left instance denotes a different path than the right instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator !=(RootedFile left, RootedFile right)
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

    public static class RootedFileExtensions
    {
        /// <summary>
        /// Tests whether the file exists. Returns true if a file with the given name exists. If a directory with the
        /// given name exists it returns false.
        /// </summary>
        public static bool Exists(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return me.IsFile();
        }

        /// <summary>
        /// Returns the file size in bytes.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The file does not exist; The path denotes an existing
        /// directory; The path is on an unmapped drive.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.UnauthorizedAccessException">Access to the file is denied (according to the MSDN documentation).</exception>
        public static long Size(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get size for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get size for file '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFileSize(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Returns the date and time the file was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist or the path descriptor
        /// denotes and existing directory.</exception>
        public static DateTime LastWriteTime(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last write time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last write time for file '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFileLastWriteTime(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the date and time the file was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist or the path descriptor
        /// denotes and existing directory.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the time value is outside the range of dates or
        /// times permitted for this operation.</exception>
        public static void SetLastWriteTime(this RootedFile me, DateTime at)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set last modified date", "file");
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last write time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last write time for file '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetFileLastWriteTime(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Returns the date and time the file was last accessed.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist or the path descriptor
        /// denotes and existing directory.</exception>
        public static DateTime LastAccessTime(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last access time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't get last access time for file '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFileLastAccessTime(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the LastAccessTime property on the file denoted by this descriptor.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist or the path descriptor
        /// denotes and existing directory.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the time value is outside the range of dates or
        /// times permitted for this operation.</exception>
        public static void SetLastAccessTime(this RootedFile me, DateTime at)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set last accessed date", "file");
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last access time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't set last access time for file '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetFileLastAccessTime(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Deletes the file denoted by this descriptor. If the file does not exists this method does nothing.
        /// </summary>
        /// <exception cref="System.IO.IOException">The file is in use; A directory is denoted by this file
        /// descriptor.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission;
        /// The descriptor denotes a read-only file (according to the MSDN documentation).</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it
        /// is on an unmapped drive).</exception>
        public static void Delete(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.Drive().VerifyExists(ThrowHelper.DirectoryNotFoundException("Can't delete the file '{0}' since the drive it is located on does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException(me.PathAsString, "Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
            if (me.Exists())
            {
                var fileSystem = me.InternalFileSystem();
                var path = me.CanonicalPathAsString();
                //if (fileSystem.IsFileInUse(path))
                //{
                //    // TODO: Better/more specific exception?
                //    throw new IOException(string.Format("Can't delete the file '{0}' since it's in use.", me.PathAsString));
                //}
                //// Attributes: Archive, ReadOnly, Hidden, System, Device, ...
                //if (fileSystem.GetAttributes(path) == FileAttributes.ReadOnly)
                //{
                //    throw new UnauthorizedAccessException(string.Format("Can't delete the read-only file '{0}'.", me.PathAsString));
                //}
                //if (fileSystem.IsReady(me.DriveName()) == false)
                //{
                //    throw new DirectoryNotFoundException(string.Format("Can't delete the file '{0}' since the drive is not ready.", me.PathAsString));
                //}
                fileSystem.DeleteFile(path);
            }
        }

        /// <summary>
        /// Tries to deletes the file denoted by this descriptor.
        /// Since this method has a no-throw contract, there might be occations when it swallows an exception. Such
        /// exception is logged to the file system's ILogger implementation with which this descriptor was created.
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the file was either deleted, or it
        /// did not exist to start with. If the file descriptor denotes a directory this method
        /// returns true.
        /// </returns>
        public static bool TryDelete(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            if (me.Exists())
            {
                try
                {
                    me.Delete();
                }
                catch (Exception ex) // To fulfil the nothrow contract...
                {
                    me.Logger.LogSwallowedException(string.Format("Exception swallowed in File.TryDelete('{0}')", me.PathAsString), ex);
                }
            }
            return !me.Exists();
        }

        /// <summary>
        /// Moves the file to a new location. After the move, the source file will have the name specified by the
        /// destination parameter.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The source file is not found or an existing
        /// directory is denoted by the source descriptor.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The destination directory does not exist.</exception>
        /// <exception cref="System.IO.IOException">An existing file or directory is denoted by the destination path;
        /// Source and destination are the same; The source and destination folders are located on different
        /// drives.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <exception cref="System.InvalidOperationException">The source and destination path descriptors where created
        /// with different IFileSystem implementations.</exception>
        public static void MoveTo(this RootedFile me, RootedFile destination)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(destination, "destination");
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyOnSameDriveAs(destination, ThrowHelper.IOException("Can't move the file '{0}' to '{1}' since they are located on different drives.", me, destination));
            me.VerifyIsNotADirectory(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't move the file '{0}' since it denotes a directory.", me));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't move the file '{0}' since it does not exist.", me));
            destination.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't move the file since the destination's parent directory '{0}' does not exist.", destination.Parent()));
            destination.VerifyIsNotAFile(ThrowHelper.IOException("Can't move the file to the destination '{0}' since a file with that name already exists.", destination));
            destination.VerifyIsNotADirectory(ThrowHelper.IOException("Can't move the file to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.IOException("Can't move the file '{0}' since the source and destination denotes the same file.", me));

            var src = me.CanonicalPathAsString();
            var dst = destination.CanonicalPathAsString();
            var fileSystem = me.InternalFileSystem();
            fileSystem.MoveFile(src, dst);
        }

        /// <summary>
        /// Copies an existing file to a new file. The new file will have the name specified by the
        /// destination parameter. If the destination file already exists this method will fail.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The source file is not found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The destination directory does not exist.</exception>
        /// <exception cref="System.IO.IOException">An existing file or directory is denoted by the destination path;
        /// Source and destination are the same.</exception>
        /// <exception cref="System.UnauthorizedAccessException">An existing directory is denoted by the source
        /// descriptor; The caller does not have the required permission (according to the MSDN
        /// documentation).</exception>
        /// <exception cref="System.InvalidOperationException">The source and destination path descriptors where created
        /// with different IFileSystem implementations.</exception>
        public static void CopyTo(this RootedFile me, RootedFile destination)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(destination, "destination");
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException(me.PathAsString, "Can't copy the file '{0}' since it denotes a directory.", me));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't copy the file '{0}' since it does not exist.", me));
            destination.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't copy the file since the destination's parent directory '{0}' does not exist.", destination.Parent()));
            destination.VerifyIsNotAFile(ThrowHelper.IOException("Can't copy the file to the destination '{0}' since a file with that name already exists.", destination));
            destination.VerifyIsNotADirectory(ThrowHelper.IOException("Can't copy the file to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.IOException("Can't copy the file '{0}' since the source and destination denotes the same file.", me));

            var src = me.CanonicalPathAsString();
            var dst = destination.CanonicalPathAsString();
            var fileSystem = me.InternalFileSystem();
            fileSystem.CopyFile(src, dst);
        }

        /// <summary>
        /// Copies an existing file to a new file. The new file will have the name specified by the
        /// destination parameter. If the destination file already exists it will be overwritten.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The source file is not found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The destination directory does not exist.</exception>
        /// <exception cref="System.IO.IOException">An existing directory is denoted by the destination path;
        /// Source and destination are the same.</exception>
        /// <exception cref="System.UnauthorizedAccessException">An existing directory is denoted by the source
        /// descriptor; The caller does not have the required permission (according to the MSDN
        /// documentation).</exception>
        /// <exception cref="System.InvalidOperationException">The source and destination path descriptors where created
        /// with different IFileSystem implementations.</exception>
        public static void CopyToAndOverwrite(this RootedFile me, RootedFile destination)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(destination, "destination");
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException(me.PathAsString, "Can't copy the file '{0}' since it denotes a directory.", me));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException(me.PathAsString, "Can't copy the file '{0}' since it does not exist.", me));
            destination.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't copy the file since the destination's parent directory '{0}' does not exist.", destination.Parent()));
            destination.VerifyIsNotADirectory(ThrowHelper.IOException("Can't copy the file to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.IOException("Can't copy the file '{0}' since the source and destination denotes the same file.", me));

            var src = me.CanonicalPathAsString();
            var dst = destination.CanonicalPathAsString();
            var fileSystem = me.InternalFileSystem();
            fileSystem.CopyAndOverwriteFile(src, dst);
        }

        /// <summary>
        /// Opens a read stream with the file denoted by this file descriptor as source.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <returns>A FileStream object opened for reading.</returns>
        public static Stream CreateReadStream(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't open the file '{0}' for reading since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException("Can't open a file '{0}' for reading since the path denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.FileNotFoundException("Can't open the file '{0}' for reading since it does not exists.", me.PathAsString));

            return me.InternalFileSystem().CreateReadStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. If the file already exists
        /// the file is overwritten.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <returns>A FileStream object opened for writing.</returns>
        public static Stream CreateWriteStream(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't create the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException("Can't create the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateWriteStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. The file is opened in
        /// write mode and the stream is position at the end of the file. If the file does not exist it is created.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <returns>A FileStream object opened for writing.</returns>
        public static Stream CreateAppendStream(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't append to the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException("Can't append to the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateAppendStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. The file is opened in
        /// read/write mode and the stream is positioned at the beginning of the file. If the file does not exist it is
        /// created.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        /// <returns>A FileStream object opened for reading and writing.</returns>
        public static Stream CreateModifyStream(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            me.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't modify the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.UnauthorizedAccessException("Can't modify the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateModifyStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Returns a RootedFile where the filename is replaced with the given filename. That is, it points to a file
        /// in the same directory.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The resulting path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        public static RootedFile WithFileName(this RootedFile me, FileName newName)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(newName, "newName");
            return me.Parent() + newName;
        }

        /// <summary>
        /// Returns this file name on a form relative to the given directory.
        /// <example>(c:\path\to\file.txt, c:\path\in) => ..\to\file.txt</example>
        /// </summary>
        /// <exception cref="System.ArgumentException">The descriptors are on different drives.</exception>
        public static RelativeFile RelativeFrom(this RootedFile me, RootedDirectory other)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(other, "other");
            me.VerifyOnSameDriveAs(other, ThrowHelper.ArgumentException("Can't find a relative path since '{0}' and '{1}' have different drives.", me.PathAsString, other.PathAsString));

            return RelativeFile.FromString(PathUtils.MakeRelativeFrom(me.CanonicalPathAsString().FullPath, other.CanonicalPathAsString().FullPath));
        }
    }
}