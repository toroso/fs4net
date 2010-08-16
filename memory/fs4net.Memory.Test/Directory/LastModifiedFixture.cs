using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.Memory.Test.Directory
{
    [TestFixture]
    public class LastModifiedFixture : PopulatedFileSystem
    {
        [Test]
        public void LastModified_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.LastModified(), Is.EqualTo(ExistingLeafDirectoryLastModified));
        }

        [Test]
        public void LastModified_On_Directory_For_Existing_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString).LastModified());
        }

        [Test]
        public void LastModified_On_NonExisting_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingDirectory.LastModified());
        }
    }
}