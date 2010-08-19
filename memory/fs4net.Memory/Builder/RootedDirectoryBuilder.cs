using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public class RootedDirectoryBuilder : FileSystemItemBuilder<RootedDirectoryBuilder>
    {
        public RootedDirectoryBuilder(IFileSystem fileSystem, string path)
            : base(fileSystem, path)
        {
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
            var dir = FileSystem.CreateDirectoryDescribing(Path);
            dir.Create();
            dir.SetLastModified(LastModified);
            return dir;
        }
    }
}