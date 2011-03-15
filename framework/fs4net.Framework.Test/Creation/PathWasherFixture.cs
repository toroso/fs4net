using System;
using NUnit.Framework;
using Template = fs4net.TestTemplates;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class PathWasherFixture : Template.Creation.PathWasherFixture
    {
        protected override IFileSystem CreateFileSystem(Func<string, string> pathWasher)
        {
            return FileSystemFactory.CreateFileSystem(pathWasher);
        }
    }
}