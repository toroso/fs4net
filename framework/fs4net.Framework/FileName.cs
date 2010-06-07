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

        /// <summary>
        /// Returns the whole filename, including the extension if it exists.
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
        }

        #endregion // Public Interface

        #region Implementation of IFileSystemItem<FileName>

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

        #endregion
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