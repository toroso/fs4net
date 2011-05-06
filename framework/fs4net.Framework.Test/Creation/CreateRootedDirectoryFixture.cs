using System;
using System.IO;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateRootedDirectoryFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        [Test]
        public void Throws_If_FileSystem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new RootedDirectory(null, @"c:\path\to\directory", AssertLogger.Instance));
        }

        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _fileSystem.DirectoryDescribing(null));
        }

        [Test]
        public void Throws_If_Path_Is_Empty()
        {
            AssertThrows<NonRootedPathException>(() => _fileSystem.DirectoryDescribing(string.Empty));
        }


        private static readonly string[] EmptyCanonicalPaths =
            {
                @".",
                @"path\..",
                @"path\..\",
            };

        [Test]
        public void Throws_If_Canonical_Path_Is_Empty()
        {
            EmptyCanonicalPaths.ForEach(Throws_If_Canonical_Path_Is_Empty);
        }

        [Test, TestCaseSource("EmptyCanonicalPaths")]
        public void Throws_If_Canonical_Path_Is_Empty(string emptyPath)
        {
            AssertThrowsNonRootedPathExceptionFor(emptyPath);
        }


        private static readonly string[] ContainsInvalidDriveCharacters =
            {
                @" c:\drive\starts\with\space",
                @"c :\drive\contains\space",
                @"c: \drive\ends\with\space",
                @"ö:\non\a\to\z\drive",
                @"\\colon:in\network\name",
                @"\\colon\in:share\name",
                @"\\network",
                @"\\network\",
                @"c:relative\path\to",
            };

        [Test]
        public void Throws_If_Drive_Contains_Invalid_Character()
        {
            ContainsInvalidDriveCharacters.ForEach(Throws_If_Drive_Contains_Invalid_Character);
        }

        [Test, TestCaseSource("ContainsInvalidDriveCharacters")]
        public void Throws_If_Drive_Contains_Invalid_Character(string startsWithInvalidCharacter)
        {
            AssertThrowsInvalidPathExceptionFor(startsWithInvalidCharacter);
        }


        private static readonly string[] ContainsInvalidPathCharacters =
            {
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


        private static readonly string[] RelativePaths =
            {
                @"standard\relative\path\to",
                @".\single\dot\path\to",
                @"..\double\dot\path\to",
                @"\missing\drive\path\to",
            };

        [Test]
        public void Throws_If_Path_Is_Relative()
        {
            RelativePaths.ForEach(Throws_If_Path_Is_Relative);
        }

        [Test, TestCaseSource("RelativePaths")]
        public void Throws_If_Path_Is_Relative(string relativePath)
        {
            AssertThrowsNonRootedPathExceptionFor(relativePath);
        }


        [Test]
        public void Create_With_Almost_Too_Long_Path()
        {
            string almostTooLongPath = @"c:\"; // 3 chars
            const string pathWith10Chars = @"123456789\";
            for (int index = 0; index < 25; index++)
            {
                almostTooLongPath += pathWith10Chars; // 10 * 25 chars
            }
            almostTooLongPath += @"123456"; // 6 chars
            _fileSystem.DirectoryDescribing(almostTooLongPath); // 259 chars in total
        }

        [Test]
        public void Throws_If_Path_Is_Too_Long()
        {
            string tooLongPath = @"c:\"; // 3 chars
            const string pathWith10Chars = @"123456789\";
            for (int index = 0; index < 25; index++)
            {
                tooLongPath += pathWith10Chars; // 10 * 25 chars
            }
            tooLongPath += @"1234567"; // 7 chars
            Assert.Throws<PathTooLongException>(() => _fileSystem.DirectoryDescribing(tooLongPath)); // 260 chars in total
        }

        [Test]
        public void Throws_If_Path_Accends_Above_Root()
        {
            Assert.Throws<InvalidPathException>(() => _fileSystem.DirectoryDescribing(@"c:\path\..\..\to"));
        }

        private static readonly string[] ValidPaths =
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

        [Test]
        public void Create_With_Valid_Path()
        {
            ValidPaths.ForEach(Create_With_Valid_Path);
        }

        [Test, TestCaseSource("ValidPaths")]
        public void Create_With_Valid_Path(string validPath)
        {
            Console.WriteLine("Doing " + validPath);
            _fileSystem.DirectoryDescribing(validPath);
        }


        private void AssertThrowsInvalidPathExceptionFor(string rootedPath)
        {
            AssertThrows<InvalidPathException>(() => _fileSystem.DirectoryDescribing(rootedPath), string.Format("for '{0}'", rootedPath));
        }

        private void AssertThrowsNonRootedPathExceptionFor(string relativePath)
        {
            AssertThrows<NonRootedPathException>(() => _fileSystem.DirectoryDescribing(relativePath), string.Format("for '{0}'", relativePath));
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