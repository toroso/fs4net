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
            var destination = source.Parent() + RelativeDirectory.FromString("new name");

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

            Should.Throw<DirectoryNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_File_As_Directory_Throws()
        {
            var source = FileSystem.DirectoryDescribing(ExistingFile.PathAsString);
            var destination = ExistingLeafDirectory + RelativeDirectory.FromString("new name");

            Should.Throw<DirectoryNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_Directory_Into_NonExisting_Directory_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = NonExistingDirectory + RelativeDirectory.FromString("new name");

            Should.Throw<DirectoryNotFoundException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Existing_Directory_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingEmptyDirectory;

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Existing_File_Throws()
        {
            var source = ExistingEmptyDirectory;
            var destination = FileSystem.DirectoryDescribing(ExistingFile.PathAsString);

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Itself_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingLeafDirectory;

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Its_Own_Subfolder_Throws()
        {
            var source = ExistingLeafDirectory;
            var destination = ExistingLeafDirectory + RelativeDirectory.FromString("new name");

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Directory_To_Another_Drive_Throws()
        {
            if (ExistingLeafDirectory.Drive() == FileSystem.DriveDescribing("d:"))
            {
                Assert.Ignore(string.Format("The test assumes that the temp directory is not on the d: drive, but it is on '{0}'", ExistingLeafDirectory.Drive()));
            }

            var source = ExistingLeafDirectory;
            var destination = FileSystem.DirectoryDescribing(@"d:\another drive");

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Between_FileSystems_Throws()
        {
            var source = ParentOfExistingLeafDirectory;
            var destination = CreateFileSystem().DirectoryDescribing((source.Parent() + RelativeDirectory.FromString("new name")).PathAsString);

            Should.Throw<InvalidOperationException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        // TODO: Access denied (source and destination)
    }
}