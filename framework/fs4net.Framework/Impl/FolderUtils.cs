using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace fs4net.Framework.Impl
{
    internal static class FolderUtils
    {
        internal static IEnumerable<string> TokenizeToFolders(this string folderPath, string fullpath)
        {
            var folders = folderPath.Split(new[] { '\\' });
            foreach (string folder in folders)
            {
                ValidateFolderName(folder, fullpath);
                yield return folder;
            }
        }

        private static void ValidateFolderName(string folder, string fullpath)
        {
            if (folder.Length == 0)
            {
                throw new InvalidPathException(string.Format("The path '{0}' contains an empty folder which is not allowed", fullpath));
            }
            if (char.IsWhiteSpace(folder.Last())) throw new InvalidPathException(string.Format("The folder '{0}' in the path '{1}' ends with a whitespace which is not allowed.", folder, fullpath));
            if (folder.EndsWith(".") && folder != "." && folder != "..")
            {
                throw new InvalidPathException(string.Format("The folder '{0}' in the path '{1}' ends with a dot which is not allowed.", folder, fullpath));
            }

            string errorMessage = ValidatePathCharacters(folder, fullpath, "folder");
            if (errorMessage != null) throw new InvalidPathException(errorMessage);
        }

        internal static string ValidatePathCharacters(string folder, string fullpath, string pathPart)
        {
            var invalidChars = GetInvalidPathChars();
            var invalidCharsInPath = folder.Where(invalidChars.Contains);
            if (invalidCharsInPath.Any())
            {
                return string.Format("The {0} '{1}' in the path '{2}' contains the invalid character '{3}'", pathPart, folder, fullpath, invalidCharsInPath.First());
            }
            return null;
        }

        internal static void ValidatePathCharacters(string folder, string pathPart, Validator validator)
        {
            var invalidChars = GetInvalidPathChars();
            var invalidCharsInPath = folder.Where(invalidChars.Contains);
            if (invalidCharsInPath.Any())
            {
                validator.AddError<InvalidPathException>("The {1} '{2}' in the path '{0}' contains the invalid character '{3}'", pathPart, folder, invalidCharsInPath.First());
            }
        }

        internal static IEnumerable<char> GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars().Concat(new[] { ':', '*', '?', '/' });
        }

        internal static IEnumerable<string> Canonify(this IEnumerable<string> folders)
        {
            return folders
                .RemoveSelfFolders()
                .FlattenParentReferences();
        }

        internal static IEnumerable<string> RemoveSelfFolders(this IEnumerable<string> folders)
        {
            return folders.Where(folder => folder != ".");
        }

        internal static IEnumerable<string> FlattenParentReferences(this IEnumerable<string> folders)
        {
            // TODO: Impossible to understand this algorithm... can we do better?
            return folders.Reverse().RemoveNextIfParent().Reverse();
        }

        private static IEnumerable<string> RemoveNextIfParent(this IEnumerable<string> folders)
        {
            int skipCount = 0;
            foreach (string folder in folders)
            {
                if (folder == "..")
                {
                    skipCount++;
                }
                else if (skipCount > 0)
                {
                    skipCount--;
                }
                else
                {
                    yield return folder;
                }
            }
            for (int i = 0; i < skipCount; i++)
            {
                yield return "..";
            }
        }

        internal static string MergeToPath(this IEnumerable<string> folders)
        {
            var builder = new StringBuilder();
            foreach (string folder in folders)
            {
                if (builder.Length > 0) builder.Append('\\');
                builder.Append(folder);
            }
            return builder.ToString();
        }

        internal static string Center(this string me, int removeAtStartCount, int removeAtEndCount)
        {
            var startIndex = removeAtStartCount;
            var length = me.Length - removeAtStartCount - removeAtEndCount;
            return me.Substring(startIndex, length);
        }
    }
}