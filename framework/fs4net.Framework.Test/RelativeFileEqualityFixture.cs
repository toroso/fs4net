using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class RelativeFileEqualityFixture
    {
        [Test]
        public void Equal_Files_Are_Equal()
        {
            var fileA = RelativeFile.FromString(@"path\to\file.txt");
            var fileB = RelativeFile.FromString(@"path\to\file.txt");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Canonically_Equal_Files_Are_Equal()
        {
            var fileA = RelativeFile.FromString(@"my\..\path\to\file.txt");
            var fileB = RelativeFile.FromString(@"path\.\from\..\to\file.txt");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Different_Files_Are_Not_Equal()
        {
            var fileA = RelativeFile.FromString(@"my\path\to\file.txt");
            var fileB = RelativeFile.FromString(@"another\path\to\file.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }

        [Test]
        public void RelativeFile_FileName_And_FileName_Are_Equal()
        {
            var file = RelativeFile.FromString("file.txt");
            var filename = FileName.FromString("file.txt");
            AssertEqualityEquals(file, filename);
        }


        private static void AssertEqualityEquals(RelativeFile lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(RelativeFile lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(RelativeFile lhs, RelativeFile rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(RelativeFile lhs, RelativeFile rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}