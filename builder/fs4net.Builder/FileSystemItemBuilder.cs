using System;

namespace fs4net.Builder
{
    public abstract class FileSystemItemBuilder<T> where T : FileSystemItemBuilder<T>
    {
        protected DateTime LastAccessTime { get; set; }
        protected DateTime LastWriteTime { get; set; }
        protected DateTime CreationTime { get; set; }

        protected abstract T Me();

        protected FileSystemItemBuilder()
        {
            LastAccessTime = DateTime.Now;
            LastWriteTime = DateTime.Now;
            CreationTime = DateTime.Now;
        }

        public T LastAccessedAt(DateTime at)
        {
            LastAccessTime = at;
            UpdateDates();
            return Me();
        }

        public T LastModifiedAt(DateTime at)
        {
            LastWriteTime = at;
            UpdateDates();
            return Me();
        }

        public T CreatedAt(DateTime at)
        {
            CreationTime = at;
            UpdateDates();
            return Me();
        }

        protected abstract void UpdateDates();
    }
}