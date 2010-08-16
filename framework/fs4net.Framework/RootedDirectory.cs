using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public class RootedDirectory : IDirectory<RootedDirectory>, IRootedDirectory<RootedDirectory>
    {
        private readonly IInternalFileSystem _fileSystem;
        private readonly string _rootedPath;
        private readonly Func<string, string> _pathWasher;
        private readonly string _canonicalFullPath;

        public RootedDirectory(IInternalFileSystem fileSystem, string rootedPath, Func<string, string> pathWasher) // TODO: public only for unit tests... necessary? Use InternalsVisibleTo attribute?
        {
            ThrowHelper.ThrowIfNull(fileSystem, "fileSystem");
            _fileSystem = fileSystem;
            _rootedPath = pathWasher(rootedPath);
            _pathWasher = pathWasher;
            _canonicalFullPath = new CanonicalPathBuilder(_rootedPath).BuildForRootedDirectory();
        }


        #region Public Interface

        /// <summary>
        /// Returns the FileSystem with which this descriptor is associated.
        /// </summary>
        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        /// <summary>
        /// Returns the path that this descriptor represent as a string. It is returned on the same format as it was
        /// created with. This means that it can contain redundant parts such as ".", ".." inside paths and multiple
        /// "\". To remove such redundant parts, use the AsCanonical() factory method.
        /// This property succeeds whether the file exists or not.
        /// </summary>
        public string PathAsString
        {
            get { return _rootedPath; }
        }

        public Func<string, string> PathWasher
        {
            get { return _pathWasher; }
        }

        /// <summary>
        /// Returns a descriptor where the PathAsString property returns the path on canonical form. A canonical
        /// descriptor does not contain any redundant names, which means that ".", ".." and extra "\" have been removed
        /// from the string that the descriptor was created with.
        /// This method succeeds whether the file exists or not.
        /// </summary>
        public RootedDirectory AsCanonical() // TODO? Make into extension method and add a Clone() method?
        {
            return new RootedDirectory(_fileSystem, _canonicalFullPath, PathWasher);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedDirectory operator +(RootedDirectory lhs, RelativeDirectory rhs)
        {
            return lhs.Append(rhs);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(RootedDirectory lhs, RelativeFile rhs)
        {
            return lhs.Append(rhs);
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RootedFile operator +(RootedDirectory lhs, FileName rhs)
        {
            return lhs.Append(rhs);
        }

        #endregion // Public Interface

        #region Value Object

        public bool Equals(RootedDirectory other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._fileSystem, _fileSystem) && Equals(other._canonicalFullPath, _canonicalFullPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (RootedDirectory)) return false;
            return Equals((RootedDirectory) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_fileSystem.GetHashCode()*397) ^ _canonicalFullPath.GetHashCode();
            }
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