using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public interface IFile<T> : IFileSystemItem<T> where T : IFile<T>
    {
    }

    public static class FileExtensions
    {
        /// <summary>
        /// Returns the file name part of this descriptor. This is the path part after the last folder separator, or
        /// the whole path if it contains no folder separator.
        /// This method succeeds whether the file exists or not.
        /// </summary>
        public static FileName FileName<T>(this IFile<T> me) where T : IFile<T>
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return Framework.FileName.FromString(Path.GetFileName(me.PathAsString));
        }
    }
}