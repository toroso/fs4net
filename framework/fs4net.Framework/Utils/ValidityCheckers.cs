using fs4net.Framework.Impl;

namespace fs4net.Framework.Utils
{
    public static class ValidityCheckers
    {
        /// <summary>
        /// Checks if the path is a valid and rooted directory.
        /// </summary>
        public static bool IsValidRootedDirectory(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRootedDirectory;
        }

        /// <summary>
        /// Checks if the path is a valid and rooted file.
        /// </summary>
        public static bool IsValidRootedFile(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRootedFile;
        }

        /// <summary>
        /// Checks if the path is a valid and relative directory.
        /// </summary>
        public static bool IsValidRelativeDirectory(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRelativeDirectory;
        }

        /// <summary>
        /// Checks if the path is a valid and relative file.
        /// </summary>
        public static bool IsValidRelativeFile(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRelativeFile;
        }
    }
}