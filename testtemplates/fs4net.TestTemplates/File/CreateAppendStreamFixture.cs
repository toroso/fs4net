using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class CreateAppendStreamFixture : PopulatedFileSystem
    {
        [Test]
        public void Append_To_Existing_File()
        {
            const string appendText = "Lluvia";
            ExistingFile.AppendText(appendText);
            var expectedContents = ExistingFileContents + appendText;
            Assert.That(ExistingFile.ReadText(), Is.EqualTo(expectedContents));
            Assert.That(ExistingFile.Size(), Is.EqualTo(expectedContents.Length));
        }

        [Test]
        public void Append_To_NonExisting_File()
        {
            var file = ExistingEmptyDirectory + RelativeFile.FromString("newfile.txt");
            const string contents = "Paradise";
            file.AppendText(contents);
            Assert.That(file.ReadText(), Is.EqualTo(contents));
            Assert.That(file.Size(), Is.EqualTo(contents.Length));
        }

        [Test]
        public void Append_To_NonExisting_File_In_NonExisting_Directory()
        {
            var file = NonExistingDirectory + RelativeFile.FromString("nonexisting.txt");
            Should.Throw<DirectoryNotFoundException>(() => file.AppendText("Willow"));
            Assert.That(NonExistingDirectory.Exists(), Is.False);
            Assert.That(file.Exists(), Is.False);
        }

        [Test]
        public void Append_To_File_That_Is_An_Existing_Directory()
        {
            var file = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            Should.Throw<UnauthorizedAccessException>(() => file.AppendText("Nota Bossa"));
        }

        [Test]
        [Ignore("Don't know what drives exist on the CI")]
        public void Append_To_File_On_NonExisting_Drive()
        {
            var file = NonExistingDrive + RelativeFile.FromString(@"path\to\nonexisting.txt");
            Should.Throw<DirectoryNotFoundException>(() => file.AppendText("Nah nah nah"));
            Assert.That(file.Exists(), Is.False);
        }

        [Test]
        public void Seek_In_Append_Stream_Throws()
        {
            using (var stream = ExistingFile.CreateAppendStream())
            {
                Should.Throw<IOException>(() => stream.Seek(0, SeekOrigin.Begin));
            }
            Assert.That(ExistingFile.ReadText(), Is.EqualTo(ExistingFileContents));
        }

        // TODO: Access denied
    }
}
