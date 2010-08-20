using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.File
{
    [TestFixture]
    public abstract class LastModifiedFixture : PopulatedFileSystem
    {
        [Test]
        public void LastModified_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(ExistingFileLastModified));
        }

        [Test]
        public void LastModified_On_File_For_Existing_Directory_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => FileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).LastModified());
        }

        [Test]
        public void LastModified_On_NonExisting_File_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.LastModified());
        }

        [Test]
        public void Set_LastModified_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingFile.SetLastModified(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(ExistingFileLastModified));
        }

        [Test]
        public void Set_LastModified_To_Min_Value()
        {
            ExistingFile.SetLastModified(MinimumDate);
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastModified_To_Max_Value()
        {
            ExistingFile.SetLastModified(MaximumDate);
            Assert.That(ExistingFile.LastModified(), Is.EqualTo(MaximumDate));
        }
    }
}