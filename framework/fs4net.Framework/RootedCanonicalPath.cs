namespace fs4net.Framework
{
    public sealed class RootedCanonicalPath
    {
        public string FullPath { get; private set; }

        internal RootedCanonicalPath(string fullPath)
        {
            FullPath = fullPath;
        }

        #region Value Object

        public bool Equals(RootedCanonicalPath other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.FullPath, FullPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (RootedCanonicalPath)) return false;
            return Equals((RootedCanonicalPath) obj);
        }

        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        public static bool operator ==(RootedCanonicalPath left, RootedCanonicalPath right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RootedCanonicalPath left, RootedCanonicalPath right)
        {
            return !Equals(left, right);
        }

        #endregion // Value Object


        #region Debugging

        public override string ToString()
        {
            return FullPath;
        }

        #endregion Debugging
    }
}