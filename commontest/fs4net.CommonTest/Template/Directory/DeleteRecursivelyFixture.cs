using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class DeleteRecursivelyFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Existing_With_SubFolders_And_Files()
        {
            ParentOfExistingLeafDirectory.DeleteRecursively();
            Assert.That(ParentOfExistingLeafDirectory.Exists(), Is.False);
            Assert.That(ExistingLeafDirectory.Exists(), Is.False);
            Assert.That(ExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_Directory_Throws()
        {
            var toBeDeleted = (ExistingLeafDirectory + RelativeDirectory.FromString("nonexisting"));
            Assert.Throws<DirectoryNotFoundException>(() => toBeDeleted.DeleteRecursively()); // Not sure if I like this, but that's how .NET works...
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_Folder_In_NonExisting_Directory_Throws()
        {
            var toBeDeleted = (NonExistingDirectory + RelativeDirectory.FromString("nonexisting"));
            Assert.Throws<DirectoryNotFoundException>(() => toBeDeleted.DeleteRecursively());
        }

        [Test]
        public void Delete_Directory_That_Is_A_File_Throws()
        {
            var fileAsDirectory = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            Assert.Throws<IOException>(() => fileAsDirectory.DeleteRecursively());
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_Directory_On_NonExisting_Drive_Throws()
        {
            var toBeDeleted = NonExistingDrive + RelativeDirectory.FromString(@"drive\does\no\exist");
            Assert.Throws<DirectoryNotFoundException>(() => toBeDeleted.DeleteRecursively());
        }

        // TODO: Access denied
        // e.g. Current Directory, file is open, directory is in use, read-only
    }
}