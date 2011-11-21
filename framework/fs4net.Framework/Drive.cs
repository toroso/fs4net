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

        /// <summary>
        /// Returns the drive name on the same same format as it was created with.
        /// This property succeeds whether the file exists or not.
        /// </summary>
        public string Name { get; private set; }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Returns the drive name as a string on the same same format as it was created with.
        /// This property succeeds whether the file exists or not.
        /// </summary>
        public string PathAsString
        {
            get { return Name; }
        }

        public ILogger Logger { get; private set; }

        public Drive AsCanonical()
        {
            return this;
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

    internal static class DriveVerifications
    {
        internal static void VerifyExists(this Drive me, Func<Exception> createException)
        {
            if (!me.Exists())
            {
                throw createException();
            }
        }
    }
}