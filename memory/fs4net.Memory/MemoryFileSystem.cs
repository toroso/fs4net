using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fs4net.Framework;
using fs4net.Memory.Node;

namespace fs4net.Memory
{
    public class MemoryFileSystem : IInternalFileSystem, IDisposable
    {
        // TODO: Make configurable from test with setter
        private const string TemporaryPathName = @"c:\temp";

        private readonly FolderNode _rootNode = FolderNode.CreateRoot();

        public void Dispose()
        {
            _rootNode.Dispose();
        }
        
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
            return CreateDirectoryDescribing(TemporaryPathName);
        }

        public RootedDirectory CreateDirectoryDescribingCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public Drive CreateDriveDescribing(string driveName)
        {
            throw new NotImplementedException();
        }

        #endregion // Implementation of IFileSystem

        #region Implementation of IInternalFileSystem

        public bool IsFile(RootedCanonicalPath path)
        {
            return (FindFileNodeByPath(path.FullPath) != null);
        }

        public bool IsDirectory(RootedCanonicalPath path)
        {
            return (FindFolderNodeByPath(path.FullPath) != null);
        }

        public DateTime GetFileLastModified(RootedCanonicalPath path)
        {
            return FindFileNodeByPath(path.FullPath).LastModified;
        }

        public void SetFileLastModified(RootedCanonicalPath path, DateTime at)
        {
            FindFileNodeByPath(path.FullPath).LastModified = at;
        }

        public DateTime GetDirectoryLastModified(RootedCanonicalPath path)
        {
            return FindFolderNodeByPath(path.FullPath).LastModified;
        }

        public void SetDirectoryLastModified(RootedCanonicalPath path, DateTime at)
        {
            FindFolderNodeByPath(path.FullPath).LastModified = at;
        }

        public DateTime GetFileLastAccessed(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public void SetFileLastAccessed(RootedCanonicalPath path, DateTime at)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDirectoryLastAccessed(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public void CreateDirectory(RootedCanonicalPath path)
        {
            CreateDirectory(path.FullPath);
        }

        public void DeleteFile(RootedCanonicalPath path)
        {
            // TODO: Tolerant if it doesn't exist.
            FindFileNodeByPath(path.FullPath).Delete();
        }

        public void DeleteDirectory(RootedCanonicalPath path, bool recursive)
        {
            // TODO: Tolerant if it doesn't exist; Support recursive.
            FindFolderNodeByPath(path.FullPath).Delete();
        }

        public Stream CreateReadStream(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public Stream CreateWriteStream(RootedCanonicalPath path)
        {
            return CreateFile(path.FullPath).CreateWriteStream();
        }

        #endregion // Implementation of IInternalFileSystem

        private FileNode CreateFile(string path)
        {
            FileNode resultNode = null;
            var currentNode = _rootNode;

            var parser = new PathParser(path);
            parser.WithEachButLastFileSystemNodeNameDo(folderName => currentNode = currentNode.CreateOrReuseFolderNode(folderName));
            parser.WithLastFileSystemNodeNameDo(filename => resultNode = currentNode.CreateFileNode(filename));
            return resultNode;
        }

        private void CreateDirectory(string path)
        {
            var currentNode = _rootNode;

            var parser = new PathParser(path);
            parser.WithEachFileSystemNodeNameDo(folderName => currentNode = currentNode.CreateOrReuseFolderNode(folderName));
        }

        private FolderNode FindFolderNodeByPath(string path)
        {
            return FindNodeByPath(path) as FolderNode;
        }

        private FileNode FindFileNodeByPath(string path)
        {
            return FindNodeByPath(path) as FileNode;
        }

        private FileSystemNode FindNodeByPath(string path)
        {
            FileSystemNode currentNode = _rootNode;

            var parser = new PathParser(path);
            parser.WithEachFileSystemNodeNameDo(delegate(string folderName)
            {
                if (currentNode != null && currentNode is FolderNode)
                {
                    var folderNode = (FolderNode)currentNode;
                    currentNode = folderNode.FindChildNodeNamed(folderName);
                }
            });

            return currentNode;
        }
    }

    internal class PathParser
    {
        private readonly string[] _fileSystemNodeNames;

        internal PathParser(string path)
        {
            //if (path == string.Empty) throw new ArgumentException("Path is empty", "path");
            _fileSystemNodeNames = ParseFileSystemNodeNames(path);
        }

        private string[] ParseFileSystemNodeNames(string path)
        {
            return path.Split('\\');
        }

        internal void WithEachFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames)
            {
                action(nodeName);
            }
        }

        internal void WithEachButLastFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames.Take(_fileSystemNodeNames.Length - 1))
            {
                action(nodeName);
            }
        }

        internal TResult WithLastFileSystemNodeNameDo<TResult>(Func<string, TResult> action)
        {
            return action(_fileSystemNodeNames.Last());
        }
    }
}