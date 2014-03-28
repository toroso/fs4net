using NUnit.Framework;

namespace fs4net.Framework.Test.Directory
{
    [TestFixture]
    public class CurrentDirectoryFixture
    {
        [Test]
        public void Application_Current_Directory_Does_Not_Change_When_Changing_FileSystem_Current_Directory()
        {
            var applicationDirectory = System.IO.Directory.GetCurrentDirectory();
            var fileSystem = new FileSystem();
            var newWorkingDir = fileSystem.DirectoryDescribingTemporaryDirectory();
            newWorkingDir.SetAsCurrent();
            Assert.AreEqual(applicationDirectory, System.IO.Directory.GetCurrentDirectory());
        }

        [Test]
        public void FileSystem_Current_Directory_Does_Not_Change_When_Changing_Application_Current_Directory()
        {
            var applicationDirectory = System.IO.Directory.GetCurrentDirectory();

            try
            {
                var fileSystem = new FileSystem();
                System.IO.Directory.SetCurrentDirectory(System.IO.Path.GetTempPath());
                Assert.AreEqual(applicationDirectory, fileSystem.DirectoryDescribingCurrentDirectory().PathAsString);
            }
            finally
            {
                // NUnit seems to restore the directory after the test, but it is not a documented feature. At least restore current directory 
                // to avoid warnings about tests changing current directory. 
                System.IO.Directory.SetCurrentDirectory(applicationDirectory);
            }
        }

        [Test]
        public void Different_Instances_Can_Have_Different_Current_Directories()
        {
            var fs1 = new FileSystem();
            var fs2 = new FileSystem();
            var fs2CurrentDirectory = fs2.DirectoryDescribingTemporaryDirectory();
            fs2CurrentDirectory.SetAsCurrent();
            Assert.AreNotEqual(fs1.DirectoryDescribingCurrentDirectory().PathAsString, fs2.DirectoryDescribingCurrentDirectory().PathAsString);
            Assert.AreEqual(System.IO.Directory.GetCurrentDirectory(), fs1.DirectoryDescribingCurrentDirectory().PathAsString);
        }
    }
}