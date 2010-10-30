using fs4net.Framework;
using fs4net.Framework.Utils;

namespace fs4net.Builder
{
    public sealed class RootedFileBuilder : FileSystemItemBuilder<RootedFileBuilder>
    {
        private readonly RootedFile _file;

        public RootedFileBuilder(RootedFile file)
        {
            _file = file;
            _file.ParentDirectory().Create();
            Containing(string.Empty); // Creates the file
        }

        protected override RootedFileBuilder Me()
        {
            return this;
        }

        protected override void UpdateDates()
        {
            _file.SetLastAccessTime(LastAccessTime);
            _file.SetLastWriteTime(LastWriteTime);
        }

        public RootedFileBuilder Containing(string contents)
        {
            _file.WriteText(contents);
            UpdateDates();
            return Me();
        }

        public static implicit operator RootedFile(RootedFileBuilder me)
        {
            return me._file;
        }
    }
}