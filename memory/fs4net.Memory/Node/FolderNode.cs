using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fs4net.Memory.Node
{
    internal class FolderNode : FileSystemNode
    {
        public List<FileSystemNode> Children { get; private set; }

        public FolderNode(FolderNode parent, string name)
            : base(parent, name)
        {
            Children = new List<FileSystemNode>();
        }

        private FolderNode()
            : base(null, "root")
        {
            Children = new List<FileSystemNode>();
        }

        public override void Dispose()
        {
            Children.ForEach(node => node.Dispose());
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
            return Children.FirstOrDefault(node => node.Name == name);
        }

        public static FolderNode CreateRoot()
        {
            return new FolderNode();
        }

        public void AddChild(FileSystemNode node)
        {
            Children.Add(node);
            TouchLastModified();
        }

        public FileNode CreateFileNode(string filename)
        {
            return new FileNode(this, filename);
        }

        protected internal void RemoveChild(FileSystemNode nodeToRemove)
        {
            nodeToRemove.Dispose();
            if (Children.Remove(nodeToRemove) == false)
                throw new InvalidOperationException(string.Format("Trying to remove a node '{0}' that doesn't exist.", nodeToRemove));
            TouchLastModified();
        }

        public void MoveTo(FolderNode destParentNode, string destName)
        {
            Parent.Move(this, destParentNode, destName);
        }

        private void Move(FolderNode source, FolderNode destParentNode, string destName)
        {
            Children.Remove(source);
            source.Name = destName;
            destParentNode.AddChild(source);
        }

        public override string TreeAsString(int indentLevel)
        {
            var builder = new StringBuilder();
            builder.Append(base.TreeAsString(indentLevel));
            foreach (var node in Children)
            {
                builder.Append(node.TreeAsString(indentLevel + 1));
            }
            return builder.ToString();
        }
    }
}