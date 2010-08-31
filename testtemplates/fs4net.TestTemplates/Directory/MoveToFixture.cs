using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Directory
{
    public abstract class MoveToFixture : PopulatedFileSystem
    {
        [Test]
        public void Move_Directory_Containing_FileSystemItems()
        {
            var source = ParentOfExistingLeafDirectory;
            var destination = source.ParentDirectory() + RelativeDirectory.FromString("new name");

            source.MoveTo(destination);

            Assert.That(source.Exists(), Is.False);
            Assert.That(destination.Exists(), Is.True);

            var movedFile = destination + ExistingFile.RelativeFrom(source);
            Assert.That(movedFile.Exists(), Is.True);
        }

        [Test]
        public void Move_NonExisting_Directory_Throws()
        {
            var source = NonExistingDirectory;
            var destination = ExistingLeafDirectory + RelativeDirectory.FromString("new name");

            Assert.Throws<DirectoryNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_File_As_Directory_Throws()
        {
            var source = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);
            var destination = ExistingLeafDirectory + RelativeDirectory.FromString("new name");

            Assert.Throws<DirectoryNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_Directory_Into_NonExisting_Directory_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = NonExistingDirectory + RelativeDirectory.FromString("new name");

            Assert.Throws<DirectoryNotFoundException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Existing_Directory_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingEmptyDirectory;

            Assert.Throws<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Existing_File_Throws()
        {
            var source = ExistingEmptyDirectory;
            var destination = FileSystem.CreateDirectoryDescribing(ExistingFile.PathAsString);

            Assert.Throws<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Itself_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingLeafDirectory;

            Assert.Throws<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Its_Own_Subfolder_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingLeafDirectory + RelativeDirectory.FromString("new name");

            Assert.Throws<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Another_Drive_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = FileSystem.CreateDirectoryDescribing(@"d:\another drive");

            Assert.Throws<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Between_FileSystems_Throws()
        {
            var source = ParentOfExistingLeafDirectory;
            var destination = CreateFileSystem().CreateDirectoryDescribing((source.ParentDirectory() + RelativeDirectory.FromString("new name")).PathAsString);

            Assert.Throws<InvalidOperationException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        // TODO: Destination is on non-existing drive
        // TODO: Access denied (source and destination)
    }
}