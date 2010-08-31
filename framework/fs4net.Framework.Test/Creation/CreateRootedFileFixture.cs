using System;
using System.IO;
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
            Assert.Throws<ArgumentNullException>(() => new RootedFile(null, @"c:\path\to\file.txt", PathWashers.NullWasher));
        }

        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _fileSystem.CreateFileDescribing(null));
        }

        [Test]
        public void Throws_If_Path_Is_Empty()
        {
            Assert.Throws<NonRootedPathException>(() => _fileSystem.CreateFileDescribing(string.Empty));
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
                @"ö:\non\a\to\z\drive.txt",
                @"\\colon:in\network\name.txt",
                @"\\colon\in:share\name.txt",
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

        [Test]
        public void Throws_If_FileName_Contains_Invalid_Character()
        {
            ContainsInvalidFilenameCharacters.ForEach(Throws_If_FileName_Contains_Invalid_Character);
        }

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
            _fileSystem.CreateFileDescribing(almostTooLongPath); // 259 chars in total
        }

        [Test]
        public void Throws_If_Path_Is_Too_Long()
        {
            string almostTooLongPath = @"c:\"; // 3 chars
            const string pathWith10Chars = @"123456789\";
            for (int index = 0; index < 25; index++)
            {
                almostTooLongPath += pathWith10Chars; // 10 * 25 chars
            }
            almostTooLongPath += @"1234567"; // 7 chars
            Assert.Throws<PathTooLongException>(() => _fileSystem.CreateFileDescribing(almostTooLongPath)); // 260 chars in total
        }

        [Test]
        public void Throws_If_Path_Accends_Above_Root()
        {
            Assert.Throws<InvalidPathException>(() => _fileSystem.CreateDirectoryDescribing(@"c:\..\path\to\file.txt"));
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

        [Test]
        public void Create_With_Valid_Path()
        {
            ValidPaths.ForEach(Create_With_Valid_Path);
        }

        [Test, TestCaseSource("ValidPaths")]
        public void Create_With_Valid_Path(string validPath)
        {
            _fileSystem.CreateFileDescribing(validPath);
        }


        private void AssertThrowsInvalidPathExceptionFor(string rootedPath)
        {
            Assert.Throws<InvalidPathException>(() => _fileSystem.CreateFileDescribing(rootedPath), string.Format("for '{0}'", rootedPath));
        }

        private void AssertThrowsNonRootedPathExceptionFor(string relativePath)
        {
            Assert.Throws<NonRootedPathException>(() => _fileSystem.CreateFileDescribing(relativePath), string.Format("for '{0}'", relativePath));
        }
    }
}