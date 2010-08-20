using Template = fs4net.CommonTest.Template;
using fs4net.Framework;

namespace fs4net.Memory.Test.Directory
{
    public class LastModifiedFixture : Template.Directory.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}