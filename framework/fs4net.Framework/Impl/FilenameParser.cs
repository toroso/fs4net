using System;
using System.IO;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal class FilenameParser
    {
        private readonly string _pathWithFilename;
        private readonly Validator _validator;
        private readonly string _filename;

        public FilenameParser(string pathWithFilename, Validator validator)
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
            _validator.Ensure<InvalidPathException>(!Char.IsWhiteSpace(_filename.Last()), "The filename of the path '{0}' ends with a whitespace which is not allowed.");
            _validator.Ensure<InvalidPathException>(!_filename.EndsWith("."), "The path '{0}' has an empty extension, which is not allowed.");
        }

        private void ValidateFilenameCharacters()
        {
            FolderUtils.ValidatePathCharacters(_filename, "filename", _validator);
        }

        private void ValidateExtension()
        {
            var extension = Path.GetExtension(_filename);
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(_filename);
            _validator.Ensure<InvalidPathException>(!filenameWithoutExtension.IsEmpty() || !extension.IsEmpty(), "The filename of the path '{0}' is empty.");
            _validator.Ensure<InvalidPathException>(filenameWithoutExtension.IsEmpty() || !Char.IsWhiteSpace(filenameWithoutExtension.Last()), "The filename of the path '{0}' ends with a whitespace which is not allowed.");
        }

        public string AppendTo(string pathWithoutFilename)
        {
            return pathWithoutFilename + _filename;
        }
    }
}