using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fs4net.Memory.Node
{
    internal sealed class FolderNode : FileSystemNode
    {
        public List<FileSystemNode> Children { get; private set; }

        private FolderNode()
            : base(null, "root")
        {
            Children = new List<FileSystemNode>();
        }

        private FolderNode(FolderNode parent, string name)
            : base(parent, name)
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
            if (childNode != null) return (FolderNode)childNode;

            return new FolderNode(this, name);
        }

        public FileSystemNode FindChildNodeNamed(string name)
        {
            return Children.FirstOrDefault(node => node.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static FolderNode CreateRoot()
        {
            return new FolderNode();
        }

        public void AddChild(FileSystemNode node)
        {
            Children.RemoveAll(child => child.Name.Equals(node.Name, StringComparison.InvariantCultureIgnoreCase));
            Children.Add(node);
            TouchLastWriteTime();
            TouchLastAccessTime();
        }

        public FileNode CreateFileNode(string filename)
        {
            RemoveNodeIfExists(filename);
            return new FileNode(this, filename);
        }

        public FileNode CreateOrReuseFileNode(string filename)
        {
            var childNode = FindChildNodeNamed(filename);
            if (childNode != null) return (FileNode)childNode;

            return new FileNode(this, filename);
        }

        private void RemoveNodeIfExists(string nodeName)
        {
            var toDelete = Children.Find(node => node.Name.Equals(nodeName, StringComparison.InvariantCultureIgnoreCase));
            if (toDelete != null)
            {
                toDelete.Dispose();
                Children.Remove(toDelete);
            }
        }

        internal void RemoveChild(FileSystemNode nodeToRemove)
        {
            nodeToRemove.VerifyCanBeRemoved();
            nodeToRemove.Dispose();
            if (Children.Remove(nodeToRemove) == false)
                throw new InvalidOperationException(string.Format("Trying to remove a node '{0}' that doesn't exist.", nodeToRemove));
            TouchLastWriteTime();
            TouchLastAccessTime();
        }

        internal void Move(FileSystemNode source, FolderNode destParentNode, string destName)
        {
            Children.Remove(source);
            source.Name = destName;
            destParentNode.AddChild(source);
        }

        public override void VerifyCanBeRemoved()
        {
            Children.ForEach(node => node.VerifyCanBeRemoved());
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