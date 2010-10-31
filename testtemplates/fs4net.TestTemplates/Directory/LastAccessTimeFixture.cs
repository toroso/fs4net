using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class LastAccessTimeFixture : PopulatedFileSystem
    {
        [Test]
        public void LastAccessTime_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.EqualTo(ExistingLeafDirectoryLastAccessTime));
        }

        [Test]
        public void LastAccessTime_On_Directory_For_Existing_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => FileSystem.DirectoryDescribing(ExistingFile.PathAsString).LastAccessTime());
        }

        [Test]
        public void LastAccessTime_On_NonExisting_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingDirectory.LastAccessTime());
        }

        [Test]
        public void Set_LastAccessTime_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingLeafDirectory.SetLastAccessTime(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.EqualTo(ExistingLeafDirectoryLastAccessTime));
        }

        [Test]
        public void Set_LastAccessTime_To_Min_Value()
        {
            ExistingLeafDirectory.SetLastAccessTime(MinimumDate);
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastAccessTime_To_Max_Value()
        {
            ExistingLeafDirectory.SetLastAccessTime(MaximumDate);
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_Directory_Sets_LastAccessTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newDir.LastAccessTime(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Sets_Parent_Directory_LastAccessTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Does_Not_Set_Parent_Parent_Directory_LastAccessTime()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.Parent().LastAccessTime(), Is.Not.InRange(before, after));
        }

        // TODO: Rename Directory
        //[Test]
        //public void Rename_Directory_Does_Not_Set_LastAccessTime() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingLeafDirectory.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.Not.InRange(before, after));
        //}

        [Test]
        public void Create_File_Sets_Parent_Directory_LastAccessTime()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastAccessTime(), Is.InRange(before, after));
        }

        [Test]
        public void Delete_File_Sets_Parent_Directory_LastAccessTime()
        {
            DateTime before = DateTime.Now.AddSeconds(-1);
            ExistingFile.Delete();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingFile.Parent().LastAccessTime(), Is.InRange(before, after));
        }

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Does_Not_Set_Parent_Directory_LastAccessTime() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.Parent().LastAccessTime(), Is.Not.InRange(before, after));
        //}

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_Parent_Directory_LastAccessTime() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.Parent().LastAccessTime(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- modified source and target directories?
        // TODO: Access denied?
    }
}