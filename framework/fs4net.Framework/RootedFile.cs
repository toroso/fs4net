using System;
using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a the full path to a file. The path is rooted, meaning that it starts with a drive (e.g. c:, d:,
    /// \\network\drive, etc).
    /// </summary>
    public sealed class RootedFile : IFile<RootedFile>, IRootedFileSystemItem<RootedFile>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _rootedPath;
        private readonly Func<string, string> _pathWasher;
        private readonly string _canonicalFullPath;

        public RootedFile(IInternalFileSystem fileSystem, string rootedPath, Func<string, string> pathWasher)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            _rootedPath = pathWasher(rootedPath);
            _pathWasher = pathWasher;
            _canonicalFullPath = new CanonicalPathBuilder(_rootedPath).BuildForRootedFile();
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
        public RootedFile AsCanonical() // TODO? Make into extension method and add a Clone() method?
        {
            return new RootedFile(_fileSystem, _canonicalFullPath, PathWasher);
        }

        #endregion // Public Interface

        #region Value Object

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

        public static bool operator ==(RootedFile left, RootedFile right)
        {
            return Equals(left, right);
        }

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
            return me.IsFile();
        }

        /// <summary>
        /// Returns the date and time the file was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        public static DateTime LastModified(this RootedFile me)
        {
            me.VerifyIsNotADirectory(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't get last modified time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't get last modified time for file '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFileLastModified(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the date and time the file was last written to.
        /// </summary>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">If the time value is outside the range of dates or
        /// times permitted for this operation.</exception>
        public static void SetLastModified(this RootedFile me, DateTime at)
        {
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set last modified date", "file");
            me.VerifyIsNotADirectory(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't set last modified time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't set last modified time for file '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetFileLastModified(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Returns the date and time the file was last accessed.
        /// </summary>
        /// TODO: Exceptions!
        public static DateTime LastAccessed(this RootedFile me)
        {
            me.VerifyIsNotADirectory(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't get last accessed time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't get last accessed time for file '{0}' since it does not exist.", me.PathAsString));

            return me.InternalFileSystem().GetFileLastAccessed(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Sets the LastAccessed property on the file denoted by this descriptor.
        /// </summary>
        /// TODO: Exceptions!
        public static void SetLastAccessed(this RootedFile me, DateTime at)
        {
            RootedFileSystemItemVerifications.VerifyDateTime(at, "set last accessed date", "file");
            me.VerifyIsNotADirectory(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't set last accessed time for file '{0}' since it denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't set last accessed time for file '{0}' since it does not exist.", me.PathAsString));

            me.InternalFileSystem().SetFileLastAccessed(me.CanonicalPathAsString(), at);
        }

        /// <summary>
        /// Deletes the file denoted by this descriptor. If the file does not exists this method does nothing.
        /// </summary>
        /// <exception cref="System.IO.IOException">The file is in use; A directory is denoted by this file
        /// descriptor.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The caller does not have the required permission;
        /// The descriptor denotes a read-only file</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it
        /// is on an unmapped drive).</exception>
        public static void Delete(this RootedFile me)
        {
            me.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't delete the file '{0}' since the parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.CreateUnauthorizedAccessException(me.PathAsString, "Can't delete the directory '{0}' since it denotes a file.", me.PathAsString));
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
        /// </summary>
        /// <returns>
        /// True if the file no longer exists. That is, the file was either deleted, or it
        /// did not exist to start with. If the file descriptor denotes a directory this method
        /// returns true.
        /// </returns>
        public static bool TryDelete(this RootedFile me)
        {
            if (me.Exists())
            {
                //try
                {
                    me.Delete();
                }
// ReSharper disable EmptyGeneralCatchClause
                //catch
                //{
                //}
// ReSharper restore EmptyGeneralCatchClause
            }
            return !me.Exists();
        }

        /// <summary>
        /// Moves the file to a new location. After the move, the source file will have the name specified by the
        /// destination parameter.
        /// </summary>
        /// TODO: Exceptions
        public static void MoveTo(this RootedFile me, RootedFile destination)
        {
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyOnSameDriveAs(destination, ThrowHelper.CreateIOException("Can't move the file '{0}' to '{1}' since they are located on different drives.", me, destination));
            me.VerifyIsNotADirectory(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't move the file '{0}' since it denotes a directory.", me));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException(me.PathAsString, "Can't move the file '{0}' since it does not exist.", me));
            destination.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't move the file since the destination's parent directory '{0}' does not exist.", destination.ParentDirectory()));
            destination.VerifyIsNotAFile(ThrowHelper.CreateIOException("Can't move the file to the destination '{0}' since a file with that name already exists.", destination));
            destination.VerifyIsNotADirectory(ThrowHelper.CreateIOException("Can't move the file to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.CreateIOException("Can't move the file '{0}' since the source and destination denotes the same directory.", destination));

            var src = me.CanonicalPathAsString();
            var dst = destination.CanonicalPathAsString();
            var fileSystem = me.InternalFileSystem();
            fileSystem.MoveFile(src, dst);
        }

        /// <summary>
        /// Opens a read stream with the file denoted by this file descriptor as source.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The file cannot be found</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it
        /// is on an unmapped drive).</exception>
        /// <returns></returns>
        /// TODO: Revise exceptions
        public static Stream CreateReadStream(this RootedFile me)
        {
            me.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't open the file '{0}' for reading since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.CreateUnauthorizedAccessException("Can't open a file '{0}' for reading since the path denotes a directory.", me.PathAsString));
            me.VerifyIsAFile(ThrowHelper.CreateFileNotFoundException("Can't open the file '{0}' for reading since it does not exists.", me.PathAsString));

            return me.InternalFileSystem().CreateReadStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. If the file already exists
        /// the file is overwritten.
        /// </summary>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid.</exception>
        /// <returns></returns>
        /// TODO: Revise exceptions
        public static Stream CreateWriteStream(this RootedFile me)
        {
            me.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't create the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.CreateUnauthorizedAccessException("Can't create the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateWriteStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. The file is opened in
        /// write mode and the stream is position at the end of the file.
        /// </summary>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid.</exception>
        /// <returns></returns>
        /// TODO: Revise exceptions
        public static Stream CreateAppendStream(this RootedFile me)
        {
            me.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't append to the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.CreateUnauthorizedAccessException("Can't append to the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateAppendStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Opens a write stream with the file denoted by this file descriptor as source. The file is opened in
        /// read/write mode and the stream is positioned at the beginning of the file. If the file does not exist it is
        /// created.
        /// </summary>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.
        /// </exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The specified path is invalid.</exception>
        /// <returns></returns>
        /// TODO: Revise exceptions
        public static Stream CreateModifyStream(this RootedFile me)
        {
            me.ParentDirectory().VerifyIsADirectory(ThrowHelper.CreateDirectoryNotFoundException("Can't modify the file '{0}' since it's parent directory does not exist.", me.PathAsString));
            me.VerifyIsNotADirectory(ThrowHelper.CreateUnauthorizedAccessException("Can't modify the file '{0}' since the path denotes an existing directory.", me.PathAsString));

            return me.InternalFileSystem().CreateModifyStream(me.CanonicalPathAsString());
        }

        /// <summary>
        /// Returns a RootedFile where the filename is replaced with the given filename. That is, it points to a file
        /// in the same directory.
        /// </summary>
        public static RootedFile WithFileName(this RootedFile me, FileName newName)
        {
            return me.ParentDirectory() + newName;
        }

        /// <summary>
        /// Returns this file name on a form relative to the given directory.
        /// <example>(c:\path\to\file.txt, c:\path\in) => ..\to\file.txt</example>
        /// </summary>
        public static RelativeFile RelativeFrom(this RootedFile me, RootedDirectory other)
        {
            me.VerifyOnSameDriveAs(other, ThrowHelper.CreateArgumentException("Can't find a relative path since '{0}' and '{1}' have different drives.", me.PathAsString, other.PathAsString));

            return RelativeFile.FromString(PathUtils.MakeRelativeFrom(me.CanonicalPathAsString().FullPath, other.CanonicalPathAsString().FullPath));
        }
    }

    public static class RootedFileUtilities
    {
        public static string ReadText(this RootedFile me)
        {
            using (var stream = me.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static void WriteText(this RootedFile me, string text)
        {
            using (var stream = me.CreateWriteStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }

        public static void AppendText(this RootedFile me, string text)
        {
            using (var stream = me.CreateAppendStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }
    }
}