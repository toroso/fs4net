using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public sealed class RelativeDirectory : IDirectory<RelativeDirectory>, IRelativeFileSystemItem<RelativeDirectory>
    {
        private readonly string _canonicalFullPath;

        private RelativeDirectory(string relativePath)
        {
            PathAsString = relativePath;
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
        public string PathAsString { get; private set; }

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
        public static RelativeDirectory operator +(RelativeDirectory left, RelativeDirectory right)
        {
            return left.Append(right);
        }

        /// <summary>
        /// Returns a descriptor where the two descriptors are concatenated.
        /// </summary>
        public static RelativeFile operator +(RelativeDirectory left, RelativeFile right)
        {
            return left.Append(right);
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
        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RelativeFile Append(this RelativeDirectory left, RelativeFile right)
        {
            ThrowHelper.ThrowIfNull(left, "left");
            ThrowHelper.ThrowIfNull(right, "right");
            if (left.PathAsString.IsEmpty()) return right;
            return RelativeFile.FromString(PathUtils.Combine(left.PathAsString, right.PathAsString));
        }

        /// <summary>
        /// Concatenates the two descriptors into one and returns it.
        /// </summary>
        public static RelativeDirectory Append(this RelativeDirectory left, RelativeDirectory right)
        {
            ThrowHelper.ThrowIfNull(left, "left");
            ThrowHelper.ThrowIfNull(right, "right");
            if (left.PathAsString.IsEmpty()) return right;
            if (right.PathAsString.IsEmpty()) return left;
            return RelativeDirectory.FromString(PathUtils.Combine(left.PathAsString, right.PathAsString));
        }

        /// <summary>
        /// Returns the parent directory of the denoted item.
        /// </summary>
        public static RelativeDirectory Parent(this RelativeDirectory me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return me.Append(RelativeDirectory.FromString("..")).AsCanonical();
        }

        /// <summary>
        /// Returns a relative descriptor containing the name of the leaf folder of this path.
        /// Example: LeafFolder("my\path\to") => "to".
        /// The method returns the leaf folder in the path's canonical form. This could be an empty directory or even
        /// "..".
        /// </summary>
        // TODO: Exceptions, Allow empty RelativeDirectories
        public static RelativeDirectory LeafFolder(this RelativeDirectory me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            var canonicalPath = me.AsCanonical().PathAsString;
            var lastBackslashIndex = canonicalPath.LastIndexOf('\\');
            return RelativeDirectory.FromString(canonicalPath.Substring(lastBackslashIndex + 1));
        }
    }
}