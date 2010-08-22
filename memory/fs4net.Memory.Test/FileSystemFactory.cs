using fs4net.Framework;

namespace fs4net.Memory.Test
{
    internal class FileSystemFactory
    {
        internal static IFileSystem CreateFileSystemWithDrives()
        {
            return new MemoryFileSystem().WithDrives("c:");
        }
    }
}