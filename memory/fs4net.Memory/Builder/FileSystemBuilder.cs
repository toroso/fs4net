using System;
using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public class FileSystemBuilder
    {
        private readonly IBuildable _buildable;

        public FileSystemBuilder(IBuildable buildable)
        {
            _buildable = buildable;
        }

        public RootedDirectoryBuilder WithDir(string path)
        {
            return new RootedDirectoryBuilder(path, _buildable);
        }

        public RootedFile WithFile(string path)
        {
            RootedFile result = _buildable.CreateFileDescribing(path);
            using (result.CreateWriteStream()) { }
            return result;
        }
    }

    public class RootedDirectoryBuilder
    {
        private readonly string _path;
        private readonly IBuildable _buildable;

        public RootedDirectoryBuilder(string path, IBuildable buildable)
        {
            _path = path;
            _buildable = buildable;
            _buildable.BuildDirectory(_path);
        }

        public RootedDirectoryBuilder LastModifiedAt(DateTime at)
        {
            _buildable.SetLastModified(_path, at);
            return this;
        }

        public static implicit operator RootedDirectory (RootedDirectoryBuilder me)
        {
            return me._buildable.CreateDirectoryDescribing(me._path);
        }
    }
}