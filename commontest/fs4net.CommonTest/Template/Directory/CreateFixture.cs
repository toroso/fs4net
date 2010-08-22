using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class CreateFixture : PopulatedFileSystem
    {
        [Test]
        public void Create_Folder_And_Parents()
        {
            var toBeCreated = ExistingLeafDirectory + RelativeDirectory.FromString(@"deeper\and\deeper");
            toBeCreated.Create();
            Assert.That(toBeCreated.Exists(), Is.True);
        }

        [Test]
        public void Create_Existing_Directory()
        {
            Assert.DoesNotThrow(() => ExistingLeafDirectory.Create());
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Create_Directory_That_Denotes_Existing_File_Throws()
        {
            var fileAsDirectory = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            Assert.Throws<IOException>(() => fileAsDirectory.Create());
            Assert.That(fileAsDirectory.Exists(), Is.False);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Create_Directory_On_NonExisting_Drive_Throws()
        {
            var toBeCreated = FileSystem.CreateDirectoryDescribing(@"z:\drive\does\no\exist");
            Assert.Throws<DirectoryNotFoundException>(() => toBeCreated.Create());
        }

        // TODO: Access denied
    }
}