using System;
using fs4net.Framework;
using fs4net.TestTemplates;

namespace fs4net.Memory.Test
{
    internal static class FileSystemFactory
    {
        internal static IFileSystem CreateFileSystemWithDrives()
        {
            return new MemoryFileSystem(AssertLogger.Instance).WithDrives("c:", "d:");
        }

        public static IFileSystem CreateFileSystemWithDrives(Func<string, string> pathWasher)
        {
            return new MemoryFileSystem(AssertLogger.Instance, pathWasher).WithDrives("c:", "d:");
        }
    }
}