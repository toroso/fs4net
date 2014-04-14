using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class DeleteFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Existing_File()
        {
            ExistingFile.Delete();
            Assert.That(ExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_File()
        {
            NonExistingFile.Delete();
            Assert.That(NonExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_File_In_NonExisting_Directory_Does_Not_Throw()
        {
            var toBeDeleted = (NonExistingDirectory + FileName.FromString("file.txt"));
            Should.NotThrow(() => toBeDeleted.Delete());
        }

        [Test]
        public void Delete_File_That_Is_A_Directory_Throws()
        {
            var directoryAsFile = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            Should.Throw<UnauthorizedAccessException>(() => directoryAsFile.Delete());
            Assert.That(ExistingEmptyDirectory.Exists(), Is.True);
        }

        [Test]
        [Ignore("Don't know what drives exist on the CI")]
        public void Delete_File_On_NonExisting_Drive_Throws()
        {
            var toBeDeleted = NonExistingDrive + RelativeFile.FromString(@"drive\does\not\exist.txt");
            Should.Throw<DirectoryNotFoundException>(() => toBeDeleted.Delete());
        }

        // TODO: Access denied
        // e.g. file is in use, read-only
    }
}