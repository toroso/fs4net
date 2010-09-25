using NUnit.Framework;

namespace fs4net.Framework.Test.PathAsString
{
    [TestFixture]
    public class RootedCanonicalPathAsStringFixture
    {
        private IFileSystem FileSystem { get; set; }


        [SetUp]
        public void CreateMockFileSystem()
        {
            FileSystem = new MockFileSystem();
        }


        private static readonly string[][] OriginalAndExpected =
            {
                new[] { @"c:\file_in_root.txt", @"c:\file_in_root.txt" },
                new[] { @"c:\standard\case\to\fileOrDirectory.txt", @"c:\standard\case\to\fileOrDirectory.txt" },
                new[] { @"c:\single\.\dots\to\.\fileOrDirectory.txt", @"c:\single\dots\to\fileOrDirectory.txt" },
                new[] { @"c:\double\..\dots\to\..\fileOrDirectory.txt", @"c:\dots\fileOrDirectory.txt" },
            };


        [Test]
        public void RootedFile_Canonical_PathAsString()
        {
            OriginalAndExpected.ForEach(RootedFile_Canonical_PathAsString);
        }

        public void RootedFile_Canonical_PathAsString(string[] testData)
        {
            RootedFile_Canonical_PathAsString(testData[0], testData[1]);
        }

        [Test, TestCaseSource("OriginalAndExpected")]
        public void RootedFile_Canonical_PathAsString(string original, string expected)
        {
            AssertCanonicalEquals(FileSystem.CreateFileDescribing(original), expected);
        }

        [Test]
        public void RootedDirectory_Canonical_PathAsString()
        {
            OriginalAndExpected.ForEach(RootedDirectory_Canonical_PathAsString);
        }

        public void RootedDirectory_Canonical_PathAsString(string[] testData)
        {
            RootedDirectory_Canonical_PathAsString(testData[0], testData[1]);
        }

        [Test, TestCaseSource("OriginalAndExpected")]
        public void RootedDirectory_Canonical_PathAsString(string original, string expected)
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(original), expected);
        }


        [Test]
        public void RelativeDirectory_Not_Ending_With_Backslash_Is_Intact_In_Canonical_Form()
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(@"c:\path\to"), @"c:\path\to");
        }

        [Test]
        public void RootedDirectory_Ending_Backslash_Removed_In_Canonical_Form()
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(@"c:\path\to\"), @"c:\path\to");
        }

        [Test]
        public void Drive_As_RootedDirectory_Ending_Backslash_Removed_In_Canonical_Form()
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(@"c:\"), @"c:");
        }

        [Test]
        public void RootedDirectory_Ending_Dot_Removed_In_Canonical_Form()
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(@"c:\path\to\."), @"c:\path\to");
        }

        [Test]
        public void RootedDirectory_Ending_DoubleDots_Removed_In_Canonical_Form()
        {
            AssertCanonicalEquals(FileSystem.CreateDirectoryDescribing(@"c:\path\to\.."), @"c:\path");
        }


        private static void AssertCanonicalEquals<T>(IFileSystemItem<T> file, string expected) where T : IFileSystemItem<T>
        {
            Assert.That(file.AsCanonical().PathAsString, Is.EqualTo(expected));
        }
    }
}