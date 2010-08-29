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

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedDirectory operator +(Drive lhs, RelativeDirectory rhs)
        {
            return lhs.Append(rhs);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(Drive lhs, RelativeFile rhs)
        {
            return lhs.Append(rhs);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(Drive lhs, FileName rhs)
        {
            return lhs.Append(rhs);
        }

        #endregion // Public Interface


        #region Value Object

        public bool Equals<T>(IRootedDirectory<T> other) where T : IRootedDirectory<T>
        {
            return this.DenotesSamePathAs(other);
        }

        public override bool Equals(object obj)
        {
            return this.DenotesSamePathAs(obj);
        }

        public override int GetHashCode()
        {
            return this.InternalGetHashCode();
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