using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.File
{
    public abstract class CreateReadStreamFixture : PopulatedFileSystem
    {
        [Test]
        public void Read_Existing_File()
        {
            Assert.That(ExistingFile.ReadText(), Is.EqualTo(ExistingFileContents));
        }

        [Test]
        public void Read_NonExisting_File_In_Existing_Directory()
        {
            var file = ExistingEmptyDirectory + RelativeFile.FromString("nonexisting.txt");
            Assert.Throws<FileNotFoundException>(() => file.ReadText());
            Assert.That(file.Exists(), Is.False);
        }

        [Test]
        public void Read_NonExisting_File_In_NonExisting_Directory()
        {
            var file = NonExistingDirectory + RelativeFile.FromString("nonexisting.txt");
            Assert.Throws<DirectoryNotFoundException>(() => file.ReadText());
            Assert.That(NonExistingDirectory.Exists(), Is.False);
            Assert.That(file.Exists(), Is.False);
        }

        [Test]
        public void Read_File_That_Is_An_Existing_Directory()
        {
            var file = FileSystem.CreateFileDescribing(ExistingEmptyDirectory.PathAsString);
            Assert.Throws<UnauthorizedAccessException>(() => file.ReadText());
        }

        [Test]
        public void Read_NonExisting_File_From_NonExisting_Drive()
        {
            var file = NonExistingDrive + RelativeFile.FromString(@"path\to\nonexisting.txt");
            Assert.Throws<DirectoryNotFoundException>(() => file.ReadText());
            Assert.That(file.Exists(), Is.False);
        }

        // TODO: Access denied
        // e.g. file is in use for writing
    }
}