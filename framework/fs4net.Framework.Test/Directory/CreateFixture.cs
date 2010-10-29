using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class CreateFixture : Template.Directory.CreateFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}