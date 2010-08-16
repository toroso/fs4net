using System;
using fs4net.Framework;
using fs4net.Memory.Builder;

namespace fs4net.Memory.Test
{
    public class PopulatedFileSystem
    {
        protected MemoryFileSystem FileSystem { get; private set; }

        protected readonly RootedDirectory ExistingLeafDirectory;
        protected readonly DateTime ExistingLeafDirectoryLastModified = new DateTime(13243546576879);
        protected readonly RootedDirectory ParentOfExistingLeafDirectory;
        protected readonly RootedDirectory NonExistingDirectory;

        protected readonly RootedFile ExistingFile;
        protected readonly DateTime ExistingFileLastModified = new DateTime(112358132134);
        protected readonly RootedFile NonExistingFile;

        public PopulatedFileSystem()
        {
            FileSystem = new MemoryFileSystem();

            var populateFileSystem = new FileSystemBuilder(FileSystem);
            ExistingLeafDirectory = populateFileSystem.WithDir(@"c:\path\to").LastModifiedAt(ExistingLeafDirectoryLastModified);
            ParentOfExistingLeafDirectory = ExistingLeafDirectory.ParentDirectory();
            NonExistingDirectory = FileSystem.CreateDirectoryDescribing(@"c:\another\path\to");
            ExistingFile = populateFileSystem.WithFile(@"c:\path\to\file.txt").LastModifiedAt(ExistingFileLastModified);
            NonExistingFile = FileSystem.CreateFileDescribing(@"c:\another\path\to\file.txt");
        }
    }
}