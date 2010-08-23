using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class DeleteIfEmptyFixture : PopulatedFileSystem
    {
        [Test]
        public void Delete_Empty_Directory()
        {
            ExistingEmptyDirectory.DeleteIfEmpty();
            Assert.That(ExistingEmptyDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_Directory_Containing_File_Throws()
        {
            Assert.Throws<IOException>(() => ExistingLeafDirectory.DeleteIfEmpty());
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_NonExisting_Directory()
        {
            Assert.DoesNotThrow(() => NonExistingDirectory.DeleteIfEmpty());
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Delete_Directory_That_Denotes_A_File_Throws()
        {
            var fileAsDirectory = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            Assert.Throws<IOException>(() => fileAsDirectory.DeleteIfEmpty());
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Delete_Directory_On_NonExisting_Drive_Throws()
        {
            var toBeCreated = NonExistingDrive + RelativeDirectory.FromString(@"drive\does\no\exist");
            Assert.DoesNotThrow(() => toBeCreated.DeleteIfEmpty());
        }


        // TODO: Access denied
        // e.g. Current Directory, file is open, directory is in use, read-only
    }
}