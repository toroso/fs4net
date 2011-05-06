using System;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class IsValidRootedFileFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        [Test]
        public void Throws_If_Path_Is_Null()
        {
            AssertThrows<ArgumentNullException>(() => _fileSystem.IsValidRootedFile(null));
        }

        private static readonly string[] NonValidOrNonRootedFile =
            {
                @"",
                @".",
                @"path\..",
                @"path\..\",
                @" c:\drive\starts\with\space",
                @"c :\drive\contains\space",
                @"c: \drive\ends\with\space",
                @"ö:\non\a\to\z\drive.txt",
                @"\\colon:in\network\name.txt",
                @"\\colon\in:share\name.txt",
                @"c:\folder\end\with\space \to\file.txt",
                @"c:\folder\contains\a*star\to\file.txt",
                @"c:\folder\contains\a?questionmark\to\file.txt",
                @"c:\folder\contains\a/slash\to\file.txt",
                @"c:\folder\contains\a:colon\to\file.txt",
                "c:\\folder\\contains\\a\"doublequote\\to\\file.txt",
                @"c:\folder\contains\a<lessthan\to\file.txt",
                @"c:\folder\contains\a>greaterthan\to\file.txt",
                @"c:\folder\contains\a|pipe\to\file.txt",
                @"c:\path\with\double\\backslashes\to\file.txt",
                @"c:\path\with\space\ \folder\name\to\file.txt",
                @"c:\path\with\space\three\...\dots\as\folder\name\to\file.txt",
                @"c:\filename\ends\with\space ",
                @"c:\filename\is\a\space\ ",
                @"c:\filename\is\empty\",
                @"c:\filename\is\a\dot\.",
                @"c:\filename\is\a\double\dot\..",
                @"c:\filename\is\three\dots\...",
                @"c:\filename\ends\with\a\dot.",
                @"c:\filename\contains\star\fi*le.txt",
                @"c:\filename\contains\questionmark\fi?le.txt",
                @"c:\filename\contains\slash\fi/le.txt",
                @"c:\filename\contains\colon\fi:le.txt",
                "c:\\filename\\contains\\doublequote\\fi\"le.txt",
                @"c:\filename\contains\lessthan\fi<le.txt",
                @"c:\filename\contains\greaterthan\fi>le.txt",
                @"c:\filename\contains\pipe\fi|le.txt",
                @"standard\relative\path\to\file.txt",
                @".\single\dot\path\to\file.txt",
                @"..\double\dot\path\to\file.txt",
                @"\missing\drive\path\to\file.txt",
            };

        [Test]
        public void Non_Valid_Or_Non_Rooted_File_Return_False()
        {
            NonValidOrNonRootedFile.ForEach(Non_Valid_Or_Non_Rooted_File_Return_False);
        }

        [Test, TestCaseSource("NonValidOrNonRootedFile")]
        public void Non_Valid_Or_Non_Rooted_File_Return_False(string path)
        {
            Assert.That(_fileSystem.IsValidRootedFile(path), Is.False);
        }


        private static readonly string[] ValidRootedFile =
            {
                @"c:\file.txt",
                @"c:\standard\path\to\file.txt",
                @"z:\last\drive\path\to\file.txt",
                @"c:\filename\with\empty\extension",
                @"\\network\path\to\file.txt",
                @"c:\path\with\..\doubledots\to\file.txt",
                @"c:\path\with\.\dot\to\file.txt",
                @"c:\folder\starts\with\ space\to\file.txt", // Can't create it from Windows Explorer, but programmatically is ok
            };

        [Test]
        public void Valid_Rooted_File_Return_True()
        {
            ValidRootedFile.ForEach(Valid_Rooted_File_Return_True);
        }

        [Test, TestCaseSource("ValidRootedFile")]
        public void Valid_Rooted_File_Return_True(string path)
        {
            Assert.That(_fileSystem.IsValidRootedFile(path), Is.True);
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
            Assert.That(_fileSystem.IsValidRootedFile(almostTooLongPath), Is.True); // 259 chars in total
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
            Assert.That(_fileSystem.IsValidRootedFile(tooLongPath), Is.False); // 260 chars in total
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