using System;
using System.Collections.Generic;
using fs4net.Framework.Impl;
using fs4net.Framework.Utils;

namespace fs4net.Framework
{
    /// <summary>
    /// An abstract representation of a file system and can be seen as a factory for rooted path descriptors. The rooted
    /// path descriptors can be used to operate on the file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// The IFileSystem interface is a facade that hides implementation details. This property returns an object
        /// representing the internal implementation. In most cases you can ignore this property, but in some testing
        /// scenarios it could be useful.
        /// </summary>
        IInternalFileSystem InternalFileSystem { get; }

        /// <summary>
        /// Returns the logger object where to abnormalities are reported.
        /// </summary>
        ILogger Logger { get; }
    }

    public static class FileSystemExtensions
    {
        /// <summary>
        /// Creates a file descriptor from the given path. This method throws if the path is invalid.
        /// This method will succeed even if the file does not exist.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.NonRootedPathException">The specified path is relative or empty.</exception>
        public static RootedFile FileDescribing(this IFileSystem fileSystem, string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedFile(fileSystem.InternalFileSystem, fullPath, fileSystem.Logger);
        }

        /// <summary>
        /// Creates a descriptor to a directory from the given path. This method throws if the path is invalid. The
        /// path may not end with a backslash.
        /// This method will succeed even if the directory does not exist.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.NonRootedPathException">The specified path is relative or empty.</exception>
        public static RootedDirectory DirectoryDescribing(this IFileSystem fileSystem, string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedDirectory(fileSystem.InternalFileSystem, fullPath, fileSystem.Logger);
        }

        /// <summary>
        /// Creates a descriptor to the temporary directory.
        /// </summary>
        public static RootedDirectory DirectoryDescribingTemporaryDirectory(this IFileSystem fileSystem)
        {
            return fileSystem.InternalFileSystem.DirectoryDescribingTemporaryDirectory();
        }

        /// <summary>
        /// Creates a descriptor to the current current working directory of the application.
        /// </summary>
        public static RootedDirectory DirectoryDescribingCurrentDirectory(this IFileSystem fileSystem)
        {
            return fileSystem.InternalFileSystem.DirectoryDescribingCurrentDirectory();
        }

        /// <summary>
        /// Creates a descriptor to the special folder identified by the parameter.
        /// </summary>
        public static RootedDirectory DirectoryDescribingSpecialFolder(this IFileSystem fileSystem, Environment.SpecialFolder folder)
        {
            return fileSystem.InternalFileSystem.DirectoryDescribingSpecialFolder(folder);
        }

        /// <summary>
        /// Creates a descriptor to a drive from the given drive name. The drive should be given without an ending
        /// backslash. Examples: "c:", "\\network\share".
        /// This method will succeed even if the drive does not exist.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path is empty or contains an invalid drive letter.</exception>
        public static Drive DriveDescribing(this IFileSystem fileSystem, string driveName)
        {
            return new Drive(fileSystem.InternalFileSystem, driveName, fileSystem.Logger);
        }

        /// <summary>
        /// Retrieves descriptors to all logical drives on the computer.
        /// </summary>
        public static IEnumerable<Drive> AllDrives(this IFileSystem fileSystem)
        {
            return fileSystem.InternalFileSystem.AllDrives();
        }

        /// <summary>
        /// Creates a file descriptor from the given path. If the given path is relative, the current directory
        /// is used to make the descriptor rooted.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified file system or the specified path is null.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        public static RootedFile FileFromCurrentDirectory(this IFileSystem fileSystem, string path)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            if (path.IsValidRootedFile())
            {
                return fileSystem.FileDescribing(path);
            }
            return fileSystem.DirectoryDescribingCurrentDirectory() + RelativeFile.FromString(path);
        }

        /// <summary>
        /// Creates a descriptor to a directory from the given path. If the given path is relative, the current
        /// directory is used to make the descriptor rooted.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified file system or the specified path is null.</exception>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        public static RootedDirectory DirectoryFromCurrentDirectory(this IFileSystem fileSystem, string path)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            if (path.IsValidRootedFile())
            {
                return fileSystem.DirectoryDescribing(path);
            }
            return fileSystem.DirectoryDescribingCurrentDirectory() + RelativeDirectory.FromString(path);
        }
    }
}
