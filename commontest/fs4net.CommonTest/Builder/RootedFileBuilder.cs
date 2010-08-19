using fs4net.Framework;

namespace fs4net.CommonTest.Builder
{
    public class RootedFileBuilder : FileSystemItemBuilder<RootedFileBuilder>
    {
        private readonly RootedFile _file;
        public string Content { get; private set; }

        public RootedFileBuilder(IFileSystem fileSystem, RootedFile file)
            : base(fileSystem)
        {
            _file = file;
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
            _file.WriteText(Content);
            _file.SetLastModified(LastModified);
            return _file;
        }
    }
}