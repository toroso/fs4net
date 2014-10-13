using System.IO;
using System.Linq;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class DirectoriesFixture : PopulatedFileSystem
    {
        [Test]
        public void Directories_For_Empty_Directory_Returns_No_Directories()
        {
            Assert.That(ExistingEmptyDirectory.Directories().Count(), Is.EqualTo(0));
        }

        [Test]
        public void Directories_For_Directory_With_Only_Directories_Returns_No_Directories()
        {
            Assert.That(ExistingLeafDirectory.Directories().Count(), Is.EqualTo(0));
        }

        [Test]
        public void Directories_For_Directory_With_Directory_Returns_That_Directory()
        {
            var actual = ExistingEmptyDirectory.Parent().Directories();
            Assert.That(actual, Is.EquivalentTo(new[] { ExistingEmptyDirectory, ExistingEmptyDirectory2 }));
        }

        [Test]
        public void Directories_For_NonExisting_Directory_Throws()
        {
            Should.Throw<DirectoryNotFoundException>(() => NonExistingDirectory.Directories());
        }

        [Test]
        public void Directories_For_Directory_That_Is_A_File_Throws()
        {
            var fileAsDirectory = FileSystem.DirectoryDescribing(ExistingFile.PathAsString);
            Should.Throw<DirectoryNotFoundException>(() => fileAsDirectory.Directories());
        }


        // TODO: Access denied
        // UnauthorizedAccessException: The caller does not have the required permission.
    }
}