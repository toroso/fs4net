using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public class RootedFileBuilder : FileSystemItemBuilder<RootedFileBuilder>
    {
        public string Content { get; private set; }

        public RootedFileBuilder(IFileSystem fileSystem, string path)
            : base(fileSystem, path)
        {
            Content = string.Empty;
        }

        protected override RootedFileBuilder Me()
        {
            return this;
        }

        public static implicit operator RootedFile(RootedFileBuilder me)
        {
            return me.Build();
        }

        private RootedFile Build()
        {
            var file = FileSystem.CreateFileDescribing(Path);
            file.WriteText(Content);
            file.SetLastModified(LastModified);
            return file;
        }
    }
}