using System;
using fs4net.Framework;

namespace fs4net.CommonTest.Builder
{
    public class RootedDirectoryBuilder : FileSystemItemBuilder<RootedDirectoryBuilder>
    {
        private readonly RootedDirectory _dir;

        public RootedDirectoryBuilder(RootedDirectory dir)
        {
            _dir = dir;
            _dir.Create();
        }

        protected override RootedDirectoryBuilder Me()
        {
            return this;
        }

        public static implicit operator RootedDirectory (RootedDirectoryBuilder me)
        {
            return me._dir;
        }

        protected override DateTime LastAccessed { set { _dir.SetLastAccessed(value); } }
        protected override DateTime LastModified { set { _dir.SetLastModified(value); } }
    }
}