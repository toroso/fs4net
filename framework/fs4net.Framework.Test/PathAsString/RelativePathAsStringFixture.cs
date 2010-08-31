using NUnit.Framework;

namespace fs4net.Framework.Test.PathAsString
{
    [TestFixture]
    public class RelativePathAsStringFixture
    {
        private static readonly string[] RelativePaths =
            {
                @"standard\case\to\fileOrDirectory.txt",
                @"single\.\dots\to\.\fileOrDirectory.txt",
                @"double\..\dots\to\..\fileOrDirectory.txt",
                @"\starting\with\backslash\to\fileOrDirectory.txt",
            };


        [Test]
        public void RelativeFile_PathAsString_Is_Intact()
        {
            RelativePaths.ForEach(RelativeFile_PathAsString_Is_Intact);
        }

        [Test, TestCaseSource("RelativePaths")]
        public void RelativeFile_PathAsString_Is_Intact(string path)
        {
            Assert.That(RelativeFile.FromString(path).PathAsString, Is.EqualTo(path));
        }

        [Test]
        public void RelativeDirectory_PathAsString_Is_Intact()
        {
            RelativePaths.ForEach(RelativeDirectory_PathAsString_Is_Intact);
        }

        [Test, TestCaseSource("RelativePaths")]
        public void RelativeDirectory_PathAsString_Is_Intact(string path)
        {
            Assert.That(RelativeDirectory.FromString(path).PathAsString, Is.EqualTo(path));
        }


        [Test]
        public void RelativeDirectory_Ending_Backslash_Remains_In_PathAsString()
        {
            Assert.That(RelativeDirectory.FromString(@"path\to\").PathAsString, Is.EqualTo(@"path\to\"));
        }

        [Test]
        public void RelativeDirectory_Ending_Dot_Remains_In_PathAsString()
        {
            Assert.That(RelativeDirectory.FromString(@"path\to\.").PathAsString, Is.EqualTo(@"path\to\."));
        }

        [Test]
        public void RelativeDirectory_Ending_DoubleDots_Remains_In_PathAsString()
        {
            Assert.That(RelativeDirectory.FromString(@"path\to\..").PathAsString, Is.EqualTo(@"path\to\.."));
        }
    }
}