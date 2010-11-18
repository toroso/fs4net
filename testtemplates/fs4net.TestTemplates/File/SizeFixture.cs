using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class SizeFixture : PopulatedFileSystem
    {
        [Test]
        public void NonEmpty_File()
        {
            Assert.That(ExistingFile.Size(), Is.EqualTo(ExistingFileContents.Length));
        }

        [Test]
        public void Empty_File()
        {
            Assert.That(ExistingEmptyFile.Size(), Is.EqualTo(0));
        }

        [Test]
        public void NonExisting_File()
        {
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.Size());
        }

        [Test]
        public void File_Denotes_An_Existing_Directory()
        {
            Assert.Throws<FileNotFoundException>(() => FileSystem.FileDescribing(ExistingLeafDirectory.PathAsString).Size());
        }

        [Test]
        public void File_On_NonExisting_Drive()
        {
            Assert.Throws<FileNotFoundException>(() => (NonExistingDrive + FileName.FromString("file.txt")).Size());
        }
    }
}