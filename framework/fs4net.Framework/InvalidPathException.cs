using System.IO;
using System.Linq;

namespace fs4net.Framework
{
    // TODO: Should this really be an IOException? The IOExceptions are a bit f*cked up... they can be anything from normal usage exceptions to programming errors.
    public class InvalidPathException : IOException
    {
        internal InvalidPathException(string message) : base(message)
        {
        }

        public static void ThrowIfEmpty(string canonicalPath, string originalPath)
        {
            if (canonicalPath == string.Empty)
            {
                throw new InvalidPathException(string.Format("The path '{0}' represents an empty path when on canonical form, which is not allowed.", originalPath));
            }
        }

        public static void ThrowIfStartsWithWhiteSpace(string fullPath)
        {
            if (fullPath.Length == 0) return;
            if (char.IsWhiteSpace(fullPath.First()))
            {
                throw new InvalidPathException(string.Format("The path '{0}' starts with a white space, which is not allowed.", fullPath));
            }
        }

        public static void ThrowIfEndsWithWhiteSpace(string fullPath)
        {
            if (fullPath.Length == 0) return;
            if (char.IsWhiteSpace(fullPath.Last()))
            {
                throw new InvalidPathException(string.Format("The path '{0}' ends with a white space, which is not allowed.", fullPath));
            }
        }

        public static void ThrowIfEndsWith(string fullPath, params char[] invalidChars)
        {
            if (fullPath.Length == 0) return;
            var invalidChar = invalidChars.Where(ch => ch == fullPath.Last());
            if (invalidChar.Any())
            {
                throw new InvalidPathException(string.Format("The path '{0}' ends with the character '{1}', which is not allowed.", fullPath, invalidChar.First()));
            }
        }
    }
}