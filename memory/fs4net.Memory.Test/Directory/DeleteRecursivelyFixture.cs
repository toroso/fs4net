using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class DeleteRecursivelyFixture : Template.Directory.DeleteRecursivelyFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystemWithDrives();
        }

        protected override void DisposeFileSystem(IFileSystem fileSystem)
        {
            ((MemoryFileSystem)fileSystem).Dispose();
        }
    }
}