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


        #region Value Object

        public bool Equals(Drive other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._fileSystem, _fileSystem) && Equals(other._name, _name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Drive)) return false;
            return Equals((Drive) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_fileSystem.GetHashCode()*397) ^ _name.GetHashCode();
            }
        }

        public static bool operator ==(Drive left, Drive right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Drive left, Drive right)
        {
            return !Equals(left, right);
        }

        #endregion // Value Object


        #region Debugging

        public override string ToString()
        {
            return PathAsString;
        }

        #endregion Debugging
    }
}