using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.File
{
    [TestFixture]
    public abstract class ExistsFixture : PopulatedFileSystem
    {
        [Test]
        public void Existing_File_Exists()
        {
            Assert.That(ExistingFile.Exists(), Is.True);
        }

        [Test]
        public void NonExisting_File_Does_Not_Exists()
        {
            Assert.That(NonExistingFile.Exists(), Is.False);
        }

        [Test]
        public void Existing_Directory_Does_Not_Exists_As_File()
        {
            Assert.That(FileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).Exists(), Is.False);
        }
    }
}