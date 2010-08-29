using System;
using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework
{
    public interface IRootedFileSystemItem<T> : IFileSystemItem<T> where T : IRootedFileSystemItem<T>
    {
        IFileSystem FileSystem { get; }
    }

    public static class RootedFileSystemItemExtensions
    {
        /// <summary>
        /// Determines whether two different paths are actually the same.
        /// </summary>
        public static bool DenotesSamePathAs<T1, T2>(this IRootedFileSystemItem<T1> me, IRootedFileSystemItem<T2> other)
            where T1 : IRootedFileSystemItem<T1>
            where T2 : IRootedFileSystemItem<T2>
        {
            if (ReferenceEquals(null, me)) return false;
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(me, other)) return true;
            return Equals(me.FileSystem, other.FileSystem) && Equals(me.CanonicalPathAsString(), other.CanonicalPathAsString());
        }

        /// <summary>
        /// Determines whether two different paths are actually the same.
        /// </summary>
        public static bool DenotesSamePathAs<T>(this IRootedFileSystemItem<T> me, object obj)
            where T : IRootedFileSystemItem<T>
        {
            if (ReferenceEquals(null, me)) return false;
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(me, obj)) return true;
            // Is there another way to do this, not violating OCP?
            if (obj.GetType() == typeof(Drive)) return me.DenotesSamePathAs((Drive)obj);
            if (obj.GetType() == typeof(RootedDirectory)) return me.DenotesSamePathAs((RootedDirectory)obj);
            if (obj.GetType() == typeof(RootedFile)) return me.DenotesSamePathAs((RootedFile)obj);
            return false;
        }

        internal static int InternalGetHashCode<T>(this IRootedFileSystemItem<T> me)
            where T : IRootedFileSystemItem<T>
        {
            unchecked
            {
                return (me.FileSystem.GetHashCode() * 397) ^ me.CanonicalPathAsString().GetHashCode();
            }
        }

        /// <summary>
        /// Returns the drive that the denoted item is located on.
        /// This property succeeds whether the denoted item exists or not.
        /// </summary>
        /// <exception cref="System.IO.PathTooLongException">If the file's path is relative and concatenated
        /// with the current directory it exceeds the system-defined maximum length.</exception>
        public static Drive Drive<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return new Drive(me.InternalFileSystem(), CanonicalPathBuilder.GetDriveName(me.PathAsString));
        }

        /// <summary>
        /// Returns the parent directory of the denoted item.
        /// </summary>
        public static RootedDirectory ParentDirectory<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            // TODO: Throw if there is no parent...?
            return new RootedDirectory(me.InternalFileSystem(), Path.GetDirectoryName(me.PathAsString).RemoveEndingBackslash(), me.PathWasher);
        }

        internal static bool IsFile<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return me.InternalFileSystem().IsFile(me.CanonicalPathAsString());
        }

        internal static bool IsDirectory<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return me.InternalFileSystem().IsDirectory(me.CanonicalPathAsString());
        }

        internal static RootedCanonicalPath CanonicalPathAsString<T>(this IFileSystemItem<T> me) where T : IFileSystemItem<T>
        {
            return new RootedCanonicalPath(me.AsCanonical().PathAsString);
        }

        internal static IInternalFileSystem InternalFileSystem<T>(this IRootedFileSystemItem<T> me) where T : IRootedFileSystemItem<T>
        {
            return ((IInternalFileSystem)me.FileSystem);
        }
    }

    internal static class RootedFileSystemItemVerifications
    {
        internal static void VerifyOnSameFileSystemAs<T>(this IRootedFileSystemItem<T> me, IRootedFileSystemItem<T> destination)
            where T : IRootedFileSystemItem<T>
        {
            if (me.FileSystem != destination.FileSystem)
            {
                throw new InvalidOperationException("The source and destination are associated with different file systems, something that is not allowed.");
            }
        }

        internal static void VerifyOnSameDriveAs<T, TOther>(this IRootedFileSystemItem<T> me, IRootedFileSystemItem<TOther> other, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
            where TOther : IRootedFileSystemItem<TOther>
        {
            if (me.Drive() != other.Drive())
            {
                throw createException();
            }
        }

        internal static void VerifyIsNotAFile<T>(this IRootedFileSystemItem<T> me, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            if (me.IsFile())
            {
                throw createException();
            }
        }

        internal static void VerifyIsNotADirectory<T>(this IRootedFileSystemItem<T> me, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            if (me.IsDirectory())
            {
                throw createException();
            }
        }

        internal static void VerifyIsAFile<T>(this IRootedFileSystemItem<T> me, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            if (!me.IsFile())
            {
                throw createException();
            }
        }

        internal static void VerifyIsADirectory<T>(this IRootedFileSystemItem<T> me, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            if (!me.IsDirectory())
            {
                throw createException();
            }
        }

        internal static void VerifyIsNotTheSameAs<T>(this IRootedFileSystemItem<T> me, IRootedFileSystemItem<T> other, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            if (me.Equals(other))
            {
                throw createException();
            }
        }

        internal static void VerifyIsNotAParentOf<T>(this IRootedFileSystemItem<T> me, IRootedFileSystemItem<T> other, Func<Exception> createException)
            where T : IRootedFileSystemItem<T>
        {
            var lhs = me.CanonicalPathAsString();
            var rhs = other.CanonicalPathAsString();
            if (rhs.FullPath.StartsWith(lhs.FullPath))
            {
                throw createException();
            }
        }

        internal static void VerifyDateTime(DateTime at, string operation, string itemType)
        {
            if (at.IsBefore(PathUtils.MinimumDate))
            {
                throw new ArgumentOutOfRangeException("at", string.Format("Can't {0} to '{1}' since it's not valid for a {2}.", operation, at, itemType));
            }
        }
    }
}