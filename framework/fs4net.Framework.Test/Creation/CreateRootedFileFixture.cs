using System;
using System.IO;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateRootedFileFixture
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
            Should.Throw<ArgumentNullException>(() => new RootedFile(null, @"c:\path\to\file.txt", AssertLogger.Instance));
        }

        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => _fileSystem.FileDescribing(null));
        }

        [Test]
        public void Throws_If_Path_Is_Empty()
        {
            Should.Throw<NonRootedPathException>(() => _fileSystem.FileDescribing(string.Empty));
        }


        private static readonly string[] EmptyCanonicalPaths =
            {
                @".",
                @"path\..",
                @"path\..\",
            };

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
                @"�:\non\a\to\z\drive.txt",
                @"\\colon:in\network\name.txt",
                @"\\colon\in:share\name.txt",
            };

        [Test, TestCaseSource("ContainsInvalidDriveCharacters")]
        public void Throws_If_Drive_Contains_Invalid_Character(string startsWithInvalidCharacter)
        {
            AssertThrowsInvalidPathExceptionFor(startsWithInvalidCharacter);
        }


        private static readonly string[] ContainsInvalidPathCharacters =
            {
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
            };

        [Test, TestCaseSource("ContainsInvalidPathCharacters")]
        public void Throws_If_Path_Contains_Invalid_Character(string containsInvalidPathCharacters)
        {
            AssertThrowsInvalidPathExceptionFor(containsInvalidPathCharacters);
        }


        private static readonly string[] ContainsInvalidFilenameCharacters =
            {
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
            };

        [Test, TestCaseSource("ContainsInvalidFilenameCharacters")]
        public void Throws_If_FileName_Contains_Invalid_Character(string containsInvalidFilenameCharacters)
        {
            AssertThrowsInvalidPathExceptionFor(containsInvalidFilenameCharacters);
        }


        private static readonly string[] RelativePaths =
            {
                @"standard\relative\path\to\file.txt",
                @".\single\dot\path\to\file.txt",
                @"..\double\dot\path\to\file.txt",
                @"\missing\drive\path\to\file.txt",
            };

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
            _fileSystem.FileDescribing(almostTooLongPath); // 259 chars in total
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
            Should.Throw<PathTooLongException>(() => _fileSystem.FileDescribing(tooLongPath)); // 260 chars in total
        }

        [Test]
        public void Throws_If_Path_Accends_Above_Root()
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.DirectoryDescribing(@"c:\..\path\to\file.txt"));
        }


        private static readonly string[] ValidPaths =
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

        [Test, TestCaseSource("ValidPaths")]
        public void Create_With_Valid_Path(string validPath)
        {
            _fileSystem.FileDescribing(validPath);
        }

        [Test]
        public void Create_By_Changing_FileName()
        {
            var original = _fileSystem.FileDescribing(@"c:\path\to\file.txt");
            var changed = original.WithFileName(FileName.FromString("other.zip"));
            Assert.That(changed.PathAsString, Is.EqualTo(@"c:\path\to\other.zip"));
        }

        private void AssertThrowsInvalidPathExceptionFor(string rootedPath)
        {
            Should.Throw<InvalidPathException>(() => _fileSystem.FileDescribing(rootedPath), string.Format("for '{0}'", rootedPath));
        }

        private void AssertThrowsNonRootedPathExceptionFor(string relativePath)
        {
            Should.Throw<NonRootedPathException>(() => _fileSystem.FileDescribing(relativePath), string.Format("for '{0}'", relativePath));
        }
    }
}