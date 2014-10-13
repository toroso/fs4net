using System.IO;
using System.Linq;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class FilesFixture : PopulatedFileSystem
    {
        [Test]
        public void Files_For_Empty_Directory_Returns_No_Files()
        {
            Assert.That(ExistingEmptyDirectory.Files().Count(), Is.EqualTo(0));
        }

        [Test]
        public void Files_For_Directory_With_Only_Directories_Returns_No_Files()
        {
            Assert.That(ExistingEmptyDirectory.Parent().Files().Count(), Is.EqualTo(0));
        }

        [Test]
        public void Files_For_Directory_With_Files_Returns_Those_Files()
        {
            var actual = ExistingLeafDirectory.Files();
            Assert.That(actual, Is.EquivalentTo(new[] { ExistingFile, ExistingEmptyFile }));
        }

        [Test]
        public void Files_For_NonExisting_Directory_Throws()
        {
            Should.Throw<DirectoryNotFoundException>(() => NonExistingDirectory.Files());
        }

        [Test]
        public void Files_For_Directory_That_Is_A_File_Throws()
        {
            var fileAsDirectory = FileSystem.DirectoryDescribing(ExistingFile.PathAsString);
            Should.Throw<DirectoryNotFoundException>(() => fileAsDirectory.Files());
        }


        // TODO: Access denied
        // UnauthorizedAccessException: The caller does not have the required permission.
    }
}