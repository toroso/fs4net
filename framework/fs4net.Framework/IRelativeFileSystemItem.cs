namespace fs4net.Framework
{
    public interface IRelativeFileSystemItem<T> : IFileSystemItem<T> where T : IRelativeFileSystemItem<T>
    {
    }

    public static class RelativeFileSystemItemExtensions
    {
        /// <summary>
        /// Determines whether two different paths are actually the same.
        /// </summary>
        public static bool DenotesSamePathAs<T1, T2>(this IRelativeFileSystemItem<T1> me, IRelativeFileSystemItem<T2> other)
            where T1 : IRelativeFileSystemItem<T1>
            where T2 : IRelativeFileSystemItem<T2>
        {
            if (ReferenceEquals(null, me)) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(me, other)) return true;
            return Equals(me.AsLowerCaseCanonicalString(), other.AsLowerCaseCanonicalString());
        }

        /// <summary>
        /// Determines whether two different paths are actually the same.
        /// </summary>
        public static bool DenotesSamePathAs<T>(this IRelativeFileSystemItem<T> me, object obj)
            where T : IRelativeFileSystemItem<T>
        {
            if (ReferenceEquals(null, me)) return false;
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(me, obj)) return true;
            // Is there another way to do this, not violating OCP?
            if (obj.GetType() == typeof(FileName)) return me.DenotesSamePathAs((FileName)obj);
            if (obj.GetType() == typeof(RelativeFile)) return me.DenotesSamePathAs((RelativeFile)obj);
            if (obj.GetType() == typeof(RelativeDirectory)) return me.DenotesSamePathAs((RelativeDirectory)obj);
            return false;
        }

        internal static int InternalGetHashCode<T>(this IRelativeFileSystemItem<T> me)
            where T : IRelativeFileSystemItem<T>
        {
            return me.AsLowerCaseCanonicalString().GetHashCode();
        }
    }
}