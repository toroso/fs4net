using fs4net.Framework;

namespace fs4net.Memory.Test
{
    internal class FileSystemFactory
    {
        internal static IFileSystem CreateFileSystemWithDrives()
        {
            var fileSystem = new MemoryFileSystem();
            fileSystem.AddDrive("c:");
            return fileSystem;
        }
    }
}