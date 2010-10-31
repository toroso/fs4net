using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public interface IRelativeFile<T> : IFile<T>, IRelativeFileSystemItem<T> where T : IRelativeFile<T>
    {
    }

    public static class RelativeFileExtensions
    {
        /// <summary>
        /// Returns the parent directory of the denoted item.
        /// </summary>
        public static RelativeDirectory Parent<T>(this IRelativeFile<T> me)
            where T : IRelativeFile<T>
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return RelativeDirectory.FromString(Path.GetDirectoryName(me.PathAsString).RemoveEndingBackslash());
        }
    }
}