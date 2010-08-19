using Template = fs4net.CommonTest.Template;
using fs4net.Framework;

namespace fs4net.Memory.Test.Directory
{
    public class ExistsFixture : Template.Directory.ExistsFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new MemoryFileSystem();
        }
    }
}