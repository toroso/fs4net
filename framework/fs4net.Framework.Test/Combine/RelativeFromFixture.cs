using System;
using NUnit.Framework;

namespace fs4net.Framework.Test.Combine
{
    [TestFixture]
    public class RelativeFromFixture
    {
        private static readonly IFileSystem FileSystem = new MockFileSystem();

        [Test]
        public void RootedDirectory_Relative_Itself_Results_In_An_Empty_Path()
        {
            var dir = FileSystem.DirectoryDescribing(@"c:\path\is");
            Assert.That(dir.RelativeFrom(dir).PathAsString, Is.EqualTo(string.Empty));
        }

        [Test]
        public void RootedDirectory_Relative_Other_RootedDirectory()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\is\deep\deep\down");
            var to = FileSystem.DirectoryDescribing(@"c:\path\is\different");
            var expected = RelativeDirectory.FromString(@"..\..\..\different");
            Assert.That(to.RelativeFrom(from).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void RootedDirectory_Relative_RootedDirectory_On_Other_Drive_Throws()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\on\one\drive");
            var to = FileSystem.DirectoryDescribing(@"d:\path\on\other\drive");
            Assert.Throws<ArgumentException>(() => to.RelativeFrom(from));
        }

        [Test]
        public void RootedDirectory_Relative_RootedDirectory_That_Ends_With_A_Backslash()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\is\here\");
            var to = FileSystem.DirectoryDescribing(@"c:\path\ends\with\backslash\");
            var expected = RelativeDirectory.FromString(@"..\..\ends\with\backslash\");
            Assert.That(to.RelativeFrom(from).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void NonCanonical_RootedDirectory_Relative_Other_NonCanonical_RootedDirectory()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\is\sleep\..\deep\deep\down");
            var to = FileSystem.DirectoryDescribing(@"c:\myth\..\path\is\same\..\different");
            var expected = RelativeDirectory.FromString(@"..\..\..\different"); // Could've been "..\..\..\same\..\different"
            Assert.That(to.RelativeFrom(from).PathAsString, Is.EqualTo(expected.PathAsString));
        }


        [Test]
        public void RootedFile_Relative_RootedDirectory()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\is\deep\deep\down");
            var to = FileSystem.FileDescribing(@"c:\path\is\different.txt");
            var expected = RelativeDirectory.FromString(@"..\..\..\different.txt");
            Assert.That(to.RelativeFrom(from).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void RootedFile_Relative_RootedDirectory_On_Other_Drive_Throws()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\on\one\drive");
            var to = FileSystem.FileDescribing(@"d:\path\on\other\drive.txt");
            Assert.Throws<ArgumentException>(() => to.RelativeFrom(from));
        }

        [Test]
        public void NonCanonical_RootedFile_Relative_NonCanonical_RootedDirectory()
        {
            var from = FileSystem.DirectoryDescribing(@"c:\path\is\sleep\..\deep\deep\down");
            var to = FileSystem.FileDescribing(@"c:\myth\..\path\is\same\..\different.txt");
            var expected = RelativeDirectory.FromString(@"..\..\..\different.txt"); // Could've been "..\..\..\same\..\different.txt"
            Assert.That(to.RelativeFrom(from).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        //RootedFile.RelativeFrom RootedDirectory => RelativeFile
    }
}