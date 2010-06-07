namespace fs4net.Framework
{
    public interface IRelativeFile<T> : IFile<T> where T : IRelativeFile<T>
    {
    }
}