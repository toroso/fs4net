using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class LastAccessedFixture : PopulatedFileSystem
    {
        [Test]
        public void LastAccessed_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastAccessed(), Is.EqualTo(ExistingFileLastAccessed));
        }

        [Test]
        public void LastAccessed_On_File_For_Existing_Directory_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => FileSystem.CreateFileDescribing(ExistingLeafDirectory.PathAsString).LastAccessed());
        }

        [Test]
        public void LastAccessed_On_NonExisting_File_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.LastAccessed());
        }

        [Test]
        public void Set_LastAccessed_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingFile.SetLastAccessed(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingFile.LastAccessed(), Is.EqualTo(ExistingFileLastAccessed));
        }

        [Test]
        public void Set_LastAccessed_To_Min_Value()
        {
            ExistingFile.SetLastAccessed(MinimumDate);
            Assert.That(ExistingFile.LastAccessed(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastAccessed_To_Max_Value()
        {
            ExistingFile.SetLastAccessed(MaximumDate);
            Assert.That(ExistingFile.LastAccessed(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_File_Sets_LastAccessed()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newFile.LastAccessed(), Is.InRange(before, after));
        }

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_LastAccessed() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.LastAccessed(), Is.InRange(before, after));
        //}

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Sets_LastAccessed() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile..LastAccessed(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- file accessed?
    }
}