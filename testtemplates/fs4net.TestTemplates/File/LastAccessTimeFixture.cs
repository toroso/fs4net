using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class LastAccessTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void LastAccessTime_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastAccessTime(), Is.EqualTo(ExistingFileLastAccessTime));
        }

        [Test]
        public void LastAccessTime_On_File_For_Existing_Directory_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => FileSystem.FileDescribing(ExistingLeafDirectory.PathAsString).LastAccessTime());
        }

        [Test]
        public void LastAccessTime_On_NonExisting_File_Throws()
        {
            Assert.Throws<FileNotFoundException>(() => NonExistingFile.LastAccessTime());
        }

        [Test]
        public void Set_LastAccessTime_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingFile.SetLastAccessTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingFile.LastAccessTime(), Is.EqualTo(ExistingFileLastAccessTime));
        }

        [Test]
        public void Set_LastAccessTime_To_Min_Value()
        {
            ExistingFile.SetLastAccessTime(MinimumDate);
            Assert.That(ExistingFile.LastAccessTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastAccessTime_To_Max_Value()
        {
            ExistingFile.SetLastAccessTime(MaximumDate);
            Assert.That(ExistingFile.LastAccessTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_File_Sets_LastAccessTime()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newFile.LastAccessTime(), Is.InRange(before, after));
        }

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_LastAccessTime() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.LastAccessTime(), Is.InRange(before, after));
        //}

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Sets_LastAccessTime() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile..LastAccessTime(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- file accessed?
    }
}