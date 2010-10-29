using fs4net.Framework;
using fs4net.TestTemplates;

namespace fs4net.Memory.Test
{
    internal static class FileSystemFactory
    {
        internal static IFileSystem CreateFileSystemWithDrives()
        {
            return new MemoryFileSystem(AssertLogger.Instance).WithDrives("c:");
        }
    }
}