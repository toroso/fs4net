using System;
using System.IO;
using fs4net.Framework;
using fs4net.Memory.Builder;
using NUnit.Framework;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class LastModifiedFixture
    {
        private MemoryFileSystem _fileSystem;

        private RootedDirectory ExistingLeafDirectory;
        private readonly DateTime ExistingLeafDirectoryLastModified = new DateTime(13243546576879);
        private RootedDirectory NonExistingDirectory;
        private RootedFile ExistingFile;

        [SetUp]
        public void PopulateFileSystem()
        {
            _fileSystem = new MemoryFileSystem();
            var populateFileSystem = new FileSystemBuilder(_fileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to").LastModifiedAt(ExistingLeafDirectoryLastModified);
            NonExistingDirectory = _fileSystem.CreateDirectoryDescribing(@"c:\another\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt");
        }

        [Test]
        public void LastModified_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.LastModified(), Is.EqualTo(ExistingLeafDirectoryLastModified));
        }

        [Test]
        public void LastModified_On_Directory_For_Existing_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => _fileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString).LastModified());
        }

        [Test]
        public void LastModified_On_NonExisting_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingDirectory.LastModified());
        }
    }
}