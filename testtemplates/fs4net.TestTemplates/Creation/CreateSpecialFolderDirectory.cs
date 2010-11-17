using System;
using System.Text;
using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.TestTemplates.Creation
{
    public abstract class CreateSpecialFolderDirectory
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
            var failures = new StringBuilder();
            foreach (Environment.SpecialFolder folder in Enum.GetValues(typeof(Environment.SpecialFolder)))
            {
                if (folder != Environment.SpecialFolder.MyComputer) // fs4net can't handle MyComputer... and neither can System.IO!
                {
                    try
                    {
                        FileSystem.DirectoryDescribingSpecialFolder(folder);
                    }
                    catch (Exception ex)
                    {
                        failures.AppendLine(string.Format("Failed to create '{0}': {1} - '{2}'", folder, ex.GetType(),
                                                          ex.Message));
                    }
                }
            }
            if (failures.Length > 0)
            {
                Assert.Fail(failures.ToString());
            }
        }

        [Test]
        public void CreateMyComputer()
        {
            Assert.Throws<NotSupportedException>(() => FileSystem.DirectoryDescribingSpecialFolder(Environment.SpecialFolder.MyComputer));
        }
    }
}