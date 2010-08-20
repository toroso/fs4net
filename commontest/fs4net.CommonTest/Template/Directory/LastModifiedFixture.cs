using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    [TestFixture]
    public abstract class LastModifiedFixture : PopulatedFileSystem
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

        [Test]
        public void Set_LastModified_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingLeafDirectory.SetLastModified(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingLeafDirectory.LastModified(), Is.EqualTo(ExistingLeafDirectoryLastModified));
        }

        [Test]
        public void Set_LastModified_To_Min_Value()
        {
            ExistingLeafDirectory.SetLastModified(MinimumDate);
            Assert.That(ExistingLeafDirectory.LastModified(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastModified_To_Max_Value()
        {
            ExistingLeafDirectory.SetLastModified(MaximumDate);
            Assert.That(ExistingLeafDirectory.LastModified(), Is.EqualTo(MaximumDate));
        }

        // TODO: Sets LastModified:
        //  * Create the folder
        //  * Create or rename a file inside the folder
        // NOT by:
        //  * Modify contents of a file inside the folder
        //  * Rename the folder
    }
}