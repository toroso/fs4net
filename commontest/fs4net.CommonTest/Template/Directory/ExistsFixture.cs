using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    public abstract class ExistsFixture : PopulatedFileSystem
    {
        [Test]
        public void Existing_Leaf_Directory_Exists()
        {
            Assert.That(ExistingLeafDirectory.Exists(), Is.True);
        }

        [Test]
        public void Existing_Parent_Directory_Exists()
        {
            Assert.That(ParentOfExistingLeafDirectory.Exists(), Is.True);
        }

        [Test]
        public void NonExisting_Directory_Does_Not_Exists()
        {
            Assert.That(NonExistingDirectory.Exists(), Is.False);
        }

        [Test]
        public void Existing_File_Does_Not_Exists_As_Directory()
        {
            Assert.That(FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString).Exists(), Is.False);
        }
    }
}