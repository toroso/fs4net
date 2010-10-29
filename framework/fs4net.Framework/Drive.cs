using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public sealed class Drive : IRootedDirectory<Drive>
    {
        private readonly IInternalFileSystem _fileSystem;

        public Drive(IInternalFileSystem fileSystem, string name, ILogger logger)
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            Name = name;
            Logger = logger;
            new CanonicalPathBuilder(Name).BuildForDrive();
        }

        #region Public Interface

        public string Name { get; private set; }

        public string PathAsString
        {
            get { return Name; }
        }

        public ILogger Logger { get; private set; }

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
        public static RootedDirectory operator +(Drive left, RelativeDirectory right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(Drive left, RelativeFile right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(Drive left, FileName right)
        {
            return left.Append(right);
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