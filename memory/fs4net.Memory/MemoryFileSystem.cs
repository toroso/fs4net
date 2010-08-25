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
            return new Drive(this, driveName);
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
            return FindFileNodeByPath(path.FullPath).LastAccessed;
        }

        public void SetFileLastAccessed(RootedCanonicalPath path, DateTime at)
        {
            FindFileNodeByPath(path.FullPath).LastAccessed = at;
        }

        public DateTime GetDirectoryLastAccessed(RootedCanonicalPath path)
        {
            return FindFolderNodeByPath(path.FullPath).LastAccessed;
        }

        public void SetDirectoryLastAccessed(RootedCanonicalPath path, DateTime at)
        {
            FindFolderNodeByPath(path.FullPath).LastAccessed = at;
        }

        public IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path)
        {
            return FindFolderNodeByPath(path.FullPath)
                .Children
                .OfType<FileNode>()
                .Select(child => CreateFileDescribing(child.FullPath));
        }

        public IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path)
        {
            return FindFolderNodeByPath(path.FullPath)
                .Children
                .OfType<FolderNode>()
                .Select(child => CreateDirectoryDescribing(child.FullPath));
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

        public void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination)
        {
            var sourceNode = FindFolderNodeByPath(source.FullPath);

            var destParentNode = _rootNode;
            var parser = new PathParser(destination.FullPath);
            parser.WithEachButLastFileSystemNodeNameDo(folderName => destParentNode = (FolderNode) destParentNode.FindChildNodeNamed(folderName));
            string destName = null;
            parser.WithLastFileSystemNodeNameDo(last => destName = last);

            sourceNode.MoveTo(destParentNode, destName);
        }

        public Stream CreateReadStream(RootedCanonicalPath path)
        {
            return FindFileNodeByPath(path.FullPath).CreateReadStream();
        }

        public Stream CreateWriteStream(RootedCanonicalPath path)
        {
            return CreateFile(path.FullPath).CreateWriteStream();
        }

        public Stream CreateModifyStream(RootedCanonicalPath path)
        {
            return CreateOrReuseFile(path.FullPath).CreateWriteStream();
        }

        #endregion // Implementation of IInternalFileSystem


        public MemoryFileSystem WithDrives(params string[] driveNames)
        {
            Array.ForEach(driveNames, drive => _rootNode.CreateOrReuseFolderNode(drive));
            return this;
        }

        private FileNode CreateFile(string path)
        {
            return CreateFile(path, (parent, filename) => parent.CreateFileNode(filename));
        }

        private FileNode CreateOrReuseFile(string path)
        {
            return CreateFile(path, (parent, filename) => parent.CreateOrReuseFileNode(filename));
        }

        private FileNode CreateFile(string path, Func<FolderNode, string, FileNode> createStrategy)
        {
            FileNode resultNode = null;
            var currentNode = _rootNode;

            var parser = new PathParser(path);
            parser.WithEachButLastFileSystemNodeNameDo(folderName => currentNode = currentNode.CreateOrReuseFolderNode(folderName));
            parser.WithLastFileSystemNodeNameDo(filename => resultNode = createStrategy(currentNode, filename));
            return resultNode;
        }

        private void CreateDirectory(string path)
        {
            FolderNode currentNode = null;

            var parser = new PathParser(path);
            parser.WithFirstFileSystemNodeNameDo(driveName => currentNode = (FolderNode) _rootNode.FindChildNodeNamed(driveName));
            if (currentNode == null) throw new DirectoryNotFoundException(string.Format("Can't create the directory '{0}' since the drive does not exist.", path));
            parser.WithEachButFirstFileSystemNodeNameDo(folderName => currentNode = currentNode.CreateOrReuseFolderNode(folderName));
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

        public override string ToString()
        {
            return _rootNode.TreeAsString(0);
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

        internal void WithEachButFirstFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames.Skip(1))
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

        internal void WithFirstFileSystemNodeNameDo(Action<string> action)
        {
            action(_fileSystemNodeNames.First());
        }

        internal TResult WithLastFileSystemNodeNameDo<TResult>(Func<string, TResult> action)
        {
            return action(_fileSystemNodeNames.Last());
        }
    }
}