using System;
using fs4net.Framework;

namespace fs4net.CommonTest.Builder
{
    public sealed class RootedFileBuilder : FileSystemItemBuilder<RootedFileBuilder>
    {
        private readonly RootedFile _file;

        public RootedFileBuilder(RootedFile file)
        {
            _file = file;
            _file.ParentDirectory().Create();
            Content = string.Empty;
            LastAccessed = DateTime.Now;
            LastModified = DateTime.Now;
        }

        protected override RootedFileBuilder Me()
        {
            return this;
        }

        public static implicit operator RootedFile(RootedFileBuilder me)
        {
            return me._file;
        }

        private string Content { set { _file.WriteText(value); } }
        protected override DateTime LastAccessed { set { _file.SetLastAccessed(value); } }
        protected override DateTime LastModified { set { _file.SetLastModified(value); } }
    }
}