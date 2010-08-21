using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class LastModifiedFixture : Template.Directory.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}