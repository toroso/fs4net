using System;
using System.Collections.Generic;

namespace fs4net.Framework.Impl
{
    internal sealed class FileSystemImpl : IInternalFileSystem
    {
        private string _currentDirectory;

        public FileSystemImpl()
        {
            _currentDirectory = System.IO.Directory.GetCurrentDirectory().RemoveEndingBackslash();
        }

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
            return System.IO.Directory.GetLastWriteTime(FullPathWithBackslashOnDrives(path.FullPath));
        }

        public void SetDirectoryLastWriteTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastWriteTime(FullPathWithBackslashOnDrives(path.FullPath), at);
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
            return System.IO.Directory.GetLastAccessTime(FullPathWithBackslashOnDrives(path.FullPath));
        }

        public void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetLastAccessTime(FullPathWithBackslashOnDrives(path.FullPath), at);
        }

        public DateTime GetDirectoryCreationTime(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetCreationTime(FullPathWithBackslashOnDrives(path.FullPath));
        }

        public void SetDirectoryCreationTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.Directory.SetCreationTime(FullPathWithBackslashOnDrives(path.FullPath), at);
        }

        public DateTime GetFileCreationTime(RootedCanonicalPath path)
        {
            return System.IO.File.GetCreationTime(path.FullPath);
        }

        public void SetFileCreationTime(RootedCanonicalPath path, DateTime at)
        {
            System.IO.File.SetCreationTime(path.FullPath, at);
        }

        public IEnumerable<string> GetFilesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetFiles(FullPathWithBackslashOnDrives(path.FullPath));
        }

        public IEnumerable<string> GetDirectoriesInDirectory(RootedCanonicalPath path)
        {
            return System.IO.Directory.GetDirectories(FullPathWithBackslashOnDrives(path.FullPath));
        }

        private static string FullPathWithBackslashOnDrives(string fullPath)
        {
            return fullPath.EndsWith(":")
                ? string.Format("{0}\\", fullPath) // Handle drives
                : fullPath;
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
            return new System.IO.FileInfo(path.FullPath).Open(System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
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