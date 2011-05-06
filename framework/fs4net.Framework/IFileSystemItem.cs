namespace fs4net.Framework
{
    public interface IFileSystemItem<T> where T: IFileSystemItem<T>
    {
        string PathAsString { get; }
        T AsCanonical();
    }

    public static class FileSystemItemExtensions
    {
        internal static string AsLowerCaseCanonicalString<T>(this IFileSystemItem<T> me)
            where T : IFileSystemItem<T>
        {
            return me.AsCanonical().PathAsString.ToLower();
        }
    }
}