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

        [Test]
        public void Comparison_Is_Case_Insensitive()
        {
            var fileA = FileName.FromString(@"FiLe.tXt");
            var fileB = FileName.FromString(@"fIle.Txt");
            AssertEqualityEquals(fileA, fileB);
            AssertEqualityEquals(fileA, fileB);
        }


        private static void AssertEqualityEquals(FileName left, object right)
        {
            Assert.That(left.Equals(right), Is.True, string.Format("'{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", left, right));
        }

        private static void AssertEqualityNotEquals(FileName left, object right)
        {
            Assert.That(left.Equals(right), Is.False, string.Format("! '{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.Not.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", left, right));
        }

        private static void AssertOperatorEquals(FileName left, FileName right)
        {
            Assert.That(left == right, Is.True, string.Format("'{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.False, string.Format("'{0}' != '{1}'", left, right));
        }

        private static void AssertOperatorNotEquals(FileName left, FileName right)
        {
            Assert.That(left == right, Is.False, string.Format("! '{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.True, string.Format("! '{0}' != '{1}'", left, right));
        }
    }
}