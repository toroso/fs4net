using System;
using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    /// <summary>
    /// Represents a file name, that is, the file name part of a path.
    /// </summary>
    public sealed class FileName : IRelativeFile<FileName>
    {
        private FileName(string fullName)
        {
            FullName = fullName;
            new CanonicalPathBuilder(fullName).BuildForFileName();
        }


        #region Public Interface

        /// <summary>
        /// Initializes a new file name instance given the full name including the extension.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path name is null.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified file name contains invalid
        /// characters, space in an invalid place, an empty extension, or is invalid in some other way.</exception>
        public static FileName FromString(string fullName)
        {
            return new FileName(fullName);
        }

        /// <summary>Create by specifying name and extension separately.</summary>
        /// <param name="name">The name part of the filename.</param>
        /// <param name="extension">The extension part of the filename, including the period (".").</param>
        /// <exception cref="System.ArgumentNullException">Either of the parameters is null.</exception>
        /// <exception cref="System.ArgumentException">The extension does not start with a period or it ends with a
        /// period.</exception>
        /// <exception cref="fs4net.Framework.InvalidPathException">The specified file name contains invalid
        /// characters, space in an invalid place, or is invalid in some other way.</exception>
        public static FileName FromNameAndExtension(string name, string extension)
        {
            ThrowHelper.ThrowIfNull(name, "name");
            ThrowHelper.ThrowIfNull(extension, "extension");
            if (extension.Length > 0 && !extension.StartsWith("."))
            {
                throw new ArgumentException(string.Format("The extension '{0}' does not start with a period.", extension), "extension");
            }
            if (name.Length > 0 && name.EndsWith("."))
            {
                throw new ArgumentException(string.Format("The name '{0}' ends with a period which is not allowed.", extension), "extension");
            }
            return new FileName(name + extension);
        }

        /// <summary>Returns the whole filename, including the extension if it exists.</summary>
        public string FullName { get; private set; }

        /// <summary>Returns the whole filename, including the extension if it exists.</summary>
        public string PathAsString
        {
            get { return FullName; }
        }

        public FileName AsCanonical()
        {
            return this;
        }

        #endregion // Public Interface

        /// <summary>
        /// Determines whether the specified instance denotes the same file name as the current instance.
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
        /// Determines whether the left instance denotes the same file name as the right instance.
        /// </summary>
        public static bool operator ==(FileName left, FileName right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// Determines whether the left instance denotes a different file name than the right instance.
        /// </summary>
        public static bool operator !=(FileName left, FileName right)
        {
            return !Equals(left, right);
        }

        #region Debugging

        public override string ToString()
        {
            return PathAsString;
        }

        #endregion Debugging
    }

    public static class FileNameExtensions
    {
        /// <summary>
        /// Returns the extension part of this filename, including the period (".").
        /// </summary>
        public static string Extension(this FileName me) // TODO: Value Object? Support with and without .?
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return Path.GetExtension(me.FullName);
        }

        /// <summary>
        /// Returns the name part of this filename, excluding the extension if it exists.
        /// </summary>
        public static string Name(this FileName me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return Path.GetFileNameWithoutExtension(me.FullName);
        }

        internal static RelativeFile AsRelativeFile(this FileName me)
        {
            return RelativeFile.FromString(me.PathAsString);
        }
    }
}