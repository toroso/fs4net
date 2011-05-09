using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal class DriveParser
    {
        private readonly string _fullPath;
        private string _invalidErrorMessage;
        private string _driveName;
        private readonly bool _isRooted;

        public DriveParser(string fullPath)
        {
            _fullPath = fullPath;
            _invalidErrorMessage = null;
            _driveName = null;
            _isRooted = true;

            if (ExtractMappedDrive()) return;
            if (ExtractNetworkDrive()) return;

            _isRooted = false;
        }

        /// <summary>Returns true if a drive was extracted or a the drive is invalid.</summary>
        private bool ExtractMappedDrive()
        {
            int firstColon = _fullPath.IndexOf(':');
            if (firstColon >= 0)
            {
                // There is a colon... might it delimit a valid drive?
                if (ExtractDriveLetter(firstColon)) return true;
                if (ValidateFirstBackslashIsBeforeFirstColon(firstColon)) return true;
                // There is a colon after the first backslash... let the folder validation handle that.
            }
            return false;
        }

        private bool ExtractDriveLetter(int positionOfFirstColon)
        {
            if (positionOfFirstColon == 1)
            {
                string pathRoot = _fullPath.Substring(0, 2); // Extract drive name, including the colon
                ValidateDriveLetter(pathRoot.First());
                if (IsValid)
                {
                    _driveName = pathRoot;
                }
                return true;
            }
            return false;
        }

        private void ValidateDriveLetter(char driveLetter)
        {
            char loweredDriveLetter = Char.ToLower(driveLetter);
            if (loweredDriveLetter < 'a' || loweredDriveLetter > 'z')
            {
                SetAsInvalid(String.Format("The path '{0}' has an invalid drive letter '{1}'.", _fullPath, driveLetter));
            }
        }

        private bool ValidateFirstBackslashIsBeforeFirstColon(int firstColon)
        {
            // This validation is only so that we get a correct error message.
            int firstBackslash = _fullPath.IndexOf('\\');
            if (firstBackslash == -1 || firstColon < firstBackslash) // There is a colon before the first backslash (or there is no backslash)
            {
                SetAsInvalid(String.Format("The path '{0}' contains the invalid drive '{1}'", _fullPath, _fullPath.Substring(0, firstColon)));
                return true;
            }
            return false;
        }

        /// <summary>Returns true if a drive was extracted or a the drive is invalid.</summary>
        private bool ExtractNetworkDrive()
        {
            if (_fullPath.StartsWith(@"\\"))
            {
                int secondBackslash = _fullPath.IndexOf('\\', 2);
                string hostName = ExtractHostName(secondBackslash);
                if (!IsValid) return true;

                string shareName = ExtractShareName(secondBackslash + 1);
                if (!IsValid) return true;

                _driveName = String.Format(@"\\{0}\{1}", hostName, shareName);
                return true;
            }
            return false;
        }

        private string ExtractHostName(int endPos)
        {
            string hostName = endPos == -1
                                  ? _fullPath
                                  : _fullPath.Substring(2, endPos - 2);
            ValidateHostName(hostName);
            return hostName;
        }

        private void ValidateHostName(string hostName)
        {
            if (hostName.IsEmpty())
            {
                SetAsInvalid(String.Format("The path '{0}' has an empty host name.", _fullPath));
                return;
            }
            ValidatePathCharacters(hostName, "host name");
        }

        private string ExtractShareName(int startPos)
        {
            string shareName = String.Empty;
            if (startPos > 0)
            {
                int thirdBackslash = _fullPath.IndexOf('\\', startPos);
                shareName = thirdBackslash == -1
                                ? _fullPath.Substring(startPos)
                                : _fullPath.Substring(startPos, thirdBackslash - startPos);
            }
            ValidateShareName(shareName);
            return shareName;
        }

        private void ValidateShareName(string shareName)
        {
            if (shareName.IsEmpty())
            {
                SetAsInvalid(String.Format("The path '{0}' has an empty share name.", _fullPath));
                return;
            }
            ValidatePathCharacters(shareName, "share name");
        }

        private void ValidatePathCharacters(string pathPart, string nameOfPathPart)
        {
            string errorMessage = FolderUtils.ValidatePathCharacters(pathPart, _fullPath, nameOfPathPart);
            if (errorMessage != null) SetAsInvalid(errorMessage);
        }

        private void SetAsInvalid(string invalidErrorMessage)
        {
            _invalidErrorMessage = invalidErrorMessage;
        }

        /// <summary>
        /// Return the valid drive name not including a backslash, or null if IsValid returns false or string.Empty if
        /// Exists return false.
        /// </summary>
        public string DriveName
        {
            get { return _driveName; }
        }

        /// <summary>Returns true if the path validation found the drive name to have a valid form, or if the path is relative.</summary>
        public bool IsValid
        {
            get { return _invalidErrorMessage == null; }
        }

        public string InvalidErrorMessage
        {
            get { return _invalidErrorMessage; }
        }

        /// <summary>Returns true if the path is rooted or starts with something that resembles a drive name.</summary>
        public bool Exists
        {
            get { return _isRooted; }
        }
    }

    internal class DriveParser3
    {
        private readonly string _rootedPath;
        private readonly Validator _validator;
        private readonly string _driveName;

        public DriveParser3(string rootedPath, Validator validator)
        {
            _rootedPath = rootedPath;
            _validator = validator;

            _driveName = ExtractMappedDrive();
            if (_driveName != null) return;

            _driveName = ExtractNetworkDrive();
            if (_driveName != null) return;

            _validator.AddError<NonRootedPathException>("The path '{0}' is not rooted.");
        }

        private bool IsValid
        {
            get { return !_validator.HasError; }
        }

        public string PathWithoutDrive
        {
            get { return _rootedPath.Substring(_driveName.Length); }
        }

        /// <summary>Returns mapped drive name if such exists.</summary>
        private string ExtractMappedDrive()
        {
            var firstColon = _rootedPath.IndexOf(':');
            if (firstColon >= 0)
            {
                var driveLetter = ExtractDriveLetter(firstColon);
                if (driveLetter != null) return driveLetter;
                ValidateFirstBackslashIsBeforeFirstColon(firstColon);
            }
            return null;
        }

        private string ExtractDriveLetter(int positionOfFirstColon)
        {
            if (positionOfFirstColon == 1)
            {
                var pathRoot = _rootedPath.Substring(0, 2); // Extract drive name, including the colon
                ValidateDriveLetter(pathRoot.First());
                if (IsValid)
                {
                    return pathRoot;
                }
            }
            return null;
        }

        private void ValidateDriveLetter(char driveLetter)
        {
            char loweredDriveLetter = Char.ToLower(driveLetter);
            _validator.Ensure<InvalidPathException>(loweredDriveLetter >= 'a' && loweredDriveLetter <= 'z', "The path '{0}' has an invalid drive letter '{1}'.", driveLetter);
        }

        private void ValidateFirstBackslashIsBeforeFirstColon(int firstColon)
        {
            // This validation is only so that we get a correct error message.
            int firstBackslash = _rootedPath.IndexOf('\\');
            _validator.Ensure<InvalidPathException>(firstBackslash != -1 && firstColon > firstBackslash, "The path '{0}' contains the invalid drive '{1}'", _rootedPath.Substring(0, firstColon));
        }

        /// <summary>Returns network drive name if such exists.</summary>
        private string ExtractNetworkDrive()
        {
            if (_rootedPath.StartsWith(@"\\"))
            {
                var secondBackslash = _rootedPath.IndexOf('\\', 2);
                var hostName = ExtractHostName(secondBackslash);
                var shareName = ExtractShareName(secondBackslash + 1);
                if (IsValid)
                {
                    return String.Format(@"\\{0}\{1}", hostName, shareName);
                }
            }
            return null;
        }

        private string ExtractHostName(int endPos)
        {
            var hostName = endPos == -1
                               ? _rootedPath
                               : _rootedPath.Substring(2, endPos - 2);
            ValidateHostName(hostName);
            return hostName;
        }

        private void ValidateHostName(string hostName)
        {
            _validator.Ensure<InvalidPathException>(!hostName.IsEmpty(), "The path '{0}' has an empty host name.");
            ValidatePathCharacters(hostName, "host name");
        }

        private string ExtractShareName(int startPos)
        {
            string shareName = String.Empty;
            if (startPos > 0)
            {
                var thirdBackslash = _rootedPath.IndexOf('\\', startPos);
                shareName = thirdBackslash == -1
                                ? _rootedPath.Substring(startPos)
                                : _rootedPath.Substring(startPos, thirdBackslash - startPos);
            }
            ValidateShareName(shareName);
            return shareName;
        }

        private void ValidateShareName(string shareName)
        {
            _validator.Ensure<InvalidPathException>(!shareName.IsEmpty(), "The path '{0}' has an empty share name.");
            ValidatePathCharacters(shareName, "share name");
        }

        private void ValidatePathCharacters(string pathPart, string nameOfPathPart)
        {
            FolderUtils.ValidatePathCharacters(pathPart, nameOfPathPart, _validator);
        }

        public string AppendTo(string relativePath)
        {
            return _driveName + relativePath;
        }
    }

    internal class FilenameParser3
    {
        private readonly string _pathWithFilename;
        private readonly Validator _validator;
        private readonly string _filename;

        public FilenameParser3(string pathWithFilename, Validator validator)
        {
            _pathWithFilename = pathWithFilename;
            _validator = validator;
            _filename = ExtractFilename();

            ValidateWhiteSpaces();
            if (_validator.HasError) return;
            ValidateFilenameCharacters();
            if (_validator.HasError) return;
            ValidateExtension();
        }

        public string PathWithoutFilename
        {
            get { return _pathWithFilename.Substring(0, _pathWithFilename.Length - _filename.Length); }
        }

        private string ExtractFilename()
        {
            int lastBackslash = _pathWithFilename.LastIndexOf('\\');
            return lastBackslash == -1
                       ? _pathWithFilename
                       : _pathWithFilename.Substring(lastBackslash + 1);
        }

        private void ValidateWhiteSpaces()
        {
            _validator.Ensure<InvalidPathException>(!_filename.IsEmpty(), "The filename of the path '{0}' is empty.");
            if (_validator.HasError) return; // To avoid empty string logic below
            _validator.Ensure<InvalidPathException>(!Char.IsWhiteSpace(_filename.First()), "The filename of the path '{0}' starts with a whitespace which is not allowed.");
            _validator.Ensure<InvalidPathException>(!Char.IsWhiteSpace(_filename.Last()), "The filename of the path '{0}' ends with a whitespace which is not allowed.");
            _validator.Ensure<InvalidPathException>(!_filename.EndsWith("."), "The path '{0}' has an empty extension, which is not allowed.");
        }

        private void ValidateFilenameCharacters()
        {
            FolderUtils.ValidatePathCharacters(_filename, "filename", _validator);
        }

        private void ValidateExtension()
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(_filename);
            _validator.Ensure<InvalidPathException>(!filenameWithoutExtension.IsEmpty(), "The filename of the path '{0}' is empty.");
            if (_validator.HasError) return; // To avoid empty string logic below
            _validator.Ensure<InvalidPathException>(!Char.IsWhiteSpace(filenameWithoutExtension.Last()), "The filename of the path '{0}' ends with a whitespace which is not allowed.");
        }

        public string AppendTo(string pathWithoutFilename)
        {
            return pathWithoutFilename + _filename;
        }
    }

    internal class FoldersParser3
    {
        private readonly string _relativePath;
        private readonly Validator _validator;
        private readonly string _canonifiedPathWithoutBackslashes;
        private bool HasDrive { get; set; }
        private bool HasFilename { get; set; }

        public FoldersParser3(string relativePath, Validator validator, bool hasDrive, bool hasFilename)
        {
            _relativePath = relativePath;
            _validator = validator;
            HasDrive = hasDrive;
            HasFilename = hasFilename;

            ValidateStartAndEnd();
            _canonifiedPathWithoutBackslashes = Canonify(PathWithoutStartingAndEndingBackslashes);
            ValidateAscensionLevel();
        }

        public string Canonified
        {
            get
            {
                var appendLeadingBackslash = HasLeadingBackslash && _canonifiedPathWithoutBackslashes.Length > 0;
                return string.Format("{0}{1}{2}",
                                     appendLeadingBackslash ? "\\" : string.Empty,
                                     _canonifiedPathWithoutBackslashes,
                                     HasEndingBackslash ? "\\" : string.Empty);
            }
        }

        private bool HasLeadingBackslash
        {
            get
            {
                return
                    _relativePath.Length > 1 && // Not to confuse it with ending backslash
                    _relativePath.StartsWith(@"\");
            }
        }

        private bool HasEndingBackslash
        {
            get { return _relativePath.EndsWith(@"\"); }
        }

        protected string PathWithoutStartingAndEndingBackslashes
        {
            get
            {
                int start = HasLeadingBackslash ? 1 : 0;
                int endDistance = HasEndingBackslash ? 1 : 0;
                return _relativePath.Substring(start, _relativePath.Length - start - endDistance);
            }
        }

        private void ValidateStartAndEnd()
        {
            _validator.Ensure<InvalidPathException>(!HasLeadingBackslash || HasDrive, "The path '{0}' starts with a '\\' which is not allowed.");
            _validator.Ensure<InvalidPathException>(!HasEndingBackslash || HasFilename, "The path '{0}' ends with a '\\' which is not allowed.");
            if (HasDrive && _relativePath.Length > 1 && !HasLeadingBackslash)
            {
                _validator.AddError<InvalidPathException>("Expected a '\\' after the drive but found a '{1}' in the path '{0}'.", _relativePath.First());
            }
        }

        private string Canonify(string relativePath)
        {
            if (relativePath.Length == 0) return relativePath;

            var folders = TokenizeToFolders(relativePath);
            return folders.Canonify().MergeToPath();
            
        }

        private IEnumerable<string> TokenizeToFolders(string relativePath)
        {
            var folders = relativePath.Split(new[] { '\\' });
            foreach (string folder in folders)
            {
                ValidateFolderName(folder);
                yield return folder;
            }
        }

        private void ValidateFolderName(string folder)
        {
            _validator.Ensure<InvalidPathException>(folder.Length > 0, "The path '{0}' contains an empty folder which is not allowed");
            if (_validator.HasError) return; // to avoid empty string logic below
            _validator.Ensure<InvalidPathException>(!char.IsWhiteSpace(folder.Last()), "The folder '{1}' in the path '{0}' ends with a whitespace which is not allowed.", folder);
            _validator.Ensure<InvalidPathException>(!folder.EndsWith(".") || folder == "." || folder == "..", "The folder '{1}' in the path '{0}' ends with a dot which is not allowed.", folder);

            FolderUtils.ValidatePathCharacters(folder, "folder", _validator);
        }

        private void ValidateAscensionLevel()
        {
            _validator.Ensure<InvalidPathException>(!_canonifiedPathWithoutBackslashes.StartsWith(".."), "The path '{0}' ascends into a folder above the root level.");
        }

        public static FoldersParser3 WithDriveAndFilename(string relativePath, Validator validator)
        {
            return new FoldersParser3(relativePath, validator, true, true);
        }

        public static FoldersParser3 WithDrive(string relativePath, Validator validator)
        {
            return new FoldersParser3(relativePath, validator, true, false);
        }
    }
}