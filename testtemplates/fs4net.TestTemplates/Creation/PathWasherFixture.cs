using System;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Creation
{
    public abstract class PathWasherFixture
    {
        private IFileSystem FileSystem { get; set; }

        private bool _pathWasherHasBeenCalled;
        private Func<string, string> _mockWasher;

        [SetUp]
        public void SetUp()
        {
            _pathWasherHasBeenCalled = false;
            _mockWasher = delegate(string path)
                {
                    Assert.That(_pathWasherHasBeenCalled, Is.False, "PathWasher called twice.");
                    _pathWasherHasBeenCalled = true;
                    return path;
                };
            FileSystem = CreateFileSystem(_mockWasher);
        }

        [TearDown]
        public void TearDownFileSystem()
        {
            DisposeFileSystem(FileSystem);
        }

        protected abstract IFileSystem CreateFileSystem(Func<string, string> pathWasher);
        protected virtual void DisposeFileSystem(IFileSystem fileSystem) { }

        [Test]
        public void CreateFileWashesPath()
        {
            FileSystem.FileDescribing(@"c:\path\to\file.txt");
            Assert.That(_pathWasherHasBeenCalled, Is.True, "PathWasher never called.");
        }

        [Test]
        public void CreateDirectoryWashesPath()
        {
            FileSystem.FileDescribing(@"c:\path\to\file.txt");
            Assert.That(_pathWasherHasBeenCalled, Is.True, "PathWasher never called.");
        }
    }
}