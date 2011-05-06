using fs4net.TestTemplates;

namespace fs4net.Framework.Test
{
    internal static class FileSystemFactory
    {
        public static IFileSystem CreateFileSystem()
        {
            return new FileSystem(AssertLogger.Instance);
        }
    }
}