namespace fs4net.Framework
{
    public class RootedCanonicalPath
    {
        public string FullPath { get; private set; }

        internal RootedCanonicalPath(string fullPath)
        {
            FullPath = fullPath;
        }
    }
}