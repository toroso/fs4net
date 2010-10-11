using NUnit.Framework;

namespace fs4net.Framework.Test.Equality
{
    [TestFixture]
    public class RootedDirectoryEqualityFixture
    {
        private IFileSystem FileSystem { get; set; }


        [SetUp]
        public void SetUp()
        {
            FileSystem = new MockFileSystem();
        }


        [Test]
        public void Equal_Directories_Are_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"c:\path\to");
            var dirB = FileSystem.DirectoryDescribing(@"c:\path\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Canonically_Equal_Directories_Are_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"c:\my\..\path\to");
            var dirB = FileSystem.DirectoryDescribing(@"c:\path\.\from\..\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Equal_Network_Directories_Are_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"\\network\share\down\in");
            var dirB = FileSystem.DirectoryDescribing(@"\\network\share\down\in");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Different_Directories_Are_Not_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"c:\my\path\to");
            var dirB = FileSystem.DirectoryDescribing(@"c:\another\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }

        [Test]
        public void Directories_On_Different_Drives_Are_Not_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"c:\my\path\to");
            var dirB = FileSystem.DirectoryDescribing(@"d:\my\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }

        [Test]
        public void RootedDirectory_Drive_And_Drive_Are_Equal()
        {
            var driveAsDirectoty = FileSystem.DirectoryDescribing(@"c:");
            var drive = FileSystem.DriveDescribing("c:");
            AssertEqualityEquals(driveAsDirectoty, drive);
        }

        [Test]
        public void Directories_On_Different_FileSystems_Are_Not_Equal()
        {
            var dirA = FileSystem.DirectoryDescribing(@"c:\path\to");
            var dirB = new MockFileSystem().DirectoryDescribing(@"c:\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }


        private static void AssertEqualityEquals(RootedDirectory left, object right)
        {
            Assert.That(left.Equals(right), Is.True, string.Format("'{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", left, right));
        }

        private static void AssertEqualityNotEquals(RootedDirectory left, object right)
        {
            Assert.That(left.Equals(right), Is.False, string.Format("! '{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.Not.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", left, right));
        }

        private static void AssertOperatorEquals(RootedDirectory left, RootedDirectory right)
        {
            Assert.That(left == right, Is.True, string.Format("'{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.False, string.Format("'{0}' != '{1}'", left, right));
        }

        private static void AssertOperatorNotEquals(RootedDirectory left, RootedDirectory right)
        {
            Assert.That(left == right, Is.False, string.Format("! '{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.True, string.Format("! '{0}' != '{1}'", left, right));
        }
    }
}