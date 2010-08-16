using fs4net.Framework;
using fs4net.Memory.Builder;
using NUnit.Framework;

namespace fs4net.Memory.Test.File
{
    [TestFixture]
    public class ExistsFixture
    {
        private MemoryFileSystem _fileSystem;

        private RootedDirectory ExistingLeafDirectory;
        private RootedFile ExistingFile;
        private RootedFile NonExistingFile;

        [SetUp]
        public void PopulateFileSystem()
        {
            _fileSystem = new MemoryFileSystem();
            var populateFileSystem = new FileSystemBuilder(_fileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt");
            NonExistingFile = _fileSystem.CreateFileDescribing(@"c:\another\path\to\file.txt");
        }

        [Test]
        public void Existing_File_Exists()
        {
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void NonExisting_File_Does_Not_Exists()
        {
            Assert.That(NonExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Existing_Directory_Does_Not_Exists_As_File()
        {
            Assert.That(_fileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).Exists(), Is.False);
        }
    }
}