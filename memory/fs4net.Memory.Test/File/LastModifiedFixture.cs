using System;
using System.IO;
using fs4net.Framework;
using fs4net.Memory.Builder;
using NUnit.Framework;

namespace fs4net.Memory.Test.File
{
    [TestFixture]
    public class LastModifiedFixture
    {
        private MemoryFileSystem _fileSystem;

        private RootedDirectory ExistingLeafDirectory;
        private RootedFile ExistingFile;
        private readonly DateTime ExistingFileLastModified = new DateTime(112358132134);
        private RootedFile NonExistingFile;

        [SetUp]
        public void PopulateFileSystem()
        {
            _fileSystem = new MemoryFileSystem();
            var populateFileSystem = new FileSystemBuilder(_fileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt").LastModifiedAt(ExistingFileLastModified);
            NonExistingFile = _fileSystem.CreateFileDescribing(@"c:\another\path\to\file.txt");
        }

        [Test]
        public void LastModified_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(ExistingFileLastModified));
        }

        [Test]
        public void LastModified_On_File_For_Existing_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => _fileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).LastModified());
        }

        [Test]
        public void LastModified_On_NonExisting_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.LastModified());
        }
    }
}