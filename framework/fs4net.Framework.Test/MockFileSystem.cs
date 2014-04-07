using System;
using System.Collections.Generic;
using System.IO;
using fs4net.TestTemplates;

namespace fs4net.Framework.Test
{
    internal class MockFileSystem : IFileSystem, IInternalFileSystem
    {
        private string _currentDirectory;

        public IInternalFileSystem InternalFileSystem
        {
            get { return this; }
        }

        public ILogger Logger
        {
            get { return AssertLogger.Instance; }
        }

        public RootedFile FileDescribing(string fullPath)
        {
            return FileSystemExtensions.FileDescribing(this, fullPath);
        }

        public RootedDirectory DirectoryDescribing(string fullPath)
        {
            return FileSystemExtensions.DirectoryDescribing(this, fullPath);
        }

        public Drive DriveDescribing(string driveName)
        {
            return FileSystemExtensions.DriveDescribing(this, driveName);
        }

        public RootedFile FileFromCurrentDirectory(string path)
        {
            return FileSystemExtensions.FileFromCurrentDirectory(this, path);
        }

        public RootedDirectory DirectoryFromCurrentDirectory(string path)
        {
            return FileSystemExtensions.DirectoryFromCurrentDirectory(this, path);
        }

        public RootedDirectory DirectoryDescribingTemporaryDirectory() { throw new NotImplementedException(); }

        public RootedDirectory DirectoryDescribingCurrentDirectory()
        {
            return DirectoryDescribing(_currentDirectory);
        }

        public RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder) { throw new NotImplementedException(); }

        public IEnumerable<Drive> AllDrives() { throw new NotImplementedException(); }

        public bool IsFile(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public bool IsDirectory(RootedCanonicalPath path) { return true; }
        public long GetFileSize(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public DateTime GetFileLastWriteTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetFileLastWriteTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastWriteTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastWriteTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetFileLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetFileLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public DateTime GetDirectoryLastAccessTime(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at) { throw new NotImplementedException(); }
        public IEnumerable<string> GetFilesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
        public IEnumerable<string> GetDirectoriesInDirectory(RootedCanonicalPath path) { throw new NotImplementedException(); }
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

        public string GetCurrentDirectory()
        {
            return _currentDirectory;
        }

        public void SetCurrentDirectory(RootedCanonicalPath path)
        {
            _currentDirectory = path.FullPath;
        }
    }
}