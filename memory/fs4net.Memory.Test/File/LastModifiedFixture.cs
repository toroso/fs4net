using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.Memory.Test.File
{
    [TestFixture]
    public class LastModifiedFixture : PopulatedFileSystem
    {
        [Test]
        public void LastModified_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(ExistingFileLastModified));
        }

        [Test]
        public void LastModified_On_File_For_Existing_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => FileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).LastModified());
        }

        [Test]
        public void LastModified_On_NonExisting_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.LastModified());
        }
    }
}