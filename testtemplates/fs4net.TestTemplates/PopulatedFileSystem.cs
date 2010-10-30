using System;
using fs4net.Builder;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates
{
    public abstract class PopulatedFileSystem
    {
        protected IFileSystem FileSystem { get; private set; }
        private RootedDirectory _tempDir;

        protected RootedFile ExistingFile { get; private set; }
        protected DateTime ExistingFileLastWriteTime { get { return new DateTime(2010, 08, 20); } }
        protected DateTime ExistingFileLastAccessTime { get { return new DateTime(2005, 09, 07); } }
        protected string ExistingFileContents { get { return "Noviembre"; } }
        protected RootedFile ExistingFile2 { get; private set; }
        protected RootedFile NonExistingFile { get; private set; }

        protected RootedDirectory ExistingLeafDirectory { get; private set; }
        protected DateTime ExistingLeafDirectoryLastWriteTime { get { return new DateTime(1998, 11, 15); } }
        protected DateTime ExistingLeafDirectoryLastAccessTime { get { return new DateTime(2001, 09, 11); } }
        protected RootedDirectory ParentOfExistingLeafDirectory { get; private set; }
        protected DateTime ParentOfExistingLeafDirectoryLastWriteTime { get { return new DateTime(1984, 12, 25); } }
        protected DateTime ParentOfExistingLeafDirectoryLastAccessTime { get { return new DateTime(2004, 06, 12); } }
        protected RootedDirectory ExistingEmptyDirectory { get; private set; }
        protected RootedDirectory NonExistingDirectory { get; private set; }

        protected Drive NonExistingDrive { get; private set; }

        protected readonly DateTime MinimumDate = new DateTime(1601, 1, 1).AddMilliseconds(1).ToLocalTime();
        protected readonly DateTime MaximumDate = DateTime.MaxValue.ToLocalTime();


        [SetUp]
        public void PopulateFileSystem()
        {
            FileSystem = CreateFileSystem();

            _tempDir = FileSystem.DirectoryDescribingTemporaryDirectory() + RelativeDirectory.FromString("PopulatedFileSystem");

            // Note: Must create file system from leaf to root since modifications to an item might modify the parent as well
            var populateFileSystem = new FileSystemBuilder(FileSystem, _tempDir);

            ExistingFile = populateFileSystem
                .WithFile(@"path\to\file.txt")
                .Containing(ExistingFileContents)
                .LastModifiedAt(ExistingFileLastWriteTime)
                .LastAccessedAt(ExistingFileLastAccessTime);
            ExistingFile2 = populateFileSystem
                .WithFile(@"path\to\file2.txt");
            NonExistingFile = FileSystem.FileDescribing(InTemp(@"path\to\another.txt"));

            ExistingLeafDirectory = populateFileSystem
                .WithDir(@"path\to")
                .LastModifiedAt(ExistingLeafDirectoryLastWriteTime)
                .LastAccessedAt(ExistingLeafDirectoryLastAccessTime);
            ParentOfExistingLeafDirectory = populateFileSystem
                .WithDir(@"path")
                .LastModifiedAt(ParentOfExistingLeafDirectoryLastWriteTime)
                .LastAccessedAt(ParentOfExistingLeafDirectoryLastAccessTime);
            ExistingEmptyDirectory = populateFileSystem
                .WithDir(@"my\path");
            NonExistingDirectory = FileSystem.DirectoryDescribing(InTemp(@"another\path\to"));

            NonExistingDrive = FileSystem.DriveDescribing("c:");
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