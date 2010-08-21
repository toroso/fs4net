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

        [Test]
        public void Create_Directory_Sets_LastModified()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newDir.LastModified(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Sets_Parent_Directory_LastModified()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastModified(), Is.InRange(before, after));
        }

        [Test]
        public void Create_Directory_Does_Not_Set_Parent_Parent_Directory_LastModified()
        {
            var newDir = ExistingLeafDirectory + RelativeDirectory.FromString("new dir");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newDir.Create();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.ParentDirectory().LastModified(), Is.Not.InRange(before, after));
        }

        // TODO: Rename Directory
        //[Test]
        //public void Rename_Directory_Does_Not_Set_LastModified()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingLeafDirectory.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingLeafDirectory.LastModified(), Is.Not.InRange(before, after));
        //}

        [Test]
        public void Create_File_Sets_Parent_Directory_LastModified()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingLeafDirectory.LastModified(), Is.InRange(before, after));
        }

        [Test]
        public void Delete_File_Sets_Parent_Directory_LastModified()
        {
            DateTime before = DateTime.Now.AddSeconds(-1);
            ExistingFile.Delete();
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(ExistingFile.ParentDirectory().LastModified(), Is.InRange(before, after));
        }

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Does_Not_Set_Parent_Directory_LastModified()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.ParentDirectory().LastModified(), Is.Not.InRange(before, after));
        //}

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_Parent_Directory_LastModified()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.ParentDirectory().LastModified(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- modified source and target directories?
    }
}