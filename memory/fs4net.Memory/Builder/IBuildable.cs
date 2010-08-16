using System;
using fs4net.Framework;

namespace fs4net.Memory.Builder
{
    public interface IBuildable : IFileSystem
    {
        void BuildDirectory(string path);
        void SetLastModified(string path, DateTime at);
    }
}