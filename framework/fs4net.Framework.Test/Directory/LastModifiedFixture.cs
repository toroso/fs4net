using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class LastModifiedFixture : Template.Directory.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}