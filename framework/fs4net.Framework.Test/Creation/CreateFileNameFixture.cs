using System;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class CreateFileNameFixture
    {
        [Test]
        public void Throws_If_Name_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => FileName.FromString(null));
        }

        [Test]
        public void Throws_If_Name_Is_Empty()
        {
            Assert.Throws<InvalidPathException>(() => FileName.FromString(string.Empty));
        }


        private static readonly string[] ContainsInvalidCharacters =
            {
                @"contains*star.txt",
                @"contains?questionmark.txt",
                @"contains/slash.txt",
                @"contains:colon.txt",
                "contains\"doublequote.txt",
                @"contains<lessthan.txt",
                @"contains>greaterthan.txt",
                @"contains|pipe.txt",
                @"contains\backslash.txt",
            };

        [Test]
        public void Throws_If_Contains_Invalid_Character()
        {
            ContainsInvalidCharacters.ForEach(Throws_If_Contains_Invalid_Character);
        }

        [Test, TestCaseSource("ContainsInvalidCharacters")]
        public void Throws_If_Contains_Invalid_Character(string containsInvalidFilenameCharacters)
        {
            AssertThrowsInvalidPathExceptionFor(containsInvalidFilenameCharacters);
        }


        private static readonly string[] InvalidNames =
            {
                @"endingspace ",
                @"endingspace.txt ",
                @" ",
                @".",
                @"..",
                @"...",
                @"endswithdot.",
                @"c:\rooted.txt",
            };

        [Test]
        public void Throws_If_Is_Invalid()
        {
            InvalidNames.ForEach(Throws_If_Is_Invalid);
        }

        [Test, TestCaseSource("InvalidNames")]
        public void Throws_If_Is_Invalid(string invalidName)
        {
            AssertThrowsInvalidPathExceptionFor(invalidName);
        }


        private static readonly string[] ValidNames =
            {
                @"normal.file",
                @"no_extension",
                @"double_extension.txt.tgz",
            };

        [Test]
        public void Create_With_Valid_Name()
        {
            ValidNames.ForEach(Create_With_Valid_Name);
        }

        [Test, TestCaseSource("ValidNames")]
        public void Create_With_Valid_Name(string validName)
        {
            Assert.DoesNotThrow(() => FileName.FromString(validName));
        }


        [Test]
        public void Create_From_Standard_RelativeFile()
        {
            Assert.DoesNotThrow(() => RelativeFile.FromString(@"path\to\file.txt").FileName());
        }

        [Test]
        public void Create_From_RelativeFile_That_Only_Contain_FileName()
        {
            Assert.DoesNotThrow(() => RelativeFile.FromString(@"file.txt").FileName());
        }

        [Test]
        public void Create_From_RelativeFile_That_Only_Contain_FileName_Without_Extension()
        {
            Assert.DoesNotThrow(() => RelativeFile.FromString(@"file").FileName());
        }


        [Test]
        public void Create_From_Standard_RootedFile()
        {
            Assert.DoesNotThrow(() => new MockFileSystem().CreateFileDescribing(@"c:\path\to\file.txt").FileName());
        }

        [Test]
        public void Create_From_RootedFile_With_FileName_Without_Extension()
        {
            Assert.DoesNotThrow(() => new MockFileSystem().CreateFileDescribing(@"c:\path\to\file").FileName());
        }

        [Test]
        public void Create_From_Name_And_Extension()
        {
            Assert.That(FileName.FromNameAndExtension("file", ".txt").FullName, Is.EqualTo("file.txt"));
        }

        [Test]
        public void Create_From_Name_And_Empty_Extension()
        {
            Assert.That(FileName.FromNameAndExtension("file", "").FullName, Is.EqualTo("file"));
        }

        [Test]
        public void Create_From_Name_And_Extension_Without_Dot_Throws()
        {
            Assert.Throws<ArgumentException>(() => FileName.FromNameAndExtension("file", "txt"));
        }

        [Test]
        public void Create_From_Name_That_Ends_With_Dot_Throws()
        {
            Assert.Throws<ArgumentException>(() => FileName.FromNameAndExtension("file.", ".txt"));
        }

        [Test]
        public void Create_From_Empty_Name_And_Extension_Throws()
        {
            Assert.Throws<InvalidPathException>(() => FileName.FromNameAndExtension("", ".txt"));
        }

        [Test]
        public void Create_From_Null_Name_And_Extension_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => FileName.FromNameAndExtension(null, ".txt"));
        }

        [Test]
        public void Create_From_Name_And_Null_Extension_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => FileName.FromNameAndExtension("file", null));
        }

        private static void AssertThrowsInvalidPathExceptionFor(string invalidPath)
        {
            Assert.Throws<InvalidPathException>(() => FileName.FromString(invalidPath), string.Format("for '{0}'", invalidPath));
        }
    }
}