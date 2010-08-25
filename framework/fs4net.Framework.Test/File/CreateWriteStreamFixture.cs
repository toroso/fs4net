using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Framework.Test.File
{
    [TestFixture]
    public class CreateWriteStreamFixture : Template.File.CreateWriteStreamFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new FileSystem();
        }
    }
}
