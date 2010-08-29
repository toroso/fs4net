using System;
using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public class FileName : IRelativeFile<FileName>
    {
        private readonly string _fullName;

        private FileName(string fullName)
        {
            _fullName = fullName;
            new CanonicalPathBuilder(fullName).BuildForFileName();
        }


        #region Public Interface

        public static FileName FromString(string fullName)
        {
            return new FileName(fullName);
        }

        public static FileName FromNameAndExtension(string name, string extension)
        {
            return new FileName(name + extension);
        }

        /// <summary>
        /// Returns the whole filename, including the extension if it exists.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
        }

        public string PathAsString
        {
            get { return _fullName; }
        }

        public Func<string, string> PathWasher
        {
            get { return PathWashers.NullWasher; }
        }

        public FileName AsCanonical()
        {
            return this;
        }

        #endregion // Public Interface

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

        public static bool operator ==(FileName left, FileName right)
        {
            return Equals(left, right);
        }

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
            return Path.GetExtension(me.FullName);
        }

        /// <summary>
        /// Returns the name part of this filename, excluding the extension if it exists.
        /// </summary>
        public static string Name(this FileName me)
        {
            return Path.GetFileNameWithoutExtension(me.FullName);
        }
    }
}