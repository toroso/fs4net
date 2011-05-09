using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class ParentFixture
    {
        private IFileSystem _fileSystem;

        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }

        
        [Test]
        public void Parent_Of_RootedDirecory()
        {
            var original = _fileSystem.DirectoryDescribing(@"c:\my\path\to");
            var expected = _fileSystem.DirectoryDescribing(@"c:\my\path");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_RootedFile()
        {
            var original = _fileSystem.FileDescribing(@"c:\my\path\to\file.txt");
            var expected = _fileSystem.FileDescribing(@"c:\my\path\to");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_RelativeDirecory()
        {
            RelativeDirectory original = RelativeDirectory.FromString(@"my\path\to");
            var expected = RelativeDirectory.FromString(@"my\path");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_RelativeFile()
        {
            var original = RelativeFile.FromString(@"my\path\to\file.txt");
            var expected = RelativeFile.FromString(@"my\path\to");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_Drive_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.DriveDescribing("c:").Parent());
        }

        [Test]
        public void Parent_Of_RootedDirectory_That_Denotes_Drive_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.DirectoryDescribing("c:").Parent());
        }

        [Test]
        public void Parent_Of_RootedDirectory_With_A_CanonicalPath_That_Denotes_Drive_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.DirectoryDescribing(@"c:\path\..").Parent());
        }
        
        [Test]
        public void Parent_Of_FolderName_Returns_Empty_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString("path");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_Empty_Directory_Returns_DoubleDots_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString(string.Empty);
            var expected = RelativeDirectory.FromString("..");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_DoubleDots_Directory_Returns_Double_DoubleDots_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString("..");
            var expected = RelativeDirectory.FromString(@"..\..");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_RelativeDirectory_Ending_With_DoubleDots_Returns_Canonically_Corrected_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString(@"my\path\to\..");
            var expected = RelativeDirectory.FromString("my");
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }

        [Test]
        public void Parent_Of_FileName_Returns_Empty_RelativeDirectory()
        {
            var original = FileName.FromString("file.txt");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.Parent(), Is.EqualTo(expected));
        }
    }
}