using System;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a the relative path to a file. It is relative, or non-rooted, meaning that that it does not start
    /// with a drive (e.g. c:, d:, \\network\drive, etc).
    /// </summary>
    public class RelativeFile : IRelativeFile<RelativeFile>
    {
        private readonly string _relativePath;
        private readonly string _canonicalFullPath;

        private RelativeFile(string relativePath)
        {
            _relativePath = relativePath;
            _canonicalFullPath = new CanonicalPathBuilder(relativePath).BuildForRelativeFile();
        }

        #region Public Interface

        /// <summary>
        /// Creates a new instance of this descriptor representing the given relative path.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        /// <exception cref="System.ArgumentException">The specified path is invalid, e.g. it's rooted, empty, starts
        /// or ends with white space or contains one or more invalid characters.</exception>
        public static RelativeFile FromString(string relativePath)
        {
            return new RelativeFile(relativePath);
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
        public RelativeFile AsCanonical() // TODO: Move to extension method?
        {
            return new RelativeFile(_canonicalFullPath);
        }

        #endregion // Public Interface

        #region Value Object

        // TODO: Move to extension methods?
        public bool Equals(RelativeFile other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other._canonicalFullPath, _canonicalFullPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(RelativeFile)) return false;
            return Equals((RelativeFile)obj);
        }

        public override int GetHashCode()
        {
            return _canonicalFullPath.GetHashCode();
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