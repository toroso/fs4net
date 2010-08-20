using Template = fs4net.CommonTest.Template;
using fs4net.Framework;

namespace fs4net.Memory.Test.File
{
    public class LastModifiedFixture : Template.File.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}