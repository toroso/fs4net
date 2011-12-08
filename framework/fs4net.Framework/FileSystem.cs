using System;
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
    }
}