using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fs4net.Framework;

namespace fs4net.Memory
{
    public class MemoryFileSystem : IInternalFileSystem, IDisposable
    {
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

    internal abstract class FileSystemNode : IDisposable
    {
        public FolderNode Parent { get; private set; }
        public string Name { get; private set; }
        public DateTime LastModified { get; set; }

        protected FileSystemNode(FolderNode parent, string name)
        {
            Parent = parent;
            Name = name;
            if (Parent != null)
            {
                Parent.AddChild(this);
            }
            LastModified = DateTime.MinValue;
        }

        public abstract void Dispose();
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

        public override void Dispose()
        {
            _children.ForEach(node => node.Dispose());
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
        private readonly NonDisposingStream _content = new NonDisposingStream(new MemoryStream());

        public FileNode(FolderNode parent, string name)
            : base(parent, name)
        {
        }

        public override void Dispose()
        {
            _content.DisposeForReal();
        }

        internal Stream CreateReadStream()
        {
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }

        internal Stream CreateWriteStream()
        {
            _content.Seek(0, SeekOrigin.Begin);
            return _content;
        }
    }

    // Note: This class is really scary and should not be used in production code.
    internal class NonDisposingStream : Stream
    {
        private readonly Stream _inner;

        public NonDisposingStream(Stream inner)
        {
            _inner = inner;
        }

        public void DisposeForReal()
        {
            _inner.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            // Do nothing... that's the charm!
        }

        public override void Flush() { _inner.Flush(); }
        public override long Seek(long offset, SeekOrigin origin) { return _inner.Seek(offset, origin); }
        public override void SetLength(long value) { _inner.SetLength(value); }
        public override int Read(byte[] buffer, int offset, int count) { return _inner.Read(buffer, offset, count); }
        public override void Write(byte[] buffer, int offset, int count) { _inner.Write(buffer, offset, count); }
        public override bool CanRead { get { return _inner.CanRead; } }
        public override bool CanSeek { get { return _inner.CanSeek; } }
        public override bool CanWrite { get { return _inner.CanWrite; } }
        public override long Length { get { return _inner.Length; } }
        public override long Position { get { return _inner.Position; } set { _inner.Position = value; } }
    }
}