using System;
using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public class FileSystemBuilder
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemBuilder(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public RootedDirectoryBuilder WithDir(string path)
        {
            return new RootedDirectoryBuilder(_fileSystem, path);
        }

        public RootedFileBuilder WithFile(string path)
        {
            return new RootedFileBuilder(_fileSystem, path);
        }
    }

    public class RootedDirectoryBuilder
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _path;
        private DateTime _lastModified = DateTime.Now;

        public RootedDirectoryBuilder(IFileSystem fileSystem, string path)
        {
            _fileSystem = fileSystem;
            _path = path;
        }

        public RootedDirectoryBuilder LastModifiedAt(DateTime at)
        {
            _lastModified = at;
            return this;
        }

        public static implicit operator RootedDirectory (RootedDirectoryBuilder me)
        {
            return me.Build();
        }

        private RootedDirectory Build()
        {
            var dir = _fileSystem.CreateDirectoryDescribing(_path);
            dir.Create();
            dir.SetLastModified(_lastModified);
            return dir;
        }
    }

    public class RootedFileBuilder
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _path;
        private DateTime _lastModified = DateTime.Now;
        private string _content = string.Empty;

        public RootedFileBuilder(IFileSystem fileSystem, string path)
        {
            _fileSystem = fileSystem;
            _path = path;
        }

        public RootedFileBuilder LastModifiedAt(DateTime at)
        {
            _lastModified = at;
            return this;
        }

        public static implicit operator RootedFile(RootedFileBuilder me)
        {
            return me.Build();
        }

        private RootedFile Build()
        {
            var file = _fileSystem.CreateFileDescribing(_path);
            file.WriteText(_content);
            file.SetLastModified(_lastModified);
            return file;
        }
    }
}