using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateSpecialFolderDirectoryFixture : Template.Creation.CreateSpecialFolderDirectoryFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}