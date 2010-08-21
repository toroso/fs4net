using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class MoveToFixture : Template.Directory.MoveToFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}