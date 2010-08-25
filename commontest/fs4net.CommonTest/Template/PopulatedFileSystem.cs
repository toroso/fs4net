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
        protected DateTime ExistingFileLastModified { get { return new DateTime(2010, 08, 20); } }
        protected string ExistingFileContents { get { return "Septiembre"; } }
        protected RootedFile NonExistingFile { get; private set; }

        protected RootedDirectory ExistingLeafDirectory { get; private set; }
        protected DateTime ExistingLeafDirectoryLastModified { get { return new DateTime(1998, 11, 15); } }
        protected RootedDirectory ParentOfExistingLeafDirectory { get; private set; }
        protected DateTime ParentOfExistingLeafDirectoryLastModified { get { return new DateTime(1984, 12, 25); } }
        protected RootedDirectory ExistingEmptyDirectory { get; private set; }
        protected RootedDirectory NonExistingDirectory { get; private set; }

        protected Drive NonExistingDrive { get; private set; }

        protected readonly DateTime MinimumDate = new DateTime(1601, 1, 1).AddMilliseconds(1).ToLocalTime();
        protected readonly DateTime MaximumDate = DateTime.MaxValue.ToLocalTime();


        [SetUp]
        public void PopulateFileSystem()
        {
            FileSystem = CreateFileSystem();

            _tempDir = FileSystem.CreateDirectoryDescribingTemporaryDirectory() + RelativeDirectory.FromString("PopulatedFileSystem");

            // Note: Must create file system from leaf to root since modifications to an item might modify the parent as well
            var populateFileSystem = new FileSystemBuilder(FileSystem, _tempDir);

            ExistingFile = populateFileSystem
                .WithFile(@"path\to\file.txt")
                .Containing(ExistingFileContents)
                .LastModifiedAt(ExistingFileLastModified);
            NonExistingFile = FileSystem.CreateFileDescribing(InTemp(@"path\to\another.txt"));

            ExistingLeafDirectory = populateFileSystem
                .WithDir(@"path\to")
                .LastModifiedAt(ExistingLeafDirectoryLastModified);
            ParentOfExistingLeafDirectory = populateFileSystem
                .WithDir(@"path")
                .LastModifiedAt(ParentOfExistingLeafDirectoryLastModified);
            ExistingEmptyDirectory = populateFileSystem
                .WithDir(@"my\path");
            NonExistingDirectory = FileSystem.CreateDirectoryDescribing(InTemp(@"another\path\to"));

            NonExistingDrive = FileSystem.CreateDriveDescribing("c:");
        }

        private string InTemp(string relativePath)
        {
            return (_tempDir + RelativeDirectory.FromString(relativePath)).PathAsString;
        }

        [TearDown]
        public void TearDownFileSystem()
        {
            _tempDir.DeleteRecursively();
            DisposeFileSystem(FileSystem);
        }

        protected abstract IFileSystem CreateFileSystem();
        protected virtual void DisposeFileSystem(IFileSystem fileSystem) { }
    }
}