using System;
using System.Collections.Generic;
using fs4net.Framework;
using fs4net.Memory.Impl;

namespace fs4net.Memory
{
    /// <summary>
    /// An in-memory file system. All files and directories created are stored in memory. When the Dispose() method is
    /// called the file system content is erased.
    /// This file system is supposed to behave exactly as the real file system which means it's a nice thing to use in
    /// tests.
    /// To make the file system usable you ought to call the WithDrives() method to configure what drives exists.
    /// Otherwise the file system has no drives at all.
    /// </summary>
    public sealed class MemoryFileSystem : IFileSystem, IDisposable
    {
        private readonly MemoryFileSystemImpl _impl;

        /// <summary>
        /// Instantiate an in-memory file system. The instance is created without a logger which means that all logged
        /// events are swallowed.
        /// </summary>
        public MemoryFileSystem()
            : this(NullLogger.Instance)
        {
        }

        /// <summary>
        /// Instantiate an in-memory file system. The instance is created with a logger where logged events are
        /// reported.
        /// </summary>
        /// <param name="logger">Anything worth reporting inside the fs4net classes are sent to this logger instance.</param>
        public MemoryFileSystem(ILogger logger)
        {
            _impl = new MemoryFileSystemImpl(logger);
        }

        /// <summary>
        /// Specifies the drives that this FileSystem contains. If called several time, the specified drives are appended to the previously specified ones.
        /// The method returns the same instance of the FileSystem.
        /// </summary>
        public MemoryFileSystem WithDrives(params string[] driveNames)
        {
            _impl.WithDrives(driveNames);
            return this;
        }

        /// <summary>
        /// Erases all files and directories in the file system and frees up the memory being used.
        /// </summary>
        public void Dispose()
        {
            _impl.Dispose();
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