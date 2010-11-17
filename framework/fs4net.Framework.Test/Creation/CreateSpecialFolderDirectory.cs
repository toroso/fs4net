using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateSpecialFolderDirectory : Template.Creation.CreateSpecialFolderDirectory
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}