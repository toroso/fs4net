using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class LastModifiedFixture : Template.Directory.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystemWithDrives();
        }
    }
}