using System;
using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public abstract class FileSystemItemBuilder<T> where T : FileSystemItemBuilder<T>
    {
        protected IFileSystem FileSystem { get; private set; }
        protected DateTime LastModified { get; private set; }

        protected FileSystemItemBuilder(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            LastModified = DateTime.Now;
        }

        protected abstract T Me();

        public T LastModifiedAt(DateTime at)
        {
            LastModified = at;
            return Me();
        }
    }
}