using NUnit.Framework;

namespace fs4net.Framework.Test
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
            var dirA = RelativeDirectory.FromString(@"my\..\path\to\");
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


        private static void AssertEqualityEquals(RelativeDirectory lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.True, string.Format("'{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() == '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertEqualityNotEquals(RelativeDirectory lhs, object rhs)
        {
            Assert.That(lhs.Equals(rhs), Is.False, string.Format("! '{0}'.Equals('{1}')", lhs, rhs));
            Assert.That(lhs.GetHashCode(), Is.Not.EqualTo(rhs.GetHashCode()), string.Format("'{0}'.GetHashCode() != '{1}'.GetHashCode()", lhs, rhs));
        }

        private static void AssertOperatorEquals(RelativeDirectory lhs, RelativeDirectory rhs)
        {
            Assert.That(lhs == rhs, Is.True, string.Format("'{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.False, string.Format("'{0}' != '{1}'", lhs, rhs));
        }

        private static void AssertOperatorNotEquals(RelativeDirectory lhs, RelativeDirectory rhs)
        {
            Assert.That(lhs == rhs, Is.False, string.Format("! '{0}' == '{1}'", lhs, rhs));
            Assert.That(lhs != rhs, Is.True, string.Format("! '{0}' != '{1}'", lhs, rhs));
        }
    }
}