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


        [Test]
        public void FullName_Remains_Intact()
        {
            OriginalAndNameAndExtension.ForEach(FullName_Remains_Intact);
        }

        private void FullName_Remains_Intact(string[] testData)
        {
            FullName_Remains_Intact(testData[0], testData[1], testData[2]);
        }

        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void FullName_Remains_Intact(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).FullName, Is.EqualTo(original));
        }


        [Test]
        public void Name_Is_Correct()
        {
            OriginalAndNameAndExtension.ForEach(Name_Is_Correct);
        }

        private void Name_Is_Correct(string[] testData)
        {
            Name_Is_Correct(testData[0], testData[1], testData[2]);
        }

        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void Name_Is_Correct(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).Name(), Is.EqualTo(name));
        }


        [Test]
        public void Extension_Is_Correct()
        {
            OriginalAndNameAndExtension.ForEach(Extension_Is_Correct);
        }

        private void Extension_Is_Correct(string[] testData)
        {
            Extension_Is_Correct(testData[0], testData[1], testData[2]);
        }

        [Test, TestCaseSource("OriginalAndNameAndExtension")]
        public void Extension_Is_Correct(string original, string name, string extension)
        {
            Assert.That(FileName.FromString(original).Extension(), Is.EqualTo(extension));
        }
    }
}