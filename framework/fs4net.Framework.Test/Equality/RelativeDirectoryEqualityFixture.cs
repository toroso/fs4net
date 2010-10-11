using NUnit.Framework;

namespace fs4net.Framework.Test.Equality
{
    [TestFixture]
    public class RelativeDirectoryEqualityFixture
    {
        [Test]
        public void Equal_Directories_Are_Equal()
        {
            var dirA = RelativeDirectory.FromString(@"path\to");
            var dirB = RelativeDirectory.FromString(@"path\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Canonically_Equal_Directories_Are_Equal()
        {
            var dirA = RelativeDirectory.FromString(@"my\..\path\to");
            var dirB = RelativeDirectory.FromString(@"path\.\from\..\to");
            AssertEqualityEquals(dirA, dirB);
            AssertOperatorEquals(dirA, dirB);
        }

        [Test]
        public void Different_Directories_Are_Not_Equal()
        {
            var dirA = RelativeDirectory.FromString(@"my\path\to");
            var dirB = RelativeDirectory.FromString(@"another\path\to");
            AssertEqualityNotEquals(dirA, dirB);
            AssertOperatorNotEquals(dirA, dirB);
        }


        private static void AssertEqualityEquals(RelativeDirectory left, object right)
        {
            Assert.That(left.Equals(right), Is.True, string.Format("'{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", left, right));
        }

        private static void AssertEqualityNotEquals(RelativeDirectory left, object right)
        {
            Assert.That(left.Equals(right), Is.False, string.Format("! '{0}'.Equals('{1}')", left, right));
            Assert.That(left.GetHashCode(), Is.Not.EqualTo(right.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", left, right));
        }

        private static void AssertOperatorEquals(RelativeDirectory left, RelativeDirectory right)
        {
            Assert.That(left == right, Is.True, string.Format("'{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.False, string.Format("'{0}' != '{1}'", left, right));
        }

        private static void AssertOperatorNotEquals(RelativeDirectory left, RelativeDirectory right)
        {
            Assert.That(left == right, Is.False, string.Format("! '{0}' == '{1}'", left, right));
            Assert.That(left != right, Is.True, string.Format("! '{0}' != '{1}'", left, right));
        }
    }
}