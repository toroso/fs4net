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
}