using System;
using System.Collections.Generic;
using System.Linq;

namespace fs4net.Framework
{
    public sealed class FileSystem : IInternalFileSystem
    {
        // Cleans the paths before validation.
        private readonly Func<string, string> _pathWasher;

        public FileSystem()
        {
            _pathWasher = (path => path);
        }

        public FileSystem(Func<string, string> pathWasher)
        {
            _pathWasher = pathWasher;
        }

        #region Implementation of IFileSystem

        public RootedFile CreateFileDescribing(string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedFile(this, fullPath, _pathWasher);
        }

        public RootedDirectory CreateDirectoryDescribing(string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedDirectory(this, fullPath, _pathWasher);
        }

        public RootedDirectory CreateDirectoryDescribingTemporaryDirectory()
        {
            return CreateDirectoryDescribing(System.IO.Path.GetTempPath());
        }

        public RootedDirectory CreateDirectoryDescribingCurrentDirectory()
        {
            return CreateDirectoryDescribing(System.IO.Directory.GetCurrentDirectory());
        }

        public Drive CreateDriveDescribing(string driveName)
        {
            return new Drive(this, driveName);
        }

        #endregion // Implementation of IFileSystem


        #region Implementation of IInternalFileSystem

        public bool IsFile(RootedCanonicalPath path)
        {
            return System.IO.File.Exists(path.FullPath);
        }

        public bool IsDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.Exists(path.FullPath);
        }

        public DateTime GetFileLastModified(RootedCanonicalPath path)
        {
            return System.IO.File.GetLastWriteTime(path.FullPath);
        }

        public void SetFileLastModified(RootedCanonicalPath path, DateTime at)
        {
            System.IO.File.SetLastWriteTime(path.FullPath, at);
        }

        public DateTime GetDirectoryLastModified(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetLastWriteTime(path.FullPath);
        }

        public void SetDirectoryLastModified(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastWriteTime(path.FullPath, at);
        }

        public DateTime GetFileLastAccessed(RootedCanonicalPath path)
        {
            return System.IO.File.GetLastAccessTime(path.FullPath);
        }

        public void SetFileLastAccessed(RootedCanonicalPath path, DateTime at)
        {
            System.IO.File.SetLastAccessTime(path.FullPath, at);
        }

        public DateTime GetDirectoryLastAccessed(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetLastAccessTime(path.FullPath);
        }

        public void SetDirectoryLastAccessed(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastAccessTime(path.FullPath, at);
        }

        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetFiles(path.FullPath).Select(filePath => CreateFileDescribing(filePath));
        }

        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetDirectories(path.FullPath).Select(filePath => CreateDirectoryDescribing(filePath));
        }

        public void CreateDirectory(RootedCanonicalPath path)
        {
            System.IO.Directory.CreateDirectory(path.FullPath);
        }

        public void DeleteFile(RootedCanonicalPath path)
        {
            System.IO.File.Delete(path.FullPath);
        }

        public void DeleteDirectory(RootedCanonicalPath path, bool recursive)
        {
            System.IO.Directory.Delete(path.FullPath, recursive);
        }

        public void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            System.IO.Directory.Move(source.FullPath, destination.FullPath);
        }

        public System.IO.Stream CreateReadStream(RootedCanonicalPath path)
        {
            return new System.IO.FileInfo(path.FullPath).Open(System.IO.FileMode.Open, System.IO.FileAccess.Read);
        }

        public System.IO.Stream CreateWriteStream(RootedCanonicalPath path)
        {
            return new System.IO.FileInfo(path.FullPath).Open(System.IO.FileMode.Create, System.IO.FileAccess.Write);
        }

        public System.IO.Stream CreateAppendStream(RootedCanonicalPath path)
        {
            return new System.IO.FileStream(path.FullPath, System.IO.FileMode.Append);
        }

        public System.IO.Stream CreateModifyStream(RootedCanonicalPath path)
        {
            return new System.IO.FileInfo(path.FullPath).Open(System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
        }

        #endregion // Implementation of IInternalFileSystem
    }
}