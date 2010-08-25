using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.File
{
    public abstract class CreateWriteStreamFixture : PopulatedFileSystem
    {
        [Test]
        public void Create_File_In_Existing_Directory()
        {
            var newFile = ExistingEmptyDirectory + RelativeFile.FromString("newfile.txt");
            Assert.That(newFile.Exists(), Is.False);
            newFile.WriteText(string.Empty);
            Assert.That(newFile.Exists(), Is.True);
        }

        [Test]
        public void Overwrite_Existing_File()
        {
            Assert.That(ExistingFile.Exists(), Is.True);
            const string newContents = "Mamboo magui";
            ExistingFile.WriteText(newContents);
            Assert.That(ExistingFile.Exists(), Is.True);
            Assert.That(ExistingFile.ReadText(), Is.EqualTo(newContents));
        }

        [Test]
        public void Create_File_In_NonExisting_Directory()
        {
            var newFile = NonExistingDirectory + RelativeFile.FromString("newfile.txt");
            Assert.That(newFile.Exists(), Is.False);
            Assert.Throws<DirectoryNotFoundException>(() => newFile.WriteText(string.Empty));
            Assert.That(newFile.Exists(), Is.False);
        }

        [Test]
        public void Create_File_That_Is_An_Existing_Directory()
        {
            var file = FileSystem.CreateFileDescribing(ExistingEmptyDirectory.PathAsString);
            Assert.Throws<UnauthorizedAccessException>(() => file.WriteText("Onom Mweng"));
        }

        [Test]
        public void Create_File_On_NonExisting_Drive()
        {
            var file = NonExistingDrive + RelativeFile.FromString(@"path\to\nonexisting.txt");
            Assert.Throws<DirectoryNotFoundException>(() => file.WriteText("Funky for you"));
            Assert.That(file.Exists(), Is.False);
        }

        // TODO: Access denied
        // e.g. file is in use, read-only
    }
}
