using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a drive. It does not guarantee that the drive exists, but rather exposes methods for operating on
    /// the drive.
    /// </summary>
    public sealed class Drive : IRootedDirectory<Drive>
    {
        private readonly IInternalFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the class on the specified drive. The drive name should be given without an
        /// ending backslash.
        /// </summary>
        /// <param name="fileSystem">The FileSystem with which this descriptor is associated.</param>
        /// <param name="name">A string specifying the drive that the class should encapsulate.</param>
        /// <param name="logger">A logger where to this descriptor reports any abnormalities.</param>
        /// <exception cref="System.IO.PathTooLongException">The specified path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path contains invalid characters,
        /// contains an invalid drive letter, or is invalid in some other way.</exception>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.NonRootedPathException">The specified path is relative or empty.</exception>
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
        /// <exception cref="System.IO.PathTooLongException">The resulting path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        public static RootedDirectory operator +(Drive left, RelativeDirectory right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The resulting path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        public static RootedFile operator +(Drive left, RelativeFile right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">The resulting path, in its canonical form, exceeds
        /// the system-defined maximum length.</exception>
        public static RootedFile operator +(Drive left, FileName right)
        {
            return left.Append(right);
        }

        #endregion // Public Interface


        #region Value Object

        /// <summary>
        /// Determines whether the specified instance denotes the same path as the current instance.
        /// </summary>
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

        /// <summary>
        /// Determines whether the left instance denotes the same path as the right instance.
        /// </summary>
        public static bool operator ==(Drive left, Drive right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the left instance denotes a different path than the right instance.
        /// </summary>
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