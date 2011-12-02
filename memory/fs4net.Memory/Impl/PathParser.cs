using System;
using System.Linq;
using fs4net.Memory.Node;

namespace fs4net.Memory.Impl
{
    internal class PathParser
    {
        private readonly string[] _fileSystemNodeNames;

        internal PathParser(string path)
        {
            //if (path == string.Empty) throw new ArgumentException("Path is empty", "path");
            _fileSystemNodeNames = ParseFileSystemNodeNames(path);
        }

        private string[] ParseFileSystemNodeNames(string path)
        {
            return path.Split('\\');
        }

        internal void WithEachFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames)
            {
                action(nodeName);
            }
        }

        internal void WithEachButFirstFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames.Skip(1))
            {
                action(nodeName);
            }
        }

        internal void WithEachButLastFileSystemNodeNameDo(Action<string> action)
        {
            foreach (string nodeName in _fileSystemNodeNames.Take(_fileSystemNodeNames.Length - 1))
            {
                action(nodeName);
            }
        }

        internal void WithFirstFileSystemNodeNameDo(Action<string> action)
        {
            action(_fileSystemNodeNames.First());
        }

        internal TResult WithLastFileSystemNodeNameDo<TResult>(Func<string, TResult> action)
        {
            return action(_fileSystemNodeNames.Last());
        }

        internal FolderNode GetParentNode(FolderNode rootNode)
        {
            var result = rootNode;
            WithEachButLastFileSystemNodeNameDo(folderName => result = (FolderNode)result.FindChildNodeNamed(folderName));
            return result;
        }

        internal string GetLeafNodeName()
        {
            return _fileSystemNodeNames.Last();
        }
    }
}