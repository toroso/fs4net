using System;

namespace fs4net.Framework
{
    internal class FileSystem : IInternalFileSystem
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

        public DateTime GetDirectoryLastModified(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetLastWriteTime(path.FullPath);
        }

        public void DeleteFile(RootedCanonicalPath path)
        {
            System.IO.File.Delete(path.FullPath);
        }

        public void DeleteDirectory(RootedCanonicalPath path)
        {
            System.IO.Directory.Delete(path.FullPath);
        }

        public System.IO.Stream CreateReadStream(RootedCanonicalPath path)
        {
            return new System.IO.FileStream(path.FullPath, System.IO.FileMode.Open);
        }

        public System.IO.Stream CreateWriteStream(RootedCanonicalPath path)
        {
            return new System.IO.FileStream(path.FullPath, System.IO.FileMode.Create);
        }

        #endregion // Implementation of IInternalFileSystem
    }
}