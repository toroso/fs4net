using System;
using System.IO;

namespace fs4net.Framework.Test
{
    internal class MockFileSystem : IInternalFileSystem
    {
        #region Implementation of IFileSystem

        public RootedFile CreateFileDescribing(string fullPath)
        {
            return new RootedFile(this, fullPath, PathWashers.NullWasher);
        }

        public RootedDirectory CreateDirectoryDescribing(string fullPath)
        {
            return new RootedDirectory(this, fullPath, PathWashers.NullWasher);
        }

        public RootedDirectory CreateDirectoryDescribingTemporaryDirectory()
        {
            throw new NotImplementedException();
        }

        public RootedDirectory CreateDirectoryDescribingCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public Drive CreateDriveDescribing(string driveName)
        {
            return new Drive(this, driveName);
        }

        #endregion // Implementation of IFileSystem


        #region Implementation of IInternalFileSystem

        public bool IsFile(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public bool IsDirectory(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public DateTime GetFileLastModified(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDirectoryLastModified(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public Stream CreateReadStream(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public Stream CreateWriteStream(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        #endregion // Implementation of IInternalFileSystem
    }
}