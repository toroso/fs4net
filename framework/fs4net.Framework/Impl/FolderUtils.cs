using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace fs4net.Framework.Impl
{
    internal static class FolderUtils
    {
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
    }
}