using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class RootedPathAsStringFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        private static readonly string[] RootedPaths =
            {
                @"c:\standard\case\to\fileOrDirectory.txt",
                @"c:\single\.\dots\to\.\fileOrDirectory.txt",
                @"c:\double\..\dots\to\..\fileOrDirectory.txt",
            };


        [Test]
        public void RootedFile_PathAsString_Is_Intact()
        {
            RootedPaths.ForEach(RootedFile_PathAsString_Is_Intact);
        }

        [Test, TestCaseSource("RootedPaths")]
        public void RootedFile_PathAsString_Is_Intact(string path)
        {
            Assert.That(_fileSystem.CreateFileDescribing(path).PathAsString, Is.EqualTo(path));
        }

        [Test]
        public void RootedDirectory_PathAsString_Is_Intact()
        {
            RootedPaths.ForEach(RootedDirectory_PathAsString_Is_Intact);
        }

        [Test, TestCaseSource("RootedPaths")]
        public void RootedDirectory_PathAsString_Is_Intact(string path)
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(path).PathAsString, Is.EqualTo(path));
        }


        [Test]
        public void RootedDirectory_Ending_Backslash_Remains_In_PathAsString()
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(@"c:\path\to\").PathAsString, Is.EqualTo(@"c:\path\to\"));
        }

        [Test]
        public void RootedDirectory_Ending_Dot_Remains_In_PathAsString()
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(@"c:\path\to\.").PathAsString, Is.EqualTo(@"c:\path\to\."));
        }

        [Test]
        public void RootedDirectory_Ending_DoubleDots_Remains_In_PathAsString()
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(@"c:\path\to\..").PathAsString, Is.EqualTo(@"c:\path\to\.."));
        }
    }
}