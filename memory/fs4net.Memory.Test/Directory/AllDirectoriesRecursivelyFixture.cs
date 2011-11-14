using System.IO;
using System.Linq;
using fs4net.Builder;
using fs4net.Framework;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class AllDirectoriesRecursivelyFixture
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
        }

        [Test]
        public void DriveWithNoDirectories()
        {
            Assert.That(_emptyDrive.AllDirectoriesRecursively(), Is.Empty);
        }

        [Test]
        public void NonExistingDrive()
        {
            Assert.Throws<DirectoryNotFoundException >(() => _nonExistingDrive.AllDirectoriesRecursively().ToList());
        }

        [Test]
        public void NonExistingDirectory()
        {
            Should.Throw<DirectoryNotFoundException>(() => _nonExistingDirectory.AllDirectoriesRecursively().ToList());
        }

        [Test]
        public void DirectoryWithNoSubDirectories()
        {
            Assert.That(_emptyDirectory.AllDirectoriesRecursively(), Is.Empty);
        }

        [Test]
        public void DirectoryWithSubDirectories()
        {
            Assert.That(_nonEmptyDirectory.AllDirectoriesRecursively(), Is.EquivalentTo(new[]
                {
                    _fileSystem.DirectoryDescribing(@"c:\crowdedDirectory\first"),
                    _fileSystem.DirectoryDescribing(@"c:\crowdedDirectory\first\first"),
                    _fileSystem.DirectoryDescribing(@"c:\crowdedDirectory\second"),
                }));
        }
    }
}