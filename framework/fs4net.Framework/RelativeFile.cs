using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a the relative path to a file. It is relative, or non-rooted, meaning that that it does not start
    /// with a drive (e.g. c:, d:, \\network\drive, etc).
    /// </summary>
    public sealed class RelativeFile : IRelativeFile<RelativeFile>
    {
        private readonly string _canonicalFullPath;

        private RelativeFile(string relativePath)
        {
            PathAsString = relativePath;
            _canonicalFullPath = new CanonicalPathBuilder(relativePath).BuildForRelativeFile();
        }

        #region Public Interface

        /// <summary>
        /// Initializes a new instance of the class on the specified path.
        /// </summary>
        /// <param name="relativePath">A string specifying the path that the class should encapsulate.</param>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified path is invalid, e.g. it's empty,
        /// starts or ends with white space or contains one or more invalid characters.</exception>
        /// <exception cref="fs4net.Framework.RootedPathException">The specified path is rooted.</exception>
        public static RelativeFile FromString(string relativePath)
        {
            return new RelativeFile(relativePath);
        }

        public string PathAsString { get; private set; }

        public RelativeFile AsCanonical()
        {
            return new RelativeFile(_canonicalFullPath);
        }

        #endregion // Public Interface

        #region Value Object

        /// <summary>
        /// Determines whether the specified instance denotes the same path as the current instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public bool Equals<T>(IRelativeFileSystemItem<T> other)
            where T : IRelativeFileSystemItem<T>
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
        /// Determines whether the left instance denotes the same path as the right instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator ==(RelativeFile left, RelativeFile right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the left instance denotes a different path than the right instance. The
        /// comparison is made using the canonical form, meaning that redundant "." and ".." have been removed.
        /// </summary>
        public static bool operator !=(RelativeFile left, RelativeFile right)
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