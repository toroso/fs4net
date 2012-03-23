using System;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateDirectoryFromCurrentDirectoryFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
            _fileSystem.DirectoryDescribing(@"c:\current\directory").SetAsCurrent();
        }


        [Test]
        public void Throws_If_FileSystem_Is_Null()
        {
// ReSharper disable PossibleNullReferenceException
            const IFileSystem nullFileSystem = null;
            Should.Throw<NullReferenceException>(() => nullFileSystem.DirectoryFromCurrentDirectory(@"file/system/is/null"));
// ReSharper restore PossibleNullReferenceException
        }

        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _fileSystem.DirectoryFromCurrentDirectory(null));
        }

        [Test]
        public void Create_From_Rooted_Path()
        {
            var path = _fileSystem.DirectoryFromCurrentDirectory(@"c:\path\to");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\path\to"));
        }

        [Test]
        public void Create_From_Empty_String()
        {
            var path = _fileSystem.DirectoryFromCurrentDirectory(string.Empty);
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current\directory"));
        }

        [Test]
        public void Create_From_Parent_String()
        {
            var path = _fileSystem.DirectoryFromCurrentDirectory("..");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current"));
        }

        [Test]
        public void Create_From_Relative_Directory()
        {
            var path = _fileSystem.DirectoryFromCurrentDirectory(@"has\a\folder");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current\directory\has\a\folder"));
        }

        [Test]
        public void Create_From_Relative_Directory_In_Parent_Directory()
        {
            var path = _fileSystem.DirectoryFromCurrentDirectory(@"..\folder");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current\folder"));
        }

        [Test]
        public void Create_From_Directory_Above_Root_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.DirectoryFromCurrentDirectory(@"..\..\..\path\to"));
        }
    }
}