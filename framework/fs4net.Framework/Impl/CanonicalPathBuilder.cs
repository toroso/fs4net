using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace fs4net.Framework.Impl
{
    internal class CanonicalPathBuilder
    {
        private const int MaxPath = 259; // TODO Get from FileSystem

        private readonly string _fullPath;

        public CanonicalPathBuilder(string fullPath)
        {
            _fullPath = fullPath;
            ThrowHelper.ThrowIfNull(fullPath, "fullPath");
            if (_fullPath == null) throw new InvalidPathException("The path is empty.");
        }

        public static string GetDriveName(string fullPath)
        {
            var driveName = new DriveParser(fullPath);
            return driveName.DriveName;
        }

        public void BuildForDrive()
        {
            if (new DriveParser(_fullPath).DriveName != _fullPath)
            {
                throw new InvalidPathException(string.Format("The drive name '{0}' is not valid.", _fullPath));
            }
        }

        internal string BuildForRootedFile()
        {
            return GetCanonicalPath(GetDriveName(), GetFilename(), true, true);
        }

        public string BuildForRootedDirectory()
        {
            return GetCanonicalPath(GetDriveName(), string.Empty, false, true);
        }

        public string BuildForRelativeFile()
        {
            ValidatePathNotRooted();
            return GetCanonicalPath(string.Empty, GetFilename(), true, false);
        }

        public string BuildForRelativeDirectory()
        {
            ValidatePathNotRooted();
            return GetCanonicalPath(string.Empty, string.Empty, false, false);
        }

        public void BuildForFileName()
        {
            if (GetFilename() != _fullPath) throw new InvalidPathException(string.Format("The filename '{0}' is not valid.", _fullPath));
        }

        private void ValidatePathNotRooted()
        {
            var driveName = new DriveParser(_fullPath);
            if (driveName.Exists) throw new RootedPathException(string.Format("The path '{0}' is rooted.", _fullPath));
            if (!driveName.IsValid) throw new InvalidPathException(driveName.InvalidErrorMessage);
        }

        /// <summary>Returns the drive without ending backslash.</summary>
        private string GetDriveName()
        {
            var driveName = new DriveParser(_fullPath);
            if (!driveName.Exists) throw new NonRootedPathException(string.Format("The path '{0}' is not rooted.", _fullPath));
            if (!driveName.IsValid) throw new InvalidPathException(driveName.InvalidErrorMessage);
            return driveName.DriveName;
        }

        /// <summary>Returns the filename without leading backslash.</summary>
        private string GetFilename()
        {
            return new FilenameParser(_fullPath).Filename;
        }

        private string GetCanonicalPath(string driveName, string filename, bool includeEndingBackslash, bool mustBeRooted)
        {
            string folderPath = _fullPath.Center(driveName.Length, filename.Length);
            bool hasLeadingBackslash = folderPath.Length > 1 && folderPath.StartsWith(@"\");
            bool hasEndingBackslash = folderPath.Length > 1 && folderPath.EndsWith(@"\");

            if (mustBeRooted && folderPath.Length > 1 && !hasLeadingBackslash) throw new InvalidPathException(string.Format("Expected a '\\' after the drive but found a '{0}' in the path '{1}'.", folderPath.First(), _fullPath));

            string canonicalFolderPath = GetCanonicalFolderPath(folderPath, hasLeadingBackslash, hasEndingBackslash, !mustBeRooted);
            string canonicalPath = driveName + (hasLeadingBackslash ? @"\" : string.Empty) + canonicalFolderPath + (hasEndingBackslash && includeEndingBackslash ? @"\" : string.Empty) + filename;

            ValidatePathLength(canonicalPath); // TODO: Should path length be validated for relative paths?
            return canonicalPath;
        }

        private string GetCanonicalFolderPath(string folderPath, bool hasLeadingBackslash, bool hasEndingBackslash, bool canonicalCanStartWithDoubleDots)
        {
            if (folderPath.Length > 1)
            {
                var canonicalFolders = folderPath
                    .Center(hasLeadingBackslash ? 1 : 0, hasEndingBackslash ? 1 : 0)
                    .TokenizeToFolders(_fullPath)
                    .Canonify()
                    .ToList();
                if (!canonicalCanStartWithDoubleDots && canonicalFolders.First() == "..")
                {
                    throw new InvalidPathException(string.Format("The path '{0}' ascends into a folder above the root level.", _fullPath));
                }
                return canonicalFolders
                    .MergeToPath();
            }

            return string.Empty;
        }

        private static void ValidatePathLength(string fullPath)
        {
            if (fullPath.Length > MaxPath)
            {
                string msg = string.Format("The path '{0}' contains '{1}' characters. Maximum allowed is '{2}'.", fullPath, fullPath.Length, MaxPath);
                throw new PathTooLongException(msg);
            }
        }
    }

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