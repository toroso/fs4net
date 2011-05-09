using System;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateFileFromCurrentDirectoryFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
            _fileSystem.SetCurrentDirectory(_fileSystem.DirectoryDescribing(@"c:\current\directory"));
        }


        [Test]
        public void Throws_If_FileSystem_Is_Null()
        {
// ReSharper disable PossibleNullReferenceException
            const IFileSystem nullFileSystem = null;
            Should.Throw<ArgumentNullException>(() => nullFileSystem.FileFromCurrentDirectory(@"file/system/is/null"));
// ReSharper restore PossibleNullReferenceException
        }

        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _fileSystem.FileFromCurrentDirectory(null));
        }

        [Test]
        public void Create_From_Rooted_Path()
        {
            var path = _fileSystem.FileFromCurrentDirectory(@"c:\path\to\file.txt");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\path\to\file.txt"));
        }

        [Test]
        public void Create_From_Empty_String_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.FileFromCurrentDirectory(string.Empty));
        }

        [Test]
        public void Create_From_Parent_String_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.FileFromCurrentDirectory(".."));
        }

        [Test]
        public void Create_From_Relative_File()
        {
            var path = _fileSystem.FileFromCurrentDirectory(@"has\a\file.txt");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current\directory\has\a\file.txt"));
        }

        [Test]
        public void Create_From_Relative_File_In_Parent_Directory()
        {
            var path = _fileSystem.FileFromCurrentDirectory(@"..\file.txt");
            Assert.That(path.AsCanonical().PathAsString, Is.EqualTo(@"c:\current\file.txt"));
        }

        [Test]
        public void Create_From_File_Above_Root_Throws()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.FileFromCurrentDirectory(@"..\..\..\path\to\file.txt"));
        }
    }
}