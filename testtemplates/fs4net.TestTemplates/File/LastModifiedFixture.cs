using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
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
            Assert.Throws<FileNotFoundException>(() => FileSystem.FileDescribing(ExistingLeafDirectory.PathAsString).LastModified());
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

        [Test]
        public void Create_File_Sets_LastModified()
        {
            var newFile = ExistingLeafDirectory + RelativeFile.FromString("new file.txt");
            DateTime before = DateTime.Now.AddSeconds(-1);
            newFile.WriteText(string.Empty);
            DateTime after = DateTime.Now.AddSeconds(1);
            Assert.That(newFile.LastModified(), Is.InRange(before, after));
        }

        // TODO: Rename File
        //[Test]
        //public void Rename_File_Sets_LastModified()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.Move(...);
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile.LastModified(), Is.InRange(before, after));
        //}

        // TODO: Append
        //[Test]
        //public void Modify_File_Contents_Sets_LastModified()
        //{
        //    DateTime before = DateTime.Now.AddSeconds(-1);
        //    ExistingFile.AppendText("tomte");
        //    DateTime after = DateTime.Now.AddSeconds(1);
        //    Assert.That(ExistingFile..LastModified(), Is.InRange(before, after));
        //}

        // TODO: Move file from one dir to another -- file modified?
    }
}