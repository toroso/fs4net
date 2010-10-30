using System;
using System.Text;

namespace fs4net.Memory.Node
{
    internal abstract class FileSystemNode : IDisposable
    {
        protected FolderNode Parent { get; private set; }
        public string Name { get; protected internal set; }
        public DateTime LastWriteTime { get; set; }
        public DateTime LastAccessTime { get; set; }

        protected FileSystemNode(FolderNode parent, string name)
        {
            Parent = parent;
            Name = name;
            if (Parent != null)
            {
                Parent.AddChild(this);
            }
            TouchLastWriteTime();
            TouchLastAccessTime();
        }

        public abstract void VerifyCanBeRemoved();

        public string FullPath
        {
            get
            {
                if (Parent == null) return string.Empty;

                var builder = new StringBuilder();
                builder.Append(Parent.FullPath);
                if (Parent.FullPath.Length > 0) builder.Append(@"\");
                builder.Append(Name);
                return builder.ToString();
            }
        }

        public void Delete()
        {
            Parent.RemoveChild(this);
        }

        public void MoveTo(FolderNode destParentNode, string destName)
        {
            Parent.Move(this, destParentNode, destName);
        }

        protected void TouchLastWriteTime()
        {
            LastWriteTime = DateTime.Now; // TODO: Get from a mockable clock
        }

        protected void TouchLastAccessTime()
        {
            LastAccessTime = DateTime.Now; // TODO: Get from a mockable clock
        }

        public abstract void Dispose();

        public override string ToString()
        {
            return FullPath;
        }

        public virtual string TreeAsString(int indentLevel)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < indentLevel; i++)
            {
                builder.Append(' ');
            }
            builder.AppendLine(Name);
            return builder.ToString();
        }
    }
}