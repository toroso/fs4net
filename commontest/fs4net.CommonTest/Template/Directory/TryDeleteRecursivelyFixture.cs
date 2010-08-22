using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class TryDeleteRecursivelyFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Existing_With_SubFolders_And_Files()
        {
            Assert.That(ParentOfExistingLeafDirectory.TryDeleteRecursively(), Is.True);
            Assert.That(ParentOfExistingLeafDirectory.Exists(), Is.False);
            Assert.That(ExistingLeafDirectory.Exists(), Is.False);
            Assert.That(ExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Delete_NonExisting_Directory()
        {
            Assert.That(NonExistingDirectory.TryDeleteRecursively(), Is.True);
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_Directory_That_Denotes_A_File_Throws()
        {
            var fileAsDirectory = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            Assert.That(fileAsDirectory.TryDeleteRecursively(), Is.True); // Disputable... There's still a file with that name.
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_Directory_On_NonExisting_Drive_Throws()
        {
            var toBeCreated = FileSystem.CreateDirectoryDescribing(@"z:\drive\does\no\exist");
            Assert.That(toBeCreated.TryDeleteRecursively(), Is.True);
        }

        // TODO: Access denied
        // e.g. Current Directory, file is open, directory is in use, read-only
    }
}