using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
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
            Should.NotThrow(() => ExistingLeafDirectory.Create());
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void Create_Directory_That_Denotes_Existing_File_Throws()
        {
            var fileAsDirectory = FileSystem.DirectoryDescribing(ExistingFile.PathAsString);
            Should.Throw<IOException>(() => fileAsDirectory.Create());
            Assert.That(fileAsDirectory.Exists(), Is.False);
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        [Ignore("Don't know what drives exist on the CI")]
        public void Create_Directory_On_NonExisting_Drive_Throws()
        {
            var toBeCreated = FileSystem.DirectoryDescribing(@"z:\drive\does\not\exist");
            Should.Throw<DirectoryNotFoundException>(() => toBeCreated.Create());
        }

        [Test]
        public void Create_Directory_Starting_With_Dot()
        {
            var toBeCreated = ExistingLeafDirectory + RelativeDirectory.FromString(@".startsWithDot");
            toBeCreated.Create();
            Assert.That(toBeCreated.Exists(), Is.True);
        }

        // TODO: Access denied
    }
}