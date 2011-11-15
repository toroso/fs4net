using System;
using fs4net.Framework.Utils;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class IsValidRelativeFileFixture
    {
        [Test]
        public void Throws_If_Path_Is_Null()
        {
            Should.Throw<ArgumentNullException>(() => ValidityCheckers.IsValidRelativeFile(null));
        }


        private static readonly string[] NonValidOrNonRelativeFile =
            {
                @"",
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
                @"c:\standard\relative\path\to\file.txt",
                @"z:\last\drive\path\to\file.txt",
                @"\\network\drive\path\to\file.txt",
                @"ö:\non\a\to\z\drive\to\file.txt",
                @"\\colon:in\network\name\to\file.txt",
                @"\\colon\in:share\name\to\file.txt",
            };

        [Test, TestCaseSource("NonValidOrNonRelativeFile")]
        public void Non_Valid_Or_Non_Relative_File_Return_False(string path)
        {
            Assert.That(path.IsValidRelativeFile(), Is.False);
        }


        private static readonly string[] ValidRelativeFile =
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

        [Test, TestCaseSource("ValidRelativeFile")]
        public void Valid_Relative_File_Return_True(string path)
        {
            Assert.That(path.IsValidRelativeFile(), Is.True);
        }
    }
}