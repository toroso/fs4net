using System.IO;

namespace fs4net.Framework.Impl
{
    internal sealed class CanonicalPathBuilder
    {
        private const int MaxPath = 259; // TODO Get from FileSystem

        private readonly string _fullPath;
        private readonly Validator _validator;
        private string _canonical;

        public CanonicalPathBuilder(string fullPath)
        {
            _fullPath = fullPath;
            ThrowHelper.ThrowIfNull(fullPath, "fullPath");
            if (_fullPath == null) throw new InvalidPathException("The path is empty.");
            _validator = new Validator(_fullPath);
        }

        public static string GetDriveName(string fullPath)
        {
            var validator = new Validator(fullPath);
            var drive = new DriveParser(fullPath, validator);
            validator.ThrowOnError();
            return drive.AppendTo(string.Empty);
        }

        public void BuildForDrive()
        {
            InternalBuildForDrive();
            _validator.ThrowOnError();
        }

        private void InternalBuildForDrive()
        {
            var validator = new Validator(_fullPath);
            var drive = new DriveParser(_fullPath, validator);
            _canonical = drive.AppendTo(string.Empty);
            _validator.Ensure<InvalidPathException>(!validator.HasError, "The drive name '{0}' is not valid.");
            _validator.Ensure<InvalidPathException>(_canonical == _fullPath, "The drive name '{0}' is not valid.");
        }

        public bool IsRootedFile
        {
            get
            {
                InternalBuildForRootedFile();
                return !_validator.HasError;
            }
        }

        internal string BuildForRootedFile()
        {
            InternalBuildForRootedFile();
            _validator.ThrowOnError();
            return _canonical;
        }

        private void InternalBuildForRootedFile()
        {
            var drive = new DriveParser(_fullPath, _validator);
            if (_validator.HasError) return;

            var filename = new FilenameParser(drive.PathWithoutDrive, _validator);
            if (_validator.HasError) return;

            var folders = FoldersParser.WithDriveAndFilename(filename.PathWithoutFilename, _validator);
            if (_validator.HasError) return;

            _canonical = drive.AppendTo(filename.AppendTo(folders.Canonified));
            ValidateCanonicalPathLength();
        }

        public bool IsRootedDirectory
        {
            get
            {
                InternalBuildForRootedDirectory();
                return !_validator.HasError;
            }
        }

        public string BuildForRootedDirectory()
        {
            InternalBuildForRootedDirectory();
            _validator.ThrowOnError();
            return _canonical;
        }

        private void InternalBuildForRootedDirectory()
        {
            var drive = new DriveParser(_fullPath, _validator);
            if (_validator.HasError) return;

            var folders = FoldersParser.WithDrive(drive.PathWithoutDrive, _validator);
            if (_validator.HasError) return;

            _canonical = drive.AppendTo(folders.Canonified);
            ValidateCanonicalPathLength();
        }

        public bool IsRelativeFile
        {
            get
            {
                InternalBuildForRelativeFile();
                return !_validator.HasError;
            }
        }

        public string BuildForRelativeFile()
        {
            InternalBuildForRelativeFile();
            _validator.ThrowOnError();
            return _canonical;
        }

        private void InternalBuildForRelativeFile()
        {
            ValidatePathNotRooted();
            if (_validator.HasError) return;

            var filename = new FilenameParser(_fullPath, _validator);
            if (_validator.HasError) return;

            var folders = FoldersParser.WithFilename(filename.PathWithoutFilename, _validator);
            if (_validator.HasError) return;

            _canonical = filename.AppendTo(folders.Canonified);
            ValidateCanonicalPathLength(); // TODO: Should path length be validated for relative paths?
        }

        public bool IsRelativeDirectory
        {
            get
            {
                InternalBuildForRelativeDirectory();
                return !_validator.HasError;
            }
        }

        public string BuildForRelativeDirectory()
        {
            InternalBuildForRelativeDirectory();
            _validator.ThrowOnError();
            return _canonical;
        }

        private void InternalBuildForRelativeDirectory()
        {
            ValidatePathNotRooted();
            if (_validator.HasError) return;

            var folders = FoldersParser.WithFolderOnly(_fullPath, _validator);
            if (_validator.HasError) return;

            _canonical = folders.Canonified;
            ValidateCanonicalPathLength(); // TODO: Should path length be validated for relative paths?
        }

        private void ValidatePathNotRooted()
        {
            var noDriveValidator = new Validator(_fullPath);
            var drive = new DriveParser(_fullPath, noDriveValidator);
            _validator.Ensure<RootedPathException>(!drive.Exists, "The path '{0}' is rooted.");
            _validator.Ensure(!drive.Exists || !noDriveValidator.HasError, noDriveValidator);
        }

        public void BuildForFileName()
        {
            InternalBuildForFilename();
            _validator.ThrowOnError();
        }

        private void InternalBuildForFilename()
        {
            var filename = new FilenameParser(_fullPath, _validator);
            _canonical = filename.AppendTo(string.Empty);
            _validator.Ensure<InvalidPathException>(_canonical == _fullPath, "The filename '{0}' is not valid.");
        }

        private void ValidateCanonicalPathLength()
        {
            _validator.Ensure<PathTooLongException>(_canonical.Length <= MaxPath, "The path '{0}' contains '{1}' characters in canonical form. Maximum allowed is '{2}'.", _canonical.Length, MaxPath);
        }
    }
}