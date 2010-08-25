using System;

namespace fs4net.CommonTest.Builder
{
    public abstract class FileSystemItemBuilder<T> where T : FileSystemItemBuilder<T>
    {
        protected abstract DateTime LastAccessed { set; }
        protected abstract DateTime LastModified { set; }

        protected abstract T Me();

        public T LastAccessedAt(DateTime at)
        {
            LastAccessed = at;
            return Me();
        }

        public T LastModifiedAt(DateTime at)
        {
            LastModified = at;
            return Me();
        }
    }
}