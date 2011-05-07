using System;
using System.Collections.Generic;
using fs4net.Framework.Impl;
using fs4net.Framework.Utils;

namespace fs4net.Framework
{
    public interface IFileSystem
    {
        /// <summary>
        /// Creates a file descriptor from the given path. This method throws if the path is invalid.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The specified path in its canonical form exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="System.ArgumentException">The specified path is empty, start or ends with white space,
        /// contains one or more invalid characters or contains an invalid drive letter.</exception>
        /// TODO: The exception list is wrong!
        RootedFile FileDescribing(string fullPath);

        /// <summary>
        /// Creates a descriptor to a directory from the given path. This method throws if the path is invalid.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The specified path in its canonical form exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="System.ArgumentException">The specified path is empty, start or ends with white space,
        /// contains one or more invalid characters or contains an invalid drive letter.</exception>
        /// TODO: The exception list is wrong!
        RootedDirectory DirectoryDescribing(string fullPath);

        /// <summary>
        /// Creates a descriptor to the temporary directory.
        /// </summary>
        RootedDirectory DirectoryDescribingTemporaryDirectory();

        /// <summary>
        /// Creates a descriptor to the current current working directory of the application.
        /// </summary>
        RootedDirectory DirectoryDescribingCurrentDirectory();

        /// <summary>
        /// Creates a descriptor to the special folder identified by the parameter.
        /// </summary>
        RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder);

        /// <summary>
        /// Creates a descriptor to a drive from the given drive name. The drive should be given without an ending
        /// backslash. Examples: "c:", "\\network\share".
        /// </summary>
        /// TODO: Exception list!
        Drive DriveDescribing(string driveName);

        /// <summary>
        /// Retrieves descriptors to all logical drives on the computer.
        /// </summary>
        /// TODO: Exceptions
        IEnumerable<Drive> AllDrives();

        void SetCurrentDirectory(RootedDirectory dir);
    }

    public static class FileSystemExternsions
    {
        /// <summary>
        /// Creates a file descriptor from the given path. If the given path is relative, the current directory
        /// is used to make the descriptor rooted.
        /// This method throws if the path is invalid.
        /// </summary>
        /// TODO: Exceptions
        public static RootedFile FileFromCurrentDirectory(this IFileSystem fileSystem, string fullPath)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            if (fullPath.IsValidRootedFile())
            {
                return fileSystem.FileDescribing(fullPath);
            }
            return fileSystem.DirectoryDescribingCurrentDirectory() + RelativeFile.FromString(fullPath);
        }

        /// <summary>
        /// Creates a descriptor to a directory from the given path. If the given path is relative, the current
        /// directory is used to make the descriptor rooted.
        /// This method throws if the path is invalid.
        /// </summary>
        /// TODO: Exceptions
        public static RootedDirectory DirectoryFromCurrentDirectory(this IFileSystem fileSystem, string fullPath)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            if (fullPath.IsValidRootedFile())
            {
                return fileSystem.DirectoryDescribing(fullPath);
            }
            return fileSystem.DirectoryDescribingCurrentDirectory() + RelativeDirectory.FromString(fullPath);
        }
    }
}
