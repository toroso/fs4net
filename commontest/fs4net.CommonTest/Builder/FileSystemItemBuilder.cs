using System;

namespace fs4net.CommonTest.Builder
{
    public abstract class FileSystemItemBuilder<T> where T : FileSystemItemBuilder<T>
    {
        protected DateTime LastAccessed { get; set; }
        protected DateTime LastModified { get; set; }

        protected abstract T Me();

        protected FileSystemItemBuilder()
        {
            LastAccessed = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public T LastAccessedAt(DateTime at)
        {
            LastAccessed = at;
            UpdateDates();
            return Me();
        }

        public T LastModifiedAt(DateTime at)
        {
            LastModified = at;
            UpdateDates();
            return Me();
        }

        protected abstract void UpdateDates();
    }
}