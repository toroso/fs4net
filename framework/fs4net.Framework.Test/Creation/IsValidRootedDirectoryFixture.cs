using System;
using fs4net.Framework.Utils;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class IsValidRootedDirectoryFixture
    {
        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => ValidityCheckers.IsValidRootedDirectory(null));
        }

        private static readonly string[] NonValidOrNonRootedDirectory =
            {
                @"",
                @" c:\drive\starts\with\space",
                @"c :\drive\contains\space",
                @"c: \drive\ends\with\space",
                @"ö:\non\a\to\z\drive",
                @"\\colon:in\network\name",
                @"\\colon\in:share\name",
                @"\\network",
                @"\\network\",
                @"c:relative\path\to",
                @".",
                @"path\..",
                @"path\..\",
                @"c:\",
                @"c:\path\with\ending\backslash\to\",
                @"\\network\path\",
                @"c:\folder\end\with\space \to",
                @"c:\folder\contains\a*star\to",
                @"c:\folder\contains\a?questionmark\to",
                @"c:\folder\contains\a/slash\to",
                @"c:\folder\contains\a:colon\to",
                "c:\\folder\\contains\\a\"doublequote\\to",
                @"c:\folder\contains\a<lessthan\to",
                @"c:\folder\contains\a>greaterthan\to",
                @"c:\folder\contains\a|pipe\to",
                @"c:\path\with\double\\backslashes\to",
                @"c:\path\with\space\ \folder\name\to",
                @"c:\path\with\space\three\...\dots\as\folder\name\to",
                @"standard\relative\path\to",
                @".\single\dot\path\to",
                @"..\double\dot\path\to",
                @"\missing\drive\path\to",
            };

        [Test, TestCaseSource("NonValidOrNonRootedDirectory")]
        public void Non_Valid_Or_Non_Rooted_Directory_Return_False(string path)
        {
            Assert.That(path.IsValidRootedDirectory(), Is.False);
        }


        private static readonly string[] ValidRootedDirectory =
            {
                @"c:",
                @"c:\path\..",
                @"c:\path\without\ending\backslash\to",
                @"z:\last\drive\path\to",
                @"\\network\path",
                @"\\network\path\to",
                @"c:\path\with\..\doubledots\to",
                @"c:\path\ending\with\doubledots\..",
                @"c:\path\with\.\dot\to",
                @"c:\path\ending\with\dot\.",
                @"c:\folder\starts\with\ space\to", // Can't create it from Windows Explorer, but programmatically is ok
            };

        [Test, TestCaseSource("ValidRootedDirectory")]
        public void Valid_Rooted_Directory_Return_True(string path)
        {
            Assert.That(path.IsValidRootedDirectory(), Is.True);
        }


        [Test]
        public void Is_Valid_When_Path_Is_Almost_Too_Long_Path()
        {
            string almostTooLongPath = @"c:\"; // 3 chars
            const string pathWith10Chars = @"123456789\";
            for (int index = 0; index < 25; index++)
            {
                almostTooLongPath += pathWith10Chars; // 10 * 25 chars
            }
            almostTooLongPath += @"123456"; // 6 chars
            Assert.That(almostTooLongPath.IsValidRootedDirectory(), Is.True); // 259 chars in total
        }

        [Test]
        public void Is_Invalid_When_Path_Is_Too_Long()
        {
            string tooLongPath = @"c:\"; // 3 chars
            const string pathWith10Chars = @"123456789\";
            for (int index = 0; index < 25; index++)
            {
                tooLongPath += pathWith10Chars; // 10 * 25 chars
            }
            tooLongPath += @"1234567"; // 7 chars
            Assert.That(tooLongPath.IsValidRootedDirectory(), Is.False); // 260 chars in total
        }
    }
}