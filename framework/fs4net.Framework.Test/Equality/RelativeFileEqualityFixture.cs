using NUnit.Framework;

namespace fs4net.Framework.Test.Equality
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


        private static void AssertEqualityEquals(RelativeFile left, object right)
        {
            Assert.That(left.Equals(right), Is.True, string.Format("'{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", left, right));
        }

        private static void AssertEqualityNotEquals(RelativeFile left, object right)
        {
            Assert.That(left.Equals(right), Is.False, string.Format("! '{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.Not.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", left, right));
        }

        private static void AssertOperatorEquals(RelativeFile left, RelativeFile right)
        {
            Assert.That(left == right, Is.True, string.Format("'{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.False, string.Format("'{0}' != '{1}'", left, right));
        }

        private static void AssertOperatorNotEquals(RelativeFile left, RelativeFile right)
        {
            Assert.That(left == right, Is.False, string.Format("! '{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.True, string.Format("! '{0}' != '{1}'", left, right));
        }
    }
}