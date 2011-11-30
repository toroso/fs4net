using System.IO;
using fs4net.Framework.Impl;

namespace fs4net.Framework.Utils
{
    public static class RootedFileUtilities
    {
        /// <summary>
        /// Reads all content of the file and returns it as a string.
        /// </summary>
        /// <exception cref="System.IO.FileNotFoundException">The file cannot be found.</exception>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        public static string ReadText(this RootedFile me)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            using (var stream = me.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Creates a new file containing the specified string.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        public static void WriteText(this RootedFile me, string text)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(text, "text");
            using (var stream = me.CreateWriteStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }

        /// <summary>
        /// Appends the specified string to the end of the file. If the file does not exist it is created.
        /// </summary>
        /// <exception cref="System.IO.DirectoryNotFoundException">The file's parent directory cannot be found; The
        /// file is on an unmapped drive.</exception>
        /// <exception cref="System.UnauthorizedAccessException">The descriptor denotes an existing directory.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission (according to the MSDN documentation).</exception>
        public static void AppendText(this RootedFile me, string text)
        {
            ThrowHelper.ThrowIfNull(me, "me");
            ThrowHelper.ThrowIfNull(text, "text");
            using (var stream = me.CreateAppendStream())
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(text);
            }
        }
    }
}