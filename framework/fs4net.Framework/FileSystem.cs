using System;
using System.Collections.Generic;
using System.Linq;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public sealed class FileSystem : IInternalFileSystem
    {
        // Cleans the paths before validation.
        private readonly Func<string, string> _pathWasher;
        private readonly ILogger _logger;

        public FileSystem()
            : this(NullLogger.Instance, PathWashers.NullWasher)
        {
        }

        /// <param name="logger">Anything worth reporting inside the fs4net classes are sent to this logger instance.</param>
        public FileSystem(ILogger logger)
            : this(logger, PathWashers.NullWasher)
        {
        }

        /// <param name="pathWasher">All paths are cleaned with this PathWasher before the FileSystemItems are created.</param>
        public FileSystem(Func<string, string> pathWasher)
            : this(NullLogger.Instance, pathWasher)
        {
        }

        /// <param name="logger">Anything worth reporting inside the fs4net classes are sent to this logger instance.</param>
        /// <param name="pathWasher">All paths are cleaned with this PathWasher before the FileSystemItems are created.</param>
        public FileSystem(ILogger logger, Func<string, string> pathWasher)
        {
            _pathWasher = pathWasher;
            _logger = logger;
        }

        #region Implementation of IFileSystem

        public RootedFile FileDescribing(string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedFile(this, fullPath, _pathWasher, _logger);
        }

        public RootedDirectory DirectoryDescribing(string fullPath)
        {
            // TODO: If relative, append it to Current Directory. Or not...?
            return new RootedDirectory(this, fullPath, _pathWasher, _logger);
        }

        public RootedDirectory DirectoryDescribingTemporaryDirectory()
        {
            var tempPathWithBackslash = System.IO.Path.GetTempPath();
            return DirectoryDescribing(tempPathWithBackslash.Remove(tempPathWithBackslash.Length - 1));
        }

        public RootedDirectory DirectoryDescribingCurrentDirectory()
        {
            return DirectoryDescribing(System.IO.Directory.GetCurrentDirectory());
        }

        public RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder)
        {
            if (folder == Environment.SpecialFolder.MyComputer) throw new NotSupportedException("MyComputer cannot be denoted by a RootedDirectory.");
            return DirectoryDescribing(Environment.GetFolderPath(folder));
        }

        public Drive DriveDescribing(string driveName)
        {
            return new Drive(this, driveName, _logger);
        }

        public IEnumerable<Drive> AllDrives()
        {
            return System.IO.DriveInfo.GetDrives()
                .Select(driveInfo => DriveDescribing(driveInfo.Name.RemoveTrailingPathSeparators()));
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

        public long GetFileSize(RootedCanonicalPath path)
        {
            return new System.IO.FileInfo(path.FullPath).Length;
        }

        public DateTime GetFileLastWriteTime(RootedCanonicalPath path)
        {
            return System.IO.File.GetLastWriteTime(path.FullPath);
        }

        public void SetFileLastWriteTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.File.SetLastWriteTime(path.FullPath, at);
        }

        public DateTime GetDirectoryLastWriteTime(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetLastWriteTime(path.FullPath);
        }

        public void SetDirectoryLastWriteTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastWriteTime(path.FullPath, at);
        }

        public DateTime GetFileLastAccessTime(RootedCanonicalPath path)
        {
            return System.IO.File.GetLastAccessTime(path.FullPath);
        }

        public void SetFileLastAccessTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.File.SetLastAccessTime(path.FullPath, at);
        }

        public DateTime GetDirectoryLastAccessTime(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetLastAccessTime(path.FullPath);
        }

        public void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastAccessTime(path.FullPath, at);
        }

        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetFiles(path.FullPath).Select(filePath => FileDescribing(filePath));
        }

        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetDirectories(path.FullPath).Select(filePath => DirectoryDescribing(filePath));
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

        public void MoveFile(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            System.IO.File.Move(source.FullPath, destination.FullPath);
        }

        public void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            System.IO.Directory.Move(source.FullPath, destination.FullPath);
        }

        public void CopyFile(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            System.IO.File.Copy(source.FullPath, destination.FullPath);
        }

        public void CopyAndOverwriteFile(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            System.IO.File.Copy(source.FullPath, destination.FullPath, true);
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

        public void SetAsCurrent(RootedCanonicalPath path)
        {
            System.IO.Directory.SetCurrentDirectory(path.FullPath);
        }

        #endregion // Implementation of IInternalFileSystem
    }
}