using NUnit.Framework;

namespace fs4net.Framework.Test.Equality
{
    [TestFixture]
    public class FileNameEqualityFixture
    {
        [Test]
        public void Equal_Names_Are_Equal()
        {
            var fileA = FileName.FromString(@"file.txt");
            var fileB = FileName.FromString(@"file.txt");
            AssertEqualityEquals(fileA, fileB);
            AssertOperatorEquals(fileA, fileB);
        }

        [Test]
        public void Different_Extensions_Are_Not_Equal()
        {
            var fileA = FileName.FromString(@"file.dat");
            var fileB = FileName.FromString(@"file.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }

        [Test]
        public void Different_Names_Are_Not_Equal()
        {
            var fileA = FileName.FromString(@"file.txt");
            var fileB = FileName.FromString(@"saw.txt");
            AssertEqualityNotEquals(fileA, fileB);
            AssertOperatorNotEquals(fileA, fileB);
        }

        [Test]
        public void FileName_And_RelativeFile_FileName_Are_Equal()
        {
            var file = RelativeFile.FromString("file.txt");
            var filename = FileName.FromString("file.txt");
            AssertEqualityEquals(filename, file);
        }


        private static void AssertEqualityEquals(FileName lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(FileName lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(FileName lhs, FileName rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(FileName lhs, FileName rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}