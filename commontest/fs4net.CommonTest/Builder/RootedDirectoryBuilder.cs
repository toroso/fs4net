using fs4net.Framework;

namespace fs4net.CommonTest.Builder
{
    public class RootedDirectoryBuilder : FileSystemItemBuilder<RootedDirectoryBuilder>
    {
        private readonly RootedDirectory _dir;

        public RootedDirectoryBuilder(IFileSystem fileSystem, RootedDirectory dir)
            : base(fileSystem)
        {
            _dir = dir;
        }

        protected override RootedDirectoryBuilder Me()
        {
            return this;
        }

        public static implicit operator RootedDirectory (RootedDirectoryBuilder me)
        {
            return me.Build();
        }

        private RootedDirectory Build()
        {
            _dir.Create();
            _dir.SetLastModified(LastModified);
            return _dir;
        }
    }
}