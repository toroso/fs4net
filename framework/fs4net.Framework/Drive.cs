using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public class Drive : IRootedDirectory<Drive>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _name;

        public Drive(IInternalFileSystem fileSystem, string name) // TODO: public only for unit tests... necessary? Use InternalsVisibleTo attribute?
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            _name = name;
            new CanonicalPathBuilder(name).BuildForDrive();
        }

        #region Public Interface

        public string Name
        {
            get { return _name; }
        }

        public string PathAsString
        {
            get { return _name; }
        }

        public Func<string, string> PathWasher
        {
            get { return PathWashers.NullWasher; }
        }

        public Drive AsCanonical()
        {
            return this;
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        #endregion // Public Interface
    }
}