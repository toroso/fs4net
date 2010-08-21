using System;
using System.Collections.Generic;
using System.Linq;

namespace fs4net.Memory.Node
{
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
            if (childNode != null) return (FolderNode)childNode;

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
            TouchLastModified();
        }

        public FileNode CreateFileNode(string filename)
        {
            return new FileNode(this, filename);
        }

        protected internal void RemoveChild(FileSystemNode nodeToRemove)
        {
            nodeToRemove.Dispose();
            if (_children.Remove(nodeToRemove) == false)
                throw new InvalidOperationException(string.Format("Trying to remove a node '{0}' that doesn't exist.", nodeToRemove));
            TouchLastModified();
        }

        public override string ToString()
        {
            if (Parent == null) return string.Empty;
            return base.ToString() + @"\";
        }
    }
}