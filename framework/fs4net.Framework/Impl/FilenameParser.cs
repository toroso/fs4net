using System;
using System.IO;
using System.Linq;

namespace fs4net.Framework.Impl
{
    internal class FilenameParser
    {
        private readonly string _fullPath;
        private readonly string _filename;

        public FilenameParser(string fullPath)
        {
            _fullPath = fullPath;

            _filename = ExtractFilename();

            ValidateWhiteSpaces();
            ValidateFilenameCharacters();
            ValidateExtension();
        }

        private string ExtractFilename()
        {
            int lastBackslash = _fullPath.LastIndexOf('\\');
            return lastBackslash == -1
                       ? _fullPath
                       : _fullPath.Substring(lastBackslash + 1);
        }

        private void ValidateWhiteSpaces()
        {
            if (_filename == String.Empty) throw new InvalidPathException(String.Format("The filename of the path '{0}' is empty.", _fullPath));
            if (Char.IsWhiteSpace(_filename.First())) throw new InvalidPathException(String.Format("The filename of the path '{0}' starts with a whitespace which is not allowed.", _fullPath));
            if (Char.IsWhiteSpace(_filename.Last())) throw new InvalidPathException(String.Format("The filename of the path '{0}' ends with a whitespace which is not allowed.", _fullPath));
            if (_filename.EndsWith(".")) throw new InvalidPathException(String.Format("The path '{0}' has an empty extension, which is not allowed.", _fullPath));
        }

        private void ValidateFilenameCharacters()
        {
            string errorMessage = FolderUtils.ValidatePathCharacters(_filename, _fullPath, "filename");
            if (errorMessage != null) throw new InvalidPathException(errorMessage);
        }

        private void ValidateExtension()
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(_filename);
            if (filenameWithoutExtension == String.Empty) throw new InvalidPathException(String.Format("The filename of the path '{0}' is empty.", _fullPath));
            if (Char.IsWhiteSpace(filenameWithoutExtension.Last())) throw new InvalidPathException(String.Format("The filename of the path '{0}' ends with a whitespace which is not allowed.", _fullPath));
        }

        public string Filename
        {
            get { return _filename; }
        }
    }
}