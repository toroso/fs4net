using System;
using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class CreateRelativeDirectoryFixture
    {
        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => RelativeDirectory.FromString(null));
        }

        [Test]
        public void Throws_If_Path_Is_Empty()
        {
            AssertThrows<InvalidPathException>(() => RelativeDirectory.FromString(string.Empty));
        }


        private static readonly string[] ContainsInvalidPathCharacters =
            {
                @"folder\end\with\space \to",
                @"folder\contains\a*star\to",
                @"folder\contains\a?questionmark\to",
                @"folder\contains\a/slash\to",
                @"folder\contains\a:colon\to",
                "folder\\contains\\a\"doublequote\\to",
                @"folder\contains\a<lessthan\to",
                @"folder\contains\a>greaterthan\to",
                @"folder\contains\a|pipe\to",
                @"path\with\double\\backslashes\to",
                @"path\with\space\ \folder\name\to",
                @"path\with\space\three\...\dots\as\folder\name\to",
            };

        [Test]
        public void Throws_If_Path_Contains_Invalid_Character()
        {
            ContainsInvalidPathCharacters.ForEach(Throws_If_Path_Contains_Invalid_Character);
        }

        [Test, TestCaseSource("ContainsInvalidPathCharacters")]
        public void Throws_If_Path_Contains_Invalid_Character(string containsInvalidPathCharacters)
        {
            AssertThrowsInvalidPathExceptionFor(containsInvalidPathCharacters);
        }


        private static readonly string[] RootedPaths =
            {
                @"c:\standard\relative\path\to",
                @"z:\last\drive\path\to",
                @"\\network\drive\path\to",
                @"ö:\non\a\to\z\drive",
                @"\\colon:in\network\name",
                @"\\colon\in:share\name",
            };

        [Test]
        public void Throws_If_Path_Is_Rooted()
        {
            RootedPaths.ForEach(Throws_If_Path_Is_Rooted);
        }

        [Test, TestCaseSource("RootedPaths")]
        public void Throws_If_Path_Is_Rooted(string rootedPath)
        {
            AssertThrowsRootedPathExceptionFor(rootedPath);
        }


        private static readonly string[] ValidPaths =
            {
                @".", // canonically empty
                @"path\..", // canonically empty
                @"path\..\", // canonically empty
                @"path\without\ending\backslash\to",
                @"path\with\ending\backslash\to\",
                @"\path\starting\with\backslash\to",
                @"..\path\starting\with\doubledots\to",
                @"path\with\..\doubledots\to",
                @"path\ending\with\doubledots\..",
                @".\path\starting\with\dot\to",
                @"path\with\.\dot\to",
                @"path\ending\with\dot\.",
                @"folder\starts\with\ space\to", // Can't create it from Windows Explorer, but programmatically is ok
            };

        [Test]
        public void Create_With_Valid_Path()
        {
            ValidPaths.ForEach(Create_With_Valid_Path);
        }

        [Test, TestCaseSource("ValidPaths")]
        public void Create_With_Valid_Path(string validPath)
        {
            Console.WriteLine("Doing " + validPath);
            RelativeDirectory.FromString(validPath);
        }


        private static void AssertThrowsRootedPathExceptionFor(string invalidPath)
        {
            AssertThrows<RootedPathException>(() => RelativeDirectory.FromString(invalidPath), invalidPath);
        }

        private static void AssertThrows<T>(Action action)
        {
            AssertThrows<T>(action, string.Empty);
        }

        private static void AssertThrows<T>(Action action, string testData)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(T))
                {
                    string forString = (testData == string.Empty) ? string.Empty : string.Format(" for '{0}'", testData);
                    Console.WriteLine("Expected '{0}'{1} but got: {2}", typeof(T), forString, ex.GetType());
                    throw;
                }
            }
        }

        private static void AssertThrowsInvalidPathExceptionFor(string invalidPath)
        {
            Assert.Throws<InvalidPathException>(() => RelativeDirectory.FromString(invalidPath), string.Format("for '{0}'", invalidPath));
        }
    }
}