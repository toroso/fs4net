using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class ExistsFixture : Template.Directory.ExistsFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new MemoryFileSystem();
        }
    }
}