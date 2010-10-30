using System;

namespace fs4net.Builder
{
    public abstract class FileSystemItemBuilder<T> where T : FileSystemItemBuilder<T>
    {
        protected DateTime LastAccessTime { get; set; }
        protected DateTime LastModified { get; set; }

        protected abstract T Me();

        protected FileSystemItemBuilder()
        {
            LastAccessTime = DateTime.Now;
            LastModified = DateTime.Now;
        }

        public T LastAccessedAt(DateTime at)
        {
            LastAccessTime = at;
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