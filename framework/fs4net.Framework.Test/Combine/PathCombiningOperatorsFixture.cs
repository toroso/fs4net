using NUnit.Framework;

namespace fs4net.Framework.Test.Combine
{
    [TestFixture]
    public class PathCombiningOperatorsFixture
    {
        private static readonly IFileSystem FileSystem = new MockFileSystem();


        [Test]
        public void Combine_Drive_And_RelativeDirectory()
        {
            var left = FileSystem.DriveDescribing(@"c:");
            var right = RelativeDirectory.FromString(@"relative\to");
            var expected = FileSystem.DirectoryDescribing(@"c:\relative\to");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_RootedDirectory_And_RelativeDirectory()
        {
            var left = FileSystem.DirectoryDescribing(@"c:\path\is");
            var right = RelativeDirectory.FromString(@"relative\to");
            var expected = FileSystem.DirectoryDescribing(@"c:\path\is\relative\to");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_Drive_And_Filename_As_RelativeFile()
        {
            var left = FileSystem.DriveDescribing(@"c:");
            var right = RelativeFile.FromString(@"file.txt");
            var expected = FileSystem.FileDescribing(@"c:\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_Drive_And_RelativeFile_With_Path()
        {
            var left = FileSystem.DriveDescribing(@"c:");
            var right = RelativeFile.FromString(@"file.txt");
            var expected = FileSystem.FileDescribing(@"c:\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_RootedDirectory_And_Filename_As_RelativeFile()
        {
            var left = FileSystem.DirectoryDescribing(@"c:\path\is");
            var right = RelativeFile.FromString(@"file.txt");
            var expected = Is.EqualTo(FileSystem.FileDescribing(@"c:\path\is\file.txt").PathAsString);
            Assert.That((left + right).PathAsString, expected);
        }

        [Test]
        public void Combine_RootedDirectory_And_RelativeFile_With_Path()
        {
            var left = FileSystem.DirectoryDescribing(@"c:\path\is");
            var right = RelativeFile.FromString(@"my\file.txt");
            var expected = FileSystem.FileDescribing(@"c:\path\is\my\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_RelativeDirectory_And_Filename_As_RelativeFile()
        {
            var left = RelativeDirectory.FromString(@"relative\to");
            var right = RelativeFile.FromString(@"file.txt");
            var expected = RelativeFile.FromString(@"relative\to\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_RelativeDirectory_And_RelativeFile_With_Path()
        {
            var left = RelativeDirectory.FromString(@"relative\to");
            var right = RelativeFile.FromString(@"my\file.txt");
            var expected = RelativeFile.FromString(@"relative\to\my\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_Empty_RelativeDirectory_And_RelativeFile_With_Path()
        {
            var left = RelativeDirectory.FromString(string.Empty);
            var right = RelativeFile.FromString(@"my\file.txt");
            var expected = RelativeFile.FromString(@"my\file.txt");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_Empty_RelativeDirectory_And_RelativeDirectory_With_Path()
        {
            var left = RelativeDirectory.FromString(string.Empty);
            var right = RelativeDirectory.FromString(@"relative\to");
            var expected = RelativeDirectory.FromString(@"relative\to");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }

        [Test]
        public void Combine_RelativeDirectory_And_Empry_RelativeDirectory_With_Path()
        {
            var left = RelativeDirectory.FromString(@"relative\to");
            var right = RelativeDirectory.FromString(string.Empty);
            var expected = RelativeDirectory.FromString(@"relative\to");
            Assert.That((left + right).PathAsString, Is.EqualTo(expected.PathAsString));
        }
    }
}