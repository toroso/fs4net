using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.File
{
    [TestFixture]
    public class CreateReadStreamFixture : Template.File.CreateReadStreamFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}
