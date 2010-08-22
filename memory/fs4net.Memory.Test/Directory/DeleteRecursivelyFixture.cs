using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class DeleteRecursivelyFixture : Template.Directory.DeleteRecursivelyFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystemWithDrives();
        }
    }
}