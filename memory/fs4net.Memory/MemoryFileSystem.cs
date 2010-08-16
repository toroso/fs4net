using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fs4net.Framework;
using fs4net.Memory.Builder;

namespace fs4net.Memory
{
    public class MemoryFileSystem : IInternalFileSystem, IBuildable
    {
        private readonly FolderNode _rootNode = FolderNode.CreateRoot();

        
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
            throw new NotImplementedException();
        }

        public DateTime GetDirectoryLastModified(RootedCanonicalPath path)
        {
            return FindFolderNodeByPath(path.FullPath).LastModified;
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
            throw new NotImplementedException();
        }

        public void DeleteDirectory(RootedCanonicalPath path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public Stream CreateReadStream(RootedCanonicalPath path)
        {
            throw new NotImplementedException();
        }

        public Stream CreateWriteStream(RootedCanonicalPath path)
        {
            var currentNode = _rootNode;

            var parser = new PathParser(path.FullPath);
            parser.WithEachButLastFileSystemNodeNameDo(folderName => currentNode = currentNode.CreateOrReuseFolderNode(folderName));
            parser.WithLastFileSystemNodeNameDo(filename => currentNode.CreateFileNode(filename));
            return null;
        }

        #endregion // Implementation of IInternalFileSystem

        #region Implementation of IBuildable

        public void BuildDirectory(string path)
        {
            CreateDirectory(path);
        }

        public void SetLastModified(string path, DateTime at)
        {
            FindNodeByPath(path).LastModified = at;
        }

        #endregion

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

    internal class FileSystemNode
    {
        public FolderNode Parent { get; private set; }
        public string Name { get; private set; }
        public DateTime LastModified { get; set; }

        public FileSystemNode(FolderNode parent, string name)
        {
            Parent = parent;
            Name = name;
            if (Parent != null)
            {
                Parent.AddChild(this);
            }
            LastModified = DateTime.MinValue;
        }
    }

    internal class FolderNode : FileSystemNode
    {
        private readonly List<FileSystemNode> _children = new List<FileSystemNode>();

        public FolderNode(FolderNode parent, string name)
            : base(parent, name)
        {
        }

        private FolderNode()
            : base(null, "root")
        {
        }

        public FolderNode CreateOrReuseFolderNode(string name)
        {
            var childNode = FindChildNodeNamed(name);
            // TODO: if (childNode is FileNode) throw...
            if (childNode != null) return (FolderNode) childNode;

            return new FolderNode(this, name);
        }

        public FileSystemNode FindChildNodeNamed(string name)
        {
            return _children.FirstOrDefault(node => node.Name == name);
        }

        public static FolderNode CreateRoot()
        {
            return new FolderNode();
        }

        public void AddChild(FileSystemNode node)
        {
            _children.Add(node);
        }

        public FileNode CreateFileNode(string filename)
        {
            return new FileNode(this, filename);
        }
    }

    internal class FileNode : FileSystemNode
    {
        public FileNode(FolderNode parent, string name)
            : base(parent, name)
        {
        }
    }
}