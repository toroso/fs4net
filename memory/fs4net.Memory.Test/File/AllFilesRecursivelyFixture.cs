using System.IO;
using System.Linq;
using fs4net.Builder;
using fs4net.Framework;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Memory.Test.File
{
    [TestFixture]
    public class AllFilesRecursivelyFixture
    {
        private MemoryFileSystem _fileSystem;
        private Drive _emptyDrive;
        private Drive _nonExistingDrive;
        private RootedDirectory _emptyDirectory;
        private RootedDirectory _nonExistingDirectory;
        private RootedDirectory _nonEmptyDirectory;

        [SetUp]
        public void Setup()
        {
            _fileSystem = new MemoryFileSystem().WithDrives("c:", "d:");

            _emptyDrive = _fileSystem.DriveDescribing("d:");
            _nonExistingDrive = _fileSystem.DriveDescribing("e:");
            _nonExistingDirectory = _fileSystem.DirectoryDescribing(@"c:\nonExistingDirectory");

            var populate = new FileSystemBuilder(_fileSystem);
            _emptyDirectory = populate.WithDir(@"c:\emptyDirectory");
            _nonEmptyDirectory = populate.WithDir(@"c:\crowdedDirectory");
            populate.WithDir(@"c:\crowdedDirectory\first");
            populate.WithDir(@"c:\crowdedDirectory\first\first");
            populate.WithDir(@"c:\crowdedDirectory\second");

            populate.WithFile(@"c:\crowdedDirectory\only.txt");
            populate.WithFile(@"c:\crowdedDirectory\first\first\only.txt");
            populate.WithFile(@"c:\crowdedDirectory\second\first.txt");
            populate.WithFile(@"c:\crowdedDirectory\second\second.txt");
        }

        [Test]
        public void DriveWithNoFiles()
        {
            Assert.That(_emptyDrive.AllFilesRecursively(), Is.Empty);
        }

        [Test]
        public void NonExistingDrive()
        {
            Assert.Throws<DirectoryNotFoundException >(() => _nonExistingDrive.AllFilesRecursively().ToList());
        }

        [Test]
        public void NonExistingDirectory()
        {
            Should.Throw<DirectoryNotFoundException>(() => _nonExistingDirectory.AllFilesRecursively().ToList());
        }

        [Test]
        public void DirectoryWithNoFiles()
        {
            Assert.That(_emptyDirectory.AllFilesRecursively(), Is.Empty);
        }

        [Test]
        public void DirectoryWithFilesInSubDirectories()
        {
            var rootedFiles = _nonEmptyDirectory.AllFilesRecursively().ToList();
            Assert.That(rootedFiles, Is.EquivalentTo(new[]
                {
                    _fileSystem.FileDescribing(@"c:\crowdedDirectory\only.txt"),
                    _fileSystem.FileDescribing(@"c:\crowdedDirectory\first\first\only.txt"),
                    _fileSystem.FileDescribing(@"c:\crowdedDirectory\second\first.txt"),
                    _fileSystem.FileDescribing(@"c:\crowdedDirectory\second\second.txt"),
                }));
        }
    }
}