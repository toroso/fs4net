using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
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
            var file = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            Assert.Throws<UnauthorizedAccessException>(() => file.ReadText());
        }

        [Test]
        public void Read_NonExisting_File_From_NonExisting_Drive()
        {
            var file = NonExistingDrive + RelativeFile.FromString(@"path\to\nonexisting.txt");
            Assert.Throws<DirectoryNotFoundException>(() => file.ReadText());
            Assert.That(file.Exists(), Is.False);
        }

        [Test]
        public void Seek_In_Read_Stream()
        {
            using (var stream = ExistingFile.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                string wholeContents = reader.ReadToEnd();
                const int readOffset = 4;
                stream.Seek(readOffset, SeekOrigin.Begin);
                string endPart = reader.ReadToEnd();
                Assert.That(wholeContents.Substring(readOffset), Is.EqualTo(endPart));
            }
        }

        [Test]
        public void Write_To_Read_Stream_Throws()
        {
            using (var stream = ExistingFile.CreateReadStream())
            {
                Assert.Throws<ArgumentException>(() => new StreamWriter(stream));
            }
        }

        // TODO: Access denied
        // e.g. file is in use for writing
    }
}