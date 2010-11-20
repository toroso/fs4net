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
        public RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder) { throw new NotImplementedException(); }

        public Drive DriveDescribing(string driveName)
        {
            return new Drive(this, driveName, AssertLogger.Instance);
        }

        #endregion // Implementation of IFileSystem


        #region Implementation of IInternalFileSystem

        public bool IsFile(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public bool IsDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public long GetFileSize(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public DateTime GetFileLastWriteTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetFileLastWriteTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastWriteTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastWriteTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetFileLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetFileLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void CreateDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void DeleteFile(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void DeleteDirectory(RootedCanonicalPath path, bool recursive) { throw new NotImplementedException(); }
        public void MoveFile(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException(); }
        public void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException(); }
        public void CopyFile(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException();}
        public void CopyAndOverwriteFile(RootedCanonicalPath source, RootedCanonicalPath destination) { throw new NotImplementedException(); }
        public Stream CreateReadStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateWriteStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateAppendStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public Stream CreateModifyStream(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetAsCurrent(RootedCanonicalPath path) { throw new NotImplementedException(); }

        #endregion // Implementation of IInternalFileSystem
    }
}