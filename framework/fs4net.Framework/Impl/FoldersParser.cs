using System.Collections.Generic;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal class FoldersParser
    {
        private readonly string _relativePath;
        private readonly Validator _validator;
        private readonly string _canonifiedPathWithoutBackslashes;
        private bool HasDrive { get; set; }
        private bool HasFilename { get; set; }

        public FoldersParser(string relativePath, Validator validator, bool hasDrive, bool hasFilename)
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
            _validator.Ensure<InvalidPathException>(!HasDrive || !_canonifiedPathWithoutBackslashes.StartsWith(".."), "The path '{0}' ascends into a folder above the root level.");
        }

        public static FoldersParser WithDriveAndFilename(string relativePath, Validator validator)
        {
            return new FoldersParser(relativePath, validator, true, true);
        }

        public static FoldersParser WithDrive(string relativePath, Validator validator)
        {
            return new FoldersParser(relativePath, validator, true, false);
        }

        public static FoldersParser WithFilename(string relativePath, Validator validator)
        {
            return new FoldersParser(relativePath, validator, false, true);
        }

        public static FoldersParser WithFolderOnly(string relativePath, Validator validator)
        {
            return new FoldersParser(relativePath, validator, false, false);
        }
    }
}