using System;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateRelativeFileFixture
    {
        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => RelativeFile.FromString(null));
        }

        [Test]
        public void Throws_If_Path_Is_Empty()
        {
            Should.Throw<InvalidPathException>(() => RelativeFile.FromString(string.Empty));
        }


        private static readonly string[] ContainsInvalidPathCharacters =
            {
                @"folder\end\with\space \to\file.txt",
                @"folder\contains\a*star\to\file.txt",
                @"folder\contains\a?questionmark\to\file.txt",
                @"folder\contains\a/slash\to\file.txt",
                @"folder\contains\a:colon\to\file.txt",
                "folder\\contains\\a\"doublequote\\to\\file.txt",
                @"folder\contains\a<lessthan\to\file.txt",
                @"folder\contains\a>greaterthan\to\file.txt",
                @"folder\contains\a|pipe\to\file.txt",
                @"path\with\double\\backslashes\to\file.txt",
                @"path\with\space\ \folder\name\to\file.txt",
                @"path\with\space\three\...\dots\as\folder\name\to\file.txt",
                @"\path\starting\with\backslash\to\file.txt",
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
                @".", // canonically empty
                @"path\..", // canonically empty
                @"path\..\", // canonically empty
                @"filename\ends\with\space ",
                @"filename\is\a\space\ ",
                @"filename\is\empty\",
                @"filename\is\a\dot\.",
                @"filename\is\a\double\dot\..",
                @"filename\is\three\dots\...",
                @"filename\ends\with\a\dot.",
                @"filename\contains\star\fi*le.txt",
                @"filename\contains\questionmark\fi?le.txt",
                @"filename\contains\slash\fi/le.txt",
                @"filename\contains\colon\fi:le.txt",
                "filename\\contains\\doublequote\\fi\"le.txt",
                @"filename\contains\lessthan\fi<le.txt",
                @"filename\contains\greaterthan\fi>le.txt",
                @"filename\contains\pipe\fi|le.txt",
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


        private static readonly string[] RootedPaths =
            {
                @"c:\standard\relative\path\to\file.txt",
                @"z:\last\drive\path\to\file.txt",
                @"\\network\drive\path\to\file.txt",
                @"ö:\non\a\to\z\drive\to\file.txt",
                @"\\colon:in\network\name\to\file.txt",
                @"\\colon\in:share\name\to\file.txt",
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
                @"path\not\starting\with\backslash\to\file.txt",
                @"filename\with\empty\extension",
                @"..\path\starting\with\doubledots\to\file.txt",
                @"path\with\..\doubledots\to\file.txt",
                @"path\ending\with\doubledots\..\file.txt",
                @".\path\starting\with\dot\to\file.txt",
                @"path\with\.\dot\to\file.txt",
                @"path\ending\with\dot\.\file.txt",
                @"folder\starts\with\ space\to\file.txt", // Can't create it from Windows Explorer, but programmatically is ok
            };

        [Test]
        public void Create_With_Valid_Path()
        {
            ValidPaths.ForEach(Create_With_Valid_Path);
        }

        [Test, TestCaseSource("ValidPaths")]
        public void Create_With_Valid_Path(string validPath)
        {
            RelativeDirectory.FromString(validPath);
        }


        private static void AssertThrowsRootedPathExceptionFor(string invalidPath)
        {
            Should.Throw<RootedPathException>(() => RelativeFile.FromString(invalidPath), string.Format("for '{0}'", invalidPath));
        }

        private static void AssertThrowsInvalidPathExceptionFor(string invalidPath)
        {
            Should.Throw<InvalidPathException>(() => RelativeFile.FromString(invalidPath), string.Format("for '{0}'", invalidPath));
        }
    }
}