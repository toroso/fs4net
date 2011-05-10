using System;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal class DriveParser
    {
        private readonly string _rootedPath;
        private readonly Validator _validator;
        private readonly string _driveName;
        private readonly bool _isRooted;

        public DriveParser(string rootedPath, Validator validator)
        {
            _rootedPath = rootedPath;
            _validator = validator;
            _isRooted = true;

            _driveName = ExtractMappedDrive();
            if (_driveName != null) return;

            _driveName = ExtractNetworkDrive();
            if (_driveName != null) return;

            _isRooted = false;
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

        public bool Exists
        {
            get { return _isRooted; }
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
                if (!IsValid) return string.Empty;
            }
            return null;
        }

        private string ExtractDriveLetter(int positionOfFirstColon)
        {
            if (positionOfFirstColon == 1)
            {
                var pathRoot = _rootedPath.Substring(0, 2); // Extract drive name, including the colon
                ValidateDriveLetter(pathRoot.First());
                //if (IsValid)
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
                return IsValid
                    ? String.Format(@"\\{0}\{1}", hostName, shareName)
                    : string.Empty;
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
}