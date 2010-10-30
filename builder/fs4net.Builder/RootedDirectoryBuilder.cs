using fs4net.Framework;

namespace fs4net.Builder
{
    public sealed class RootedDirectoryBuilder : FileSystemItemBuilder<RootedDirectoryBuilder>
    {
        private readonly RootedDirectory _dir;

        public RootedDirectoryBuilder(RootedDirectory dir)
        {
            _dir = dir;
            _dir.Create();
        }

        protected override RootedDirectoryBuilder Me()
        {
            return this;
        }

        protected override void UpdateDates()
        {
            _dir.SetLastAccessTime(LastAccessTime);
            _dir.SetLastModified(LastModified);
        }

        public static implicit operator RootedDirectory (RootedDirectoryBuilder me)
        {
            return me._dir;
        }
    }
}