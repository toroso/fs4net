using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class LastWriteTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void LastWriteTime_For_File_Is_Correct()
        {
            Assert.That(ExistingFile.LastWriteTime(), Is.EqualTo(ExistingFileLastWriteTime));
        }

        [Test]
        public void LastWriteTime_On_File_For_Existing_Directory_Throws()
        {
            Should.Throw<FileNotFoundException>(() => FileSystem.FileDescribing(ExistingLeafDirectory.PathAsString).LastWriteTime());
        }

        [Test]
        public void LastWriteTime_On_NonExisting_File_Throws()
        {
            Should.Throw<FileNotFoundException>(() => NonExistingFile.LastWriteTime());
        }

        [Test]
        public void Set_LastWriteTime_Below_Min_Value_Throws()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => ExistingFile.SetLastWriteTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingFile.LastWriteTime(), Is.EqualTo(ExistingFileLastWriteTime));
        }

        [Test]
        public void Set_LastWriteTime_To_Min_Value()
        {
            ExistingFile.SetLastWriteTime(MinimumDate);
            Assert.That(ExistingFile.LastWriteTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastWriteTime_To_Max_Value()
        {
            ExistingFile.SetLastWriteTime(MaximumDate);
            Assert.That(ExistingFile.LastWriteTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_File_Sets_LastWriteTime()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newFile.LastWriteTime(), Is.InRange(before, after));
        }

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_LastWriteTime()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.LastWriteTime(), Is.InRange(before, after));
        //}

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Sets_LastWriteTime()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile..LastWriteTime(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- file modified?
    }
}