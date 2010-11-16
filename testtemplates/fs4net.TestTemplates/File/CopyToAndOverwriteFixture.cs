using System;
using System.IO;
using fs4net.Framework;
using fs4net.Framework.Utils;
using NUnit.Framework;

namespace fs4net.TestTemplates.File
{
    public abstract class CopyToAndOverwriteFixture : PopulatedFileSystem
    {
        [Test]
        public void Copy_File_To_Same_Directory_But_Different_Name()
        {
            var source = ExistingFile;
            var destination = source.WithFileName(FileName.FromString("new name.dat"));

            source.CopyToAndOverwrite(destination);

            Assert.That(source.Exists(), Is.True);
            Assert.That(destination.Exists(), Is.True);
            Assert.That(source.ReadText(), Is.EqualTo(destination.ReadText()));
        }

        [Test]
        public void Copy_File_To_Another_Directory()
        {
            var source = ExistingFile;
            var destination = ExistingEmptyDirectory + ExistingFile.FileName();

            source.CopyToAndOverwrite(destination);

            Assert.That(source.Exists(), Is.True);
            Assert.That(destination.Exists(), Is.True);
            Assert.That(source.ReadText(), Is.EqualTo(destination.ReadText()));
        }

        [Test]
        [Ignore("Fails if you don't have a writable d: drive")]
        public void Copy_File_To_Another_Drive()
        {
            // IF THIS TEST FAILS:
            // Unfortunately, this test assumes that you have a writable d: drive. If not, it will fail.
            var source = ExistingFile;
            var destination = FileSystem.FileDescribing(@"d:\another drive.txt");

            try
            {
                source.CopyToAndOverwrite(destination);

                Assert.That(source.Exists(), Is.True);
                Assert.That(destination.Exists(), Is.True);
                Assert.That(source.ReadText(), Is.EqualTo(destination.ReadText()));
            }
            finally
            {
                destination.TryDelete();
            }
        }

        [Test]
        public void Copy_NonExisting_File_Throws()
        {
            var source = NonExistingFile;
            var destination = ExistingLeafDirectory + FileName.FromString("new name.dat");

            Assert.Throws<FileNotFoundException>(() => source.CopyToAndOverwrite(destination));
        }

        [Test]
        public void Copy_Directory_As_File_Throws()
        {
            var source = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);
            var destination = ExistingLeafDirectory + RelativeFile.FromString("new name.dat");

            Assert.Throws<UnauthorizedAccessException>(() => source.CopyToAndOverwrite(destination));
        }

        [Test]
        public void Copy_File_Into_NonExisting_Directory_Throws()
        {
            var source = ExistingFile;
            var destination = NonExistingDirectory + source.FileName();

            Assert.Throws<DirectoryNotFoundException>(() => source.CopyToAndOverwrite(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Copy_File_To_Existing_File_Overwrites()
        {
            var source = ExistingFile;
            var destination = ExistingFile2;

            source.CopyToAndOverwrite(destination);
            Assert.That(source.Exists(), Is.True);
            Assert.That(destination.Exists(), Is.True);
            Assert.That(source.ReadText(), Is.EqualTo(destination.ReadText()));
        }

        [Test]
        public void Copy_File_To_Name_Occupied_By_Existing_Directory_Throws()
        {
            var source = ExistingFile;
            var destination = FileSystem.FileDescribing(ExistingEmptyDirectory.PathAsString);

            Assert.Throws<IOException>(() => source.CopyToAndOverwrite(destination));
            Assert.That(source.Exists(), Is.True);
            Assert.That(ExistingEmptyDirectory.Exists(), Is.True);
        }

        [Test]
        public void Copy_File_To_Itself_Throws()
        {
            // Throw to be consistant with MoveTo()...
            var source = ExistingFile;
            var destination = ExistingFile;

            Assert.Throws<IOException>(() => source.CopyToAndOverwrite(destination));
            Assert.That(source.Exists(), Is.True);
        }

        [Test]
        public void Copy_Between_FileSystems_Throws()
        {
            var source = ExistingFile;
            var destination = CreateFileSystem().FileDescribing(source.WithFileName(FileName.FromString("new name.dat")).PathAsString);

            Assert.Throws<InvalidOperationException>(() => source.CopyToAndOverwrite(destination));
            Assert.That(source.Exists(), Is.True);
        }

        // TODO: Access denied (source and destination)
    }
}