using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class CreationTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void CreationTime_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.CreationTime(), Is.EqualTo(ExistingFileCreationTime));
        }

        [Test]
        public void CreationTime_On_File_For_Existing_Directory_Throws()
        {
            Should.Throw<FileNotFoundException>(() => FileSystem.FileDescribing(ExistingLeafDirectory.PathAsString).CreationTime());
        }

        [Test]
        public void CreationTime_On_NonExisting_File_Throws()
        {
            Should.Throw<FileNotFoundException>(() => NonExistingFile.CreationTime());
        }

        [Test]
        public void Set_CreationTime_Below_Min_Value_Throws()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => ExistingFile.SetCreationTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingFile.CreationTime(), Is.EqualTo(ExistingFileCreationTime));
        }

        [Test]
        public void Set_CreationTime_To_Min_Value()
        {
            ExistingFile.SetCreationTime(MinimumDate);
            Assert.That(ExistingFile.CreationTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_CreationTime_To_Max_Value()
        {
            ExistingFile.SetCreationTime(MaximumDate);
            Assert.That(ExistingFile.CreationTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_File_Sets_CreationTime()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newFile.CreationTime(), Is.InRange(before, after));
        }
    }
}