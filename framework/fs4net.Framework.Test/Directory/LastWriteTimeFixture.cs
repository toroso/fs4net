using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class LastWriteTimeFixture : Template.Directory.LastWriteTimeFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return FileSystemFactory.CreateFileSystem();
        }
    }
}