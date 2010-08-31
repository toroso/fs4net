using NUnit.Framework;

namespace fs4net.Framework.Test.Equality
{
    [TestFixture]
    public class RootedFileEqualityFixture
    {
        private IFileSystem FileSystem { get; set; }


        [SetUp]
        public void SetUp()
        {
            FileSystem = new MockFileSystem();
        }


        [Test]
        public void Equal_Files_Are_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"c:\path\to\file.txt");
            var fileB = FileSystem.CreateFileDescribing(@"c:\path\to\file.txt");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Canonically_Equal_Files_Are_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"c:\my\..\path\to\file.txt");
            var fileB = FileSystem.CreateFileDescribing(@"c:\path\.\from\..\to\file.txt");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Equal_Network_Files_Are_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"\\network\share\down\in\drain");
            var fileB = FileSystem.CreateFileDescribing(@"\\network\share\down\in\drain");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Different_Files_Are_Not_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"c:\my\path\to\file.txt");
            var fileB = FileSystem.CreateFileDescribing(@"c:\another\path\to\file.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }

        [Test]
        public void Files_On_Different_Drives_Are_Not_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"c:\my\path\to\file.txt");
            var fileB = FileSystem.CreateFileDescribing(@"d:\my\path\to\file.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }

        [Test]
        public void Files_On_Different_FileSystems_Are_Not_Equal()
        {
            var fileA = FileSystem.CreateFileDescribing(@"c:\path\to\file.txt");
            var fileB = new MockFileSystem().CreateFileDescribing(@"c:\path\to\file.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }


        private static void AssertEqualityEquals(RootedFile lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(RootedFile lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(RootedFile lhs, RootedFile rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(RootedFile lhs, RootedFile rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}