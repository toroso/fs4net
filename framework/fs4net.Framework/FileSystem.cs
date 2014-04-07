using System;
using System.Collections.Generic;
using System.Linq;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// A wrapper around the real file system. All file operations done file file descriptors created from this class
    /// will be done on the real file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        public IInternalFileSystem InternalFileSystem { get; private set; }
        public ILogger Logger { get; private set; }

        /// <summary>
        /// Instantiate a file system wrapper. The instance is created without a logger which means that all logged
        /// events are swallowed.
        /// </summary>
        public FileSystem()
            : this(NullLogger.Instance)
        {
        }

        /// <summary>
        /// Instantiate a file system wrapper. The instance is created with a logger where logged events are reported.
        /// </summary>
        /// <param name="logger">Anything worth reporting inside the fs4net classes are sent to this logger instance.</param>
        public FileSystem(ILogger logger)
        {
            InternalFileSystem = new FileSystemImpl(this);
            Logger = logger;
        }

        public RootedFile FileDescribing(string fullPath)
        {
            return FileSystemExtensions.FileDescribing(this, fullPath);
        }

        public RootedDirectory DirectoryDescribing(string fullPath)
        {
            return FileSystemExtensions.DirectoryDescribing(this, fullPath);
        }

        public Drive DriveDescribing(string driveName)
        {
            return FileSystemExtensions.DriveDescribing(this, driveName);
        }

        public RootedFile FileFromCurrentDirectory(string path)
        {
            return FileSystemExtensions.FileFromCurrentDirectory(this, path);
        }

        public RootedDirectory DirectoryFromCurrentDirectory(string path)
        {
            return FileSystemExtensions.DirectoryFromCurrentDirectory(this, path);
        }

        public RootedDirectory DirectoryDescribingTemporaryDirectory()
        {
            var tempPathWithBackslash = System.IO.Path.GetTempPath();
            return DirectoryDescribing(tempPathWithBackslash.Remove(tempPathWithBackslash.Length - 1));
        }

        public RootedDirectory DirectoryDescribingCurrentDirectory()
        {
            return InternalFileSystem.GetCurrentDirectory();
        }

        public RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder)
        {
            if (folder == Environment.SpecialFolder.MyComputer) throw new NotSupportedException("MyComputer cannot be denoted by a RootedDirectory.");
            return DirectoryDescribing(Environment.GetFolderPath(folder));
        }

        public IEnumerable<Drive> AllDrives()
        {
            return System.IO.DriveInfo.GetDrives()
                .Select(driveInfo => DriveDescribing(driveInfo.Name.RemoveTrailingPathSeparators()));
        }
    }
}