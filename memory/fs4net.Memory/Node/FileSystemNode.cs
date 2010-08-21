using System;

namespace fs4net.Memory.Node
{
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
            TouchLastModified();
        }

        public void Delete()
        {
            Parent.RemoveChild(this);
        }

        protected void TouchLastModified()
        {
            LastModified = DateTime.Now; // TODO: Get from a mockable clock
        }

        public abstract void Dispose();

        public override string ToString()
        {
            return Parent + Name;
        }
    }
}