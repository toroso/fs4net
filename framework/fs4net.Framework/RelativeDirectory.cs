using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public sealed class RelativeDirectory : IDirectory<RelativeDirectory>, IRelativeFileSystemItem<RelativeDirectory>
    {
        private readonly string _relativePath;
        private readonly string _canonicalFullPath;

        private RelativeDirectory(string relativePath)
        {
            _relativePath = relativePath;
            _canonicalFullPath = new CanonicalPathBuilder(relativePath).BuildForRelativeDirectory();
        }

        #region Public Interface

        /// <summary>
        /// Creates a new instance of this descriptor representing the given relative path.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path is invalid, e.g. it's rooted, empty, starts
        /// or ends with white space or contains one or more invalid characters.</exception>
        public static RelativeDirectory FromString(string relativePath)
        {
            return new RelativeDirectory(relativePath);
        }

        /// <summary>
        /// Returns the path that this descriptor represent as a string. It is returned on the same format as it was
        /// created with. This means that it can contain redundant parts such as ".", ".." inside paths and multiple
        /// "\". To remove such redundant parts, use the AsCanonical() factory method.
        /// </summary>
        public string PathAsString
        {
            get { return _relativePath; }
        }

        public Func<string, string> PathWasher
        {
            get { return PathWashers.NullWasher; }
        }

        /// <summary>
        /// Returns a descriptor where the PathAsString property returns the path on canonical form. A canonical
        /// descriptor does not contain any redundant names, which means that ".", ".." and extra "\" have been removed
        /// from the string that the descriptor was created with.
        /// </summary>
        public RelativeDirectory AsCanonical()
        {
            return new RelativeDirectory(_canonicalFullPath);
        }

        /// <summary>
        /// Returns a descriptor where the two descriptors are concatenated.
        /// </summary>
        public static RelativeDirectory operator +(RelativeDirectory lhs, RelativeDirectory rhs)
        {
            return lhs.Append(rhs);
        }

        /// <summary>
        /// Returns a descriptor where the two descriptors are concatenated.
        /// </summary>
        public static RelativeFile operator +(RelativeDirectory lhs, RelativeFile rhs)
        {
            return lhs.Append(rhs);
        }

        #endregion // Public Interface

        #region Value Object

        public bool Equals(RelativeDirectory other)
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

        public static bool operator ==(RelativeDirectory left, RelativeDirectory right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RelativeDirectory left, RelativeDirectory right)
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

    public static class RelativeDirectoryExtensions
    {
        public static RelativeFile Append(this RelativeDirectory lhs, RelativeFile rhs)
        {
            return RelativeFile.FromString(PathUtils.Combine(lhs.PathAsString, rhs.PathAsString));
        }

        public static RelativeDirectory Append(this RelativeDirectory lhs, RelativeDirectory rhs)
        {
            return RelativeDirectory.FromString(PathUtils.Combine(lhs.PathAsString, rhs.PathAsString));
        }
    }
}