using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class RootedFileSystemItemFixture
    {
        private IFileSystem _fileSystem;

        [SetUp]
        public void SetUp()
        {
            _fileSystem = new MockFileSystem();
        }

        [Test]
        public void RootedFile_Has_Correct_FileSystem()
        {
            Assert.That(_fileSystem.CreateFileDescribing(@"c:\standard\path\to\file.txt").FileSystem, Is.EqualTo(_fileSystem));
        }

        [Test]
        public void RootedDirectory_Has_Correct_FileSystem()
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(@"c:\standard\path\to").FileSystem, Is.EqualTo(_fileSystem));
        }
    }
}