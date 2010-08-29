using NUnit.Framework;

namespace fs4net.Framework.Test
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
            var dirA = FileSystem.CreateDirectoryDescribing(@"c:\path\to");
            var dirB = FileSystem.CreateDirectoryDescribing(@"c:\path\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Canonically_Equal_Directories_Are_Equal()
        {
            var dirA = FileSystem.CreateDirectoryDescribing(@"c:\my\..\path\to\");
            var dirB = FileSystem.CreateDirectoryDescribing(@"c:\path\.\from\..\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Equal_Network_Directories_Are_Equal()
        {
            var dirA = FileSystem.CreateDirectoryDescribing(@"\\network\share\down\in");
            var dirB = FileSystem.CreateDirectoryDescribing(@"\\network\share\down\in\");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Different_Directories_Are_Not_Equal()
        {
            var dirA = FileSystem.CreateDirectoryDescribing(@"c:\my\path\to");
            var dirB = FileSystem.CreateDirectoryDescribing(@"c:\another\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }

        [Test]
        public void Directories_On_Different_Drives_Are_Not_Equal()
        {
            var dirA = FileSystem.CreateDirectoryDescribing(@"c:\my\path\to");
            var dirB = FileSystem.CreateDirectoryDescribing(@"d:\my\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }

        [Test]
        public void RootedDirectory_Drive_And_Drive_Are_Equal()
        {
            var driveAsDirectoty = FileSystem.CreateDirectoryDescribing(@"c:\");
            var drive = FileSystem.CreateDriveDescribing("c:");
            AssertEqualityEquals(driveAsDirectoty, drive);
        }

        [Test]
        public void Directories_On_Different_FileSystems_Are_Not_Equal()
        {
            var dirA = FileSystem.CreateDirectoryDescribing(@"c:\path\to");
            var dirB = new MockFileSystem().CreateDirectoryDescribing(@"c:\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }


        private static void AssertEqualityEquals(RootedDirectory lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(RootedDirectory lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(RootedDirectory lhs, RootedDirectory rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(RootedDirectory lhs, RootedDirectory rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}