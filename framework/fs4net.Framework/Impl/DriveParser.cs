using System;
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
            if (hostName == String.Empty)
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
            if (shareName == String.Empty)
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

        /// <summary>Return the valid drive name, or null if IsValid returns false or string.Empty if Exists return false.</summary>
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
}