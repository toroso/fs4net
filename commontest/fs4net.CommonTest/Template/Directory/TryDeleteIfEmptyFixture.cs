using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class TryDeleteIfEmptyFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Empty_Directory_Succeeds()
        {
            Assert.That(ExistingEmptyDirectory.TryDeleteIfEmpty(), Is.True);
            Assert.That(ExistingEmptyDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_Directory_Containing_File_Fails()
        {
            Assert.That(ExistingLeafDirectory.TryDeleteIfEmpty(), Is.False);
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_NonExisting_Directory_Succeeds()
        {
            Assert.That(NonExistingDirectory.TryDeleteIfEmpty(), Is.True);
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_Directory_That_Is_A_File_Succeeds()
        {
            var fileAsDirectory = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            Assert.That(fileAsDirectory.TryDeleteIfEmpty(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_Directory_On_NonExisting_Drive_Succeeds()
        {
            var toBeCreated = NonExistingDrive + RelativeDirectory.FromString(@"drive\does\no\exist");
            Assert.That(toBeCreated.TryDeleteIfEmpty(), Is.True);
        }


        // TODO: Access denied
        // e.g. Current Directory, file is open, directory is in use, read-only
    }
}