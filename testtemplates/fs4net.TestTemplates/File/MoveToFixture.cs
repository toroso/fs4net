using System;
using System.IO;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class MoveToFixture : PopulatedFileSystem
    {
        [Test]
        public void Move_File_To_Same_Directory_But_Different_Name()
        {
            var source = ExistingFile;
            var destination = source.WithFileName(FileName.FromString("new name.dat"));

            source.MoveTo(destination);

            Assert.That(source.Exists(), Is.False);
            Assert.That(destination.Exists(), Is.True);
        }

        [Test]
        public void Move_File_To_Another_Directory()
        {
            var source = ExistingFile;
            var destination = ExistingEmptyDirectory + ExistingFile.FileName();

            source.MoveTo(destination);

            Assert.That(source.Exists(), Is.False);
            Assert.That(destination.Exists(), Is.True);
        }

        [Test]
        public void Move_NonExisting_File_Throws()
        {
            var source = NonExistingFile;
            var destination = ExistingLeafDirectory + FileName.FromString("new name.dat");

            Should.Throw<FileNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_Directory_As_File_Throws()
        {
            var source = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            var destination = ExistingLeafDirectory + RelativeFile.FromString("new name.dat");

            Should.Throw<FileNotFoundException>(() => source.MoveTo(destination));
        }

        [Test]
        public void Move_File_Into_NonExisting_Directory_Throws()
        {
            var source = ExistingFile;
            var destination = NonExistingDirectory + source.FileName();

            Should.Throw<DirectoryNotFoundException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_File_To_Existing_File_Throws()
        {
            var source = ExistingFile;
            var destination = ExistingEmptyFile;

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_File_To_Name_Occupied_By_Existing_Directory_Throws()
        {
            var source = ExistingFile;
            var destination = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_File_To_Itself_Throws()
        {
            // System.IO.File.Move() allows this but not System.IO.Directory.Move()...
            // I prefer to be consequent
            var source = ExistingFile;
            var destination = ExistingFile;

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_File_To_Another_Drive_Throws()
        {
            // System.IO.File.Move() allows this but not System.IO.Directory.Move()...
            // I prefer to be consequent
            var source = ExistingFile;
            var destination = FileSystem.FileDescribing(@"d:\another drive.txt");

            Should.Throw<IOException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Move_Between_FileSystems_Throws()
        {
            var source = ExistingFile;
            var destination = CreateFileSystem().FileDescribing(source.WithFileName(FileName.FromString("new name.dat")).PathAsString);

            Should.Throw<InvalidOperationException>(() => source.MoveTo(destination));
            Assert.That(source.Exists(), Is.True);
        }

        // TODO: Access denied (source and destination)
    }
}