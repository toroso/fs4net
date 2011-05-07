using System;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class IsValidRelativeDirectoryFixture
    {
        [Test]
        public void Throws_If_Path_Is_Null()
        {
            AssertThrows<ArgumentNullException>(() => ValidityCheckers.IsValidRelativeDirectory(null));
        }

        private static readonly string[] NonValidOrNonRelativeDirectory =
            {
                @"path\..\", // canonically empty, ends with backslash
                @"path\with\ending\backslash\to\",
                @"\path\starting\with\backslash\to",
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
                @"c:\standard\relative\path\to",
                @"z:\last\drive\path\to",
                @"\\network\drive\path\to",
                @"ö:\non\a\to\z\drive",
                @"\\colon:in\network\name",
                @"\\colon\in:share\name",
            };

        [Test]
        public void Non_Valid_Or_Non_Relative_Directory_Return_False()
        {
            NonValidOrNonRelativeDirectory.ForEach(Non_Valid_Or_Non_Relative_Directory_Return_False);
        }

        [Test, TestCaseSource("NonValidOrNonRelativeDirectory")]
        public void Non_Valid_Or_Non_Relative_Directory_Return_False(string path)
        {
            Assert.That(path.IsValidRelativeDirectory(), Is.False);
        }


        private static readonly string[] ValidRelativeDirectory =
            {
                @"",
                @".", // canonically empty
                @"path\..", // canonically empty
                @"path\without\ending\backslash\to",
                @"..\path\starting\with\doubledots\to",
                @"path\with\..\doubledots\to",
                @"path\ending\with\doubledots\..",
                @".\path\starting\with\dot\to",
                @"path\with\.\dot\to",
                @"path\ending\with\dot\.",
                @"folder\starts\with\ space\to", // Can't create it from Windows Explorer, but programmatically is ok
            };

        [Test]
        public void Valid_Relative_Directory_Return_True()
        {
            ValidRelativeDirectory.ForEach(Valid_Relative_Directory_Return_True);
        }

        [Test, TestCaseSource("ValidRelativeDirectory")]
        public void Valid_Relative_Directory_Return_True(string path)
        {
            Assert.That(path.IsValidRelativeDirectory(), Is.True);
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
                FailOnWrongException<T>(testData, "no exception");
            }
            catch (Exception ex)
            {
                if (ex.GetType() != typeof(T))
                {
                    FailOnWrongException<T>(testData, ex.GetType().ToString());
                }
            }
        }

        private static void FailOnWrongException<T>(string testData, string exception)
        {
            string forString = (testData == string.Empty) ? string.Empty : string.Format(" for '{0}'", testData);
            Assert.Fail(string.Format("Expected {0}{1} but got {2}.", typeof(T), forString, exception));
        }
    }
}