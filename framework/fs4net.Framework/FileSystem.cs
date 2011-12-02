using System;
using System.Collections.Generic;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// A wrapper around the real file system. All file operations done file file descriptors created from this class
    /// will be done on the real file system.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        private readonly FileSystemImpl _impl;

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
            _impl = new FileSystemImpl(logger);
        }

        public RootedFile FileDescribing(string fullPath)
        {
            return _impl.FileDescribing(fullPath);
        }

        public RootedDirectory DirectoryDescribing(string fullPath)
        {
            return _impl.DirectoryDescribing(fullPath);
        }

        public RootedDirectory DirectoryDescribingTemporaryDirectory()
        {
            return _impl.DirectoryDescribingTemporaryDirectory();
        }

        public RootedDirectory DirectoryDescribingCurrentDirectory()
        {
            return _impl.DirectoryDescribingCurrentDirectory();
        }

        public RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder)
        {
            return _impl.DirectoryDescribingSpecialFolder(folder);
        }

        public Drive DriveDescribing(string driveName)
        {
            return _impl.DriveDescribing(driveName);
        }

        public IEnumerable<Drive> AllDrives()
        {
            return _impl.AllDrives();
        }
    }
}