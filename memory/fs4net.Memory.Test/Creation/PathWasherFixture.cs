using System;
using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Memory.Test.Creation
{
    [TestFixture]
    public class PathWasherFixture : Template.Creation.PathWasherFixture
    {
        protected override IFileSystem CreateFileSystem(Func<string, string> pathWasher)
        {
            return FileSystemFactory.CreateFileSystemWithDrives(pathWasher);
        }

        protected override void DisposeFileSystem(IFileSystem fileSystem)
        {
            ((MemoryFileSystem)fileSystem).Dispose();
        }
    }
}