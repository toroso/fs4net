using Template = fs4net.CommonTest.Template;
using fs4net.Framework;

namespace fs4net.Memory.Test.File
{
    public class ExistsFixture : Template.File.ExistsFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}