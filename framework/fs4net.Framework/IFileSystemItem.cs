using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public interface IFileSystemItem<T> where T: IFileSystemItem<T>
    {
        /// <summary>
        /// Returns the path that this descriptor represents as a string. It is returned on the same format as it was
        /// created with which means that it can contain redundant parts such as ".", "..". To remove such redundant
        /// parts, use the AsCanonical() factory method.
        /// This property succeeds whether the object that the descriptor points to exists or not.
        /// </summary>
        string PathAsString { get; }

        /// <summary>
        /// Returns a descriptor where the PathAsString property returns the path on canonical form. A canonical
        /// descriptor does not contain any redundant names, which means that "." and ".." have been removed from the
        /// string that the descriptor was created with.
        /// This property succeeds whether the object that the descriptor points to exists or not.
        /// </summary>
        T AsCanonical();
    }

    public static class FileSystemItemExtensions
    {
        internal static string AsLowerCaseCanonicalString<T>(this IFileSystemItem<T> me)
            where T : IFileSystemItem<T>
        {
            ThrowHelper.ThrowIfNull(me, "me");
            return me.AsCanonical().PathAsString.ToLower();
        }
    }
}