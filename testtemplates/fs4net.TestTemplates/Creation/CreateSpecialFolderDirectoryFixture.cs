using System;
using System.Text;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Creation
{
    public abstract class CreateSpecialFolderDirectoryFixture
    {
        protected IFileSystem FileSystem { get; private set; }

        [SetUp]
        public void SetUp()
        {
            FileSystem = CreateFileSystem();
        }

        [TearDown]
        public void TearDownFileSystem()
        {
            DisposeFileSystem(FileSystem);
        }

        protected abstract IFileSystem CreateFileSystem();
        protected virtual void DisposeFileSystem(IFileSystem fileSystem) { }

        [Test]
        public void CreateAllSpecialFolders()
        {
            var iterator = new FolderIterator(FileSystem);
            iterator.CheckAll();
            if (iterator.HasFailures)
            {
                Assert.Fail(iterator.FailuresAsString);
            }
        }

        private class FolderIterator
        {
            private readonly IFileSystem _fileSystem;
            private readonly StringBuilder _failures = new StringBuilder();

            public FolderIterator(IFileSystem fileSystem)
            {
                _fileSystem = fileSystem;
            }

            public bool HasFailures
            {
                get { return _failures.Length > 0; }
            }

            public string FailuresAsString
            {
                get { return _failures.ToString(); }
            }

            public void CheckAll()
            {
                foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
                {
                    if (folder != Environment.SpecialFolder.MyComputer) // fs4net can't handle MyComputer... and neither can System.IO!
                    {
                        CheckFolder(folder);
                    }
                }
            }

            private void CheckFolder(Environment.SpecialFolder folder)
            {
                try
                {
                    var directory = _fileSystem.DirectoryDescribingSpecialFolder(folder);
                    if (!directory.Exists())
                    {
                        AddFailure(string.Format("The path '{0}' for '{1}' does not exist", directory, folder));
                    }
                }
                catch (Exception ex)
                {
                    AddFailure(string.Format("Failed to create '{0}': {1} - '{2}'", folder, ex.GetType(), ex.Message));
                }
            }

            private void AddFailure(string failMessage)
            {
                _failures.AppendLine(failMessage);
            }
        }

        [Test]
        public void CreateMyComputer()
        {
            Should.Throw<NotSupportedException>(() => FileSystem.DirectoryDescribingSpecialFolder(Environment.SpecialFolder.MyComputer));
        }
    }
}