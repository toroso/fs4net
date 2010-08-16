using fs4net.Framework;
using fs4net.Memory.Builder;
using NUnit.Framework;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class ExistsFixture
    {
        private MemoryFileSystem _fileSystem;

        private RootedDirectory ExistingLeafDirectory;
        private RootedDirectory ExistingParentDirectory;
        private RootedDirectory NonExistingDirectory;
        private RootedFile ExistingFile;

        [SetUp]
        public void PopulateFileSystem()
        {
            _fileSystem = new MemoryFileSystem();
            var populateFileSystem = new FileSystemBuilder(_fileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to");
            ExistingParentDirectory = _fileSystem.CreateDirectoryDescribing(@"c:\path");
            NonExistingDirectory = _fileSystem.CreateDirectoryDescribing(@"c:\another\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt");
        }

        [Test]
        public void Existing_Leaf_Directory_Exists()
        {
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
        }

        [Test]
        public void Existing_Parent_Directory_Exists()
        {
            Assert.That(ExistingParentDirectory.Exists(), Is.True);
        }

        [Test]
        public void NonExisting_Directory_Does_Not_Exists()
        {
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Existing_File_Does_Not_Exists_As_Directory()
        {
            Assert.That(_fileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString).Exists(), Is.False);
        }
    }
}