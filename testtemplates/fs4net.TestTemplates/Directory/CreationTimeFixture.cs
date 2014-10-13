using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class CreationTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void CreationTime_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.CreationTime(), Is.EqualTo(ExistingLeafDirectoryCreationTime));
        }

        [Test]
        public void CreationTime_On_Directory_For_Existing_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Should.Throw<FileNotFoundException>(() => FileSystem.DirectoryDescribing(ExistingFile.PathAsString).CreationTime());
        }

        [Test]
        public void CreationTime_On_NonExisting_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Should.Throw<FileNotFoundException>(() => NonExistingDirectory.CreationTime());
        }

        [Test]
        public void Set_CreationTime_Below_Min_Value_Throws()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => ExistingLeafDirectory.SetCreationTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingLeafDirectory.CreationTime(), Is.EqualTo(ExistingLeafDirectoryCreationTime));
        }

        [Test]
        public void Set_CreationTime_To_Min_Value()
        {
            ExistingLeafDirectory.SetCreationTime(MinimumDate);
            Assert.That(ExistingLeafDirectory.CreationTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_CreationTime_To_Max_Value()
        {
            ExistingLeafDirectory.SetCreationTime(MaximumDate);
            Assert.That(ExistingLeafDirectory.CreationTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_Directory_Sets_CreationTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newDir.CreationTime(), Is.InRange(before, after));
        }
    }
}