using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class LeafFolderFixture
    {
        private IFileSystem _fileSystem;

        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }

        [Test]
        public void LeafFolder_Of_Drive()
        {
            var original = _fileSystem.DriveDescribing("c:");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RootedDirectory_That_Denotes_Drive()
        {
            var original = _fileSystem.DirectoryDescribing("c:");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        [Ignore(@"Converting the path 'c:\path\..' to canonical form throws")]
        public void LeafFolder_Of_RootedDirectory_With_A_CanonicalPath_That_Denotes_Drive()
        {
            var original = _fileSystem.DirectoryDescribing(@"c:\path\..");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RootedDirectory()
        {
            var original = _fileSystem.DirectoryDescribing(@"c:\path\to");
            var expected = RelativeDirectory.FromString("to");
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RootedDirectory_Ending_With_DoubleDots()
        {
            var original = _fileSystem.DirectoryDescribing(@"c:\path\to\..");
            var expected = RelativeDirectory.FromString("path");
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString(@"path\to");
            var expected = RelativeDirectory.FromString("to");
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RelativeDirectory_Ending_With_DoubleDots()
        {
            var original = RelativeDirectory.FromString(@"path\to\..");
            var expected = RelativeDirectory.FromString("path");
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_Empty_RelativeDirectory()
        {
            var original = RelativeDirectory.FromString(string.Empty);
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RelativeDirectory_That_Denotes_Empty_Directory()
        {
            var original = RelativeDirectory.FromString(@"path\..");
            var expected = RelativeDirectory.FromString(string.Empty);
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }

        [Test]
        public void LeafFolder_Of_RelativeDirectory_With_DoubleDots()
        {
            var original = RelativeDirectory.FromString("..");
            var expected = RelativeDirectory.FromString("..");
            Assert.That(original.LeafFolder(), Is.EqualTo(expected));
        }
    }
}