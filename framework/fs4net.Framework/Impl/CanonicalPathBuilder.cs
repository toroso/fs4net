using System.IO;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal sealed class CanonicalPathBuilder
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
            bool hasEndingBackslash = folderPath.Length > 0 && folderPath.EndsWith(@"\");

            if (driveName.IsEmpty() && hasLeadingBackslash) throw new InvalidPathException(string.Format("The path '{0}' starts with a '\\' which is not allowed.", _fullPath));
            if (filename.IsEmpty() && hasEndingBackslash) throw new InvalidPathException(string.Format("The path '{0}' ends with a '\\' which is not allowed.", _fullPath));
            if (mustBeRooted && folderPath.Length > 1 && !hasLeadingBackslash) throw new InvalidPathException(string.Format("Expected a '\\' after the drive but found a '{0}' in the path '{1}'.", folderPath.First(), _fullPath));

            string canonicalFolderPath = GetCanonicalFolderPath(folderPath, hasLeadingBackslash, hasEndingBackslash, !mustBeRooted);
            bool appendLeadingBackslash = hasLeadingBackslash && canonicalFolderPath.Length > 0;
            string canonicalPath = driveName + (appendLeadingBackslash ? @"\" : string.Empty) + canonicalFolderPath + (hasEndingBackslash && includeEndingBackslash ? @"\" : string.Empty) + filename;

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
                if (!canonicalCanStartWithDoubleDots && canonicalFolders.Any() && canonicalFolders.First() == "..")
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
}