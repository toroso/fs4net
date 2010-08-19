using System;
using fs4net.Framework;
using fs4net.Memory.Builder;
using NUnit.Framework;

namespace fs4net.Memory.Test
{
    public class PopulatedFileSystem
    {
        protected MemoryFileSystem FileSystem { get; private set; }

        protected RootedDirectory ExistingLeafDirectory { get; private set; }
        protected DateTime ExistingLeafDirectoryLastModified { get { return new DateTime(13243546576879); } }
        protected RootedDirectory ParentOfExistingLeafDirectory { get; private set; }
        protected RootedDirectory NonExistingDirectory { get; private set; }

        protected RootedFile ExistingFile { get; private set; }
        protected DateTime ExistingFileLastModified { get { return new DateTime(112358132134); } }
        protected RootedFile NonExistingFile { get; private set; }

        [TestFixtureSetUp]
        public void PopulateFileSystem()
        {
            FileSystem = new MemoryFileSystem();

            var populateFileSystem = new FileSystemBuilder(FileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to").LastModifiedAt(ExistingLeafDirectoryLastModified);
            ParentOfExistingLeafDirectory = ExistingLeafDirectory.ParentDirectory();
            NonExistingDirectory = FileSystem.CreateDirectoryDescribing(@"c:\another\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt").LastModifiedAt(ExistingFileLastModified);
            NonExistingFile = FileSystem.CreateFileDescribing(@"c:\another\path\to\file.txt");
        }

        [TestFixtureTearDown]
        public void TearDownFileSystem()
        {
            FileSystem.Dispose();
        }
    }
}