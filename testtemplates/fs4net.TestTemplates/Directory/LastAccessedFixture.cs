using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class LastAccessedFixture : PopulatedFileSystem
    {
        [Test]
        public void LastAccessed_For_Directory_Is_Correct()
        {
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.EqualTo(ExistingLeafDirectoryLastAccessed));
        }

        [Test]
        public void LastAccessed_On_Directory_For_Existing_File_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString).LastAccessed());
        }

        [Test]
        public void LastAccessed_On_NonExisting_Directory_Throws()
        {
            // TODO: Don't like this exception: DirectoryNotFound?
            Assert.Throws<FileNotFoundException>(() => NonExistingDirectory.LastAccessed());
        }

        [Test]
        public void Set_LastAccessed_Below_Min_Value_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExistingLeafDirectory.SetLastAccessed(MinimumDate.AddMilliseconds(-1)));
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.EqualTo(ExistingLeafDirectoryLastAccessed));
        }

        [Test]
        public void Set_LastAccessed_To_Min_Value()
        {
            ExistingLeafDirectory.SetLastAccessed(MinimumDate);
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.EqualTo(MinimumDate));
        }

        [Test]
        public void Set_LastAccessed_To_Max_Value()
        {
            ExistingLeafDirectory.SetLastAccessed(MaximumDate);
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.EqualTo(MaximumDate));
        }

        [Test]
        public void Create_Directory_Sets_LastAccessed()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newDir.LastAccessed(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Sets_Parent_Directory_LastAccessed()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Does_Not_Set_Parent_Parent_Directory_LastAccessed()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.ParentDirectory().LastAccessed(), Is.Not.InRange(before, after));
        }

        // TODO: Rename Directory
        //[Test]
        //public void Rename_Directory_Does_Not_Set_LastAccessed() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingLeafDirectory.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingLeafDirectory.LastAccessed(), Is.Not.InRange(before, after));
        //}

        [Test]
        public void Create_File_Sets_Parent_Directory_LastAccessed()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastAccessed(), Is.InRange(before, after));
        }

        [Test]
        public void Delete_File_Sets_Parent_Directory_LastAccessed()
        {
            DateTime before = DateTime.Now.AddSeconds(-1);
            ExistingFile.Delete();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingFile.ParentDirectory().LastAccessed(), Is.InRange(before, after));
        }

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Does_Not_Set_Parent_Directory_LastAccessed() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.ParentDirectory().LastAccessed(), Is.Not.InRange(before, after));
        //}

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_Parent_Directory_LastAccessed() -- or does it?
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.ParentDirectory().LastAccessed(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- modified source and target directories?
        // TODO: Access denied?
    }
}