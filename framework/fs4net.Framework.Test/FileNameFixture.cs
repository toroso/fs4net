using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class FileNameFixture
    {
        private static readonly string[][] OriginalAndNameAndExtension =
            {
                new[] { "file.txt", "file", ".txt" },
                new[] { "file.t", "file", ".t" },
                new[] { "file", "file", "" },
                new[] { "file.txt.tgz", "file.txt", ".tgz" },
            };


        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void FullName_Remains_Intact(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).FullName, Is.EqualTo(original));
        }


        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void Name_Is_Correct(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).Name(), Is.EqualTo(name));
        }


        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void Extension_Is_Correct(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).Extension(), Is.EqualTo(extension));
        }
    }
}