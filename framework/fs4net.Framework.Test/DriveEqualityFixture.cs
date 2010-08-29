using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class DriveEqualityFixture
    {
        private IFileSystem FileSystem { get; set; }


        [SetUp]
        public void SetUp()
        {
            FileSystem = new MockFileSystem();
        }


        [Test]
        public void Equal_Mapped_Drives_Are_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing("c:");
            var driveB = FileSystem.CreateDriveDescribing("c:");
            AssertEqualityEquals(driveA, driveB);
            AssertOperatorEquals(driveA, driveB);
        }

        [Test]
        public void Different_Mapped_Drives_Are_Not_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing("c:");
            var driveB = FileSystem.CreateDriveDescribing("d:");
            AssertEqualityNotEquals(driveA, driveB);
            AssertOperatorNotEquals(driveA, driveB);
        }

        [Test]
        public void Equal_Network_Drives_Are_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing(@"\\network\drive");
            var driveB = FileSystem.CreateDriveDescribing(@"\\network\drive");
            AssertEqualityEquals(driveA, driveB);
            AssertOperatorEquals(driveA, driveB);
        }

        [Test]
        public void Different_Network_Host_Names_In_Drives_Are_Not_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing(@"\\network\drive");
            var driveB = FileSystem.CreateDriveDescribing(@"\\worknet\drive");
            AssertEqualityNotEquals(driveA, driveB);
            AssertOperatorNotEquals(driveA, driveB);
        }

        [Test]
        public void Different_Network_Share_Names_In_Drives_Are_Not_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing(@"\\network\drive");
            var driveB = FileSystem.CreateDriveDescribing(@"\\network\share");
            AssertEqualityNotEquals(driveA, driveB);
            AssertOperatorNotEquals(driveA, driveB);
        }

        [Test]
        public void Drive_And_RootedDirectory_Drive_Are_Equal()
        {
            var drive = FileSystem.CreateDriveDescribing("c:");
            var driveAsDirectoty = FileSystem.CreateDirectoryDescribing("c:");
            AssertEqualityEquals(drive, driveAsDirectoty);
        }

        [Test]
        public void Drives_On_Different_FileSystems_Are_Not_Equal()
        {
            var driveA = FileSystem.CreateDriveDescribing("c:");
            var driveB = new MockFileSystem().CreateDriveDescribing("c:");
            AssertEqualityNotEquals(driveA, driveB);
            AssertOperatorNotEquals(driveA, driveB);
        }


        private static void AssertEqualityEquals(Drive lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(Drive lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(Drive lhs, Drive rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(Drive lhs, Drive rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}