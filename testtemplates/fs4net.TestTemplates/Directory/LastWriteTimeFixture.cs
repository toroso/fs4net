using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class LastWriteTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void LastWriteTime_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.EqualTo(ExistingLeafDirectoryLastWriteTime));
        }

        [Test]
        public void LastWriteTime_On_Directory_For_Existing_File_Throws()
        {
            Should.Throw<DirectoryNotFoundException>(() => FileSystem.DirectoryDescribing(ExistingFile.PathAsString).LastWriteTime());
        }

        [Test]
        public void LastWriteTime_On_NonExisting_Directory_Throws()
        {
            Should.Throw<DirectoryNotFoundException>(() => NonExistingDirectory.LastWriteTime());
        }

        [Test]
        public void Set_LastWriteTime_Below_Min_Value_Throws()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => ExistingLeafDirectory.SetLastWriteTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.EqualTo(ExistingLeafDirectoryLastWriteTime));
        }

        [Test]
        public void Set_LastWriteTime_To_Min_Value()
        {
            ExistingLeafDirectory.SetLastWriteTime(MinimumDate);
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastWriteTime_To_Max_Value()
        {
            ExistingLeafDirectory.SetLastWriteTime(MaximumDate);
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_Directory_Sets_LastWriteTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newDir.LastWriteTime(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Sets_Parent_Directory_LastWriteTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Does_Not_Set_Parent_Parent_Directory_LastWriteTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.Parent().LastWriteTime(), Is.Not.InRange(before, after));
        }

        // TODO: Rename Directory
        //[Test]
        //public void Rename_Directory_Does_Not_Set_LastWriteTime()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingLeafDirectory.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.Not.InRange(before, after));
        //}

        [Test]
        public void Create_File_Sets_Parent_Directory_LastWriteTime()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastWriteTime(), Is.InRange(before, after));
        }

        [Test]
        public void Delete_File_Sets_Parent_Directory_LastWriteTime()
        {
            DateTime before = DateTime.Now.AddSeconds(-1);
            ExistingFile.Delete();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingFile.Parent().LastWriteTime(), Is.InRange(before, after));
        }

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Does_Not_Set_Parent_Directory_LastWriteTime()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.Parent().LastWriteTime(), Is.Not.InRange(before, after));
        //}

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_Parent_Directory_LastWriteTime()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.Parent().LastWriteTime(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- modified source and target directories?
        // TODO: Access denied?
    }
}