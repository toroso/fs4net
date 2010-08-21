using fs4net.Framework;
using NUnit.Framework;
using Template = fs4net.CommonTest.Template;

namespace fs4net.Memory.Test.File
{
    [TestFixture]
    public class LastModifiedFixture : Template.File.LastModifiedFixture
    {
        protected override IFileSystem CreateFileSystem()
        {
            return new MemoryFileSystem();
        }
    }
}