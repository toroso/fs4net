namespace fs4net.Framework
{
    public interface IDirectory<T> : IFileSystemItem<T> where T : IDirectory<T>
    {
    }
}