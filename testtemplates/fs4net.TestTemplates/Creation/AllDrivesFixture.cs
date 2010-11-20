using System;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Creation
{
    public abstract class AllDrivesFixture
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
        public void GetAllDrives()
        {
            foreach (var drive in FileSystem.AllDrives())
            {
                // Could be a Removable or CDRom drive, and they do not exist...
                // TODO: When DriveType is supported, only do this for Fixed drives
                //Assert.True(drive.Exists(), string.Format("The drive '{0}' does not exist.", drive));
                if (!drive.Exists()) Console.WriteLine("The drive '{0}' does not exist.", drive);
            }
        }
    }
}