using fs4net.Framework;

namespace fs4net.CommonTest.Builder
{
    public class FileSystemBuilder
    {
        private readonly IFileSystem _fileSystem;
        private readonly RootedDirectory _rootDir;

        public FileSystemBuilder(IFileSystem fileSystem, RootedDirectory rootDir)
        {
            _fileSystem = fileSystem;
            _rootDir = rootDir;
        }

        public RootedDirectoryBuilder WithDir(string path)
        {
            return new RootedDirectoryBuilder(_fileSystem, _rootDir + RelativeDirectory.FromString(path));
        }

        public RootedFileBuilder WithFile(string path)
        {
            return new RootedFileBuilder(_fileSystem, _rootDir + RelativeFile.FromString(path));
        }
    }
}