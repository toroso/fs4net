using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void MoveTo(FolderNode destParentNode, string destName)
        {
            Parent.Move(this, destParentNode, destName);
        }

        private void Move(FolderNode source, FolderNode destParentNode, string destName)
        {
            _children.Remove(source);
            source.Name = destName;
            destParentNode.AddChild(source);
        }

        public override string ToString()
        {
            if (Parent == null) return string.Empty;
            return base.ToString() + @"\";
        }

        public override string TreeAsString(int indentLevel)
        {
            var builder = new StringBuilder();
            builder.Append(base.TreeAsString(indentLevel));
            foreach (var node in _children)
            {
                builder.Append(node.TreeAsString(indentLevel + 1));
            }
            return builder.ToString();
        }
    }
}