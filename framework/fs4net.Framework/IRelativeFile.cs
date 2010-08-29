namespace fs4net.Framework
{
    public interface IRelativeFile<T> : IFile<T>, IRelativeFileSystemItem<T> where T : IRelativeFile<T>
    {
    }
}