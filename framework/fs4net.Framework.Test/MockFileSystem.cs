using System;
using System.Collections.Generic;
using System.IO;
using fs4net.TestTemplates;

namespace fs4net.Framework.Test
{
    internal class MockFileSystem : IInternalFileSystem
    {
        #region Implementation of IFileSystem

        public RootedFile FileDescribing(string fullPath)
        {
            return new RootedFile(this, fullPath, PathWashers.NullWasher, AssertLogger.Instance);
        }

        public RootedDirectory DirectoryDescribing(string fullPath)
        {
            return new RootedDirectory(this, fullPath, PathWashers.NullWasher, AssertLogger.Instance);
        }

        public RootedDirectory DirectoryDescribingTemporaryDirectory() { throw new NotImplementedException(); }
        public RootedDirectory DirectoryDescribingCurrentDirectory() { throw new NotImplementedException(); }

        public Drive DriveDescribing(string driveName)
        {
            return new Drive(this, driveName, AssertLogger.Instance);
        }

        #endregion // Implementation of IFileSystem


        #region Implementation of IInternalFileSystem

        public bool IsFile(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public bool IsDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public DateTime GetFileLastModified(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetFileLastModified(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastModified(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastModified(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void CreateDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void DeleteFile(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void DeleteDirectory(RootedCanonicalPath path, bool recursive) { throw new NotImplementedException(); }
        public void MoveFile(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException(); }
        public void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException(); }
        public Stream CreateReadStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateWriteStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateAppendStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateModifyStream(RootedCanonicalPath path) { throw new NotImplementedException(); }

        #endregion // Implementation of IInternalFileSystem
    }
}