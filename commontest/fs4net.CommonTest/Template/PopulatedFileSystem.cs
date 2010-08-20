using System;
using fs4net.CommonTest.Builder;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template
{
    public abstract class PopulatedFileSystem
    {
        protected IFileSystem FileSystem { get; private set; }
        private RootedDirectory _tempDir;

        protected RootedFile ExistingFile { get; private set; }
        protected DateTime ExistingFileLastModified { get { return new DateTime(2010, 08, 20, 18, 37, 00); } }
        protected RootedFile NonExistingFile { get; private set; }

        protected RootedDirectory ExistingLeafDirectory { get; private set; }
        protected DateTime ExistingLeafDirectoryLastModified { get { return new DateTime(1998, 11, 15, 11, 15, 00); } }
        protected RootedDirectory ParentOfExistingLeafDirectory { get; private set; }
        protected RootedDirectory NonExistingDirectory { get; private set; }

        [TestFixtureSetUp]
        public void PopulateFileSystem()
        {
            FileSystem = CreateFileSystem();

            _tempDir = FileSystem.CreateDirectoryDescribingTemporaryDirectory() + RelativeDirectory.FromString("PopulatedFileSystem");

            // Note: Must create file system from leaf to root since modifications to an item might modify the parent as well
            var populateFileSystem = new FileSystemBuilder(FileSystem, _tempDir);

            ExistingFile = populateFileSystem
                .WithFile(@"path\to\file.txt")
                .LastModifiedAt(ExistingFileLastModified);
            NonExistingFile = FileSystem.CreateFileDescribing(InTemp(@"another\path\to\file.txt"));

            ExistingLeafDirectory = populateFileSystem
                .WithDir(@"path\to")
                .LastModifiedAt(ExistingLeafDirectoryLastModified);
            ParentOfExistingLeafDirectory = ExistingLeafDirectory.ParentDirectory();
            NonExistingDirectory = FileSystem.CreateDirectoryDescribing(InTemp(@"another\path\to"));
        }

        private string InTemp(string relativePath)
        {
            return (_tempDir + RelativeDirectory.FromString(relativePath)).PathAsString;
        }

        [TestFixtureTearDown]
        public void TearDownFileSystem()
        {
            _tempDir.DeleteRecursively();
            DisposeFileSystem();
        }

        protected abstract IFileSystem CreateFileSystem();
        protected virtual void DisposeFileSystem() { }
    }
}