using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class TryDeleteFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Existing_File_Succeeds()
        {
            Assert.That(ExistingFile.TryDelete(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_File_Succeeds()
        {
            Assert.That(NonExistingFile.TryDelete(), Is.True);
            Assert.That(NonExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_File_In_NonExisting_Directory_Succeeds()
        {
            var toBeDeleted = (NonExistingDirectory + FileName.FromString("file.txt"));
            Assert.That(toBeDeleted.TryDelete(), Is.True);
        }

        [Test]
        public void Delete_File_That_Is_A_Directory_Succeeds()
        {
            var directoryAsFile = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            Assert.That(directoryAsFile.TryDelete(), Is.True); // Disputable... There's still a directory with that name.
            Assert.That(ExistingEmptyDirectory.Exists(), Is.True);
        }

        [Test]
        public void Delete_File_On_NonExisting_Drive_Succeeds()
        {
            var toBeDeleted = NonExistingDrive + RelativeFile.FromString(@"drive\does\not\exist.txt");
            Assert.That(toBeDeleted.TryDelete(), Is.True);
        }

        // TODO: Access denied
        // e.g. file is in use, read-only
    }
}
