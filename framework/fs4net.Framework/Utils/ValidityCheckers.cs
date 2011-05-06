using fs4net.Framework.Impl;

namespace fs4net.Framework.Utils
{
    public static class ValidityCheckers
    {
        /// <summary>
        /// Checks if the path is a valid and rooted directory.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        public static bool IsValidRootedDirectory(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRootedDirectory;
        }

        /// <summary>
        /// Checks if the path is a valid and rooted file.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">The specified path is null.</exception>
        public static bool IsValidRootedFile(this string path)
        {
            ThrowHelper.ThrowIfNull(path, "path");
            return new CanonicalPathBuilder(path).IsRootedFile;
        }
    }
}