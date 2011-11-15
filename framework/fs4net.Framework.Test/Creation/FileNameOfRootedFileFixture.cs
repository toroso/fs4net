using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class FileNameOfRootedFileFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        private static readonly string[] RootedPaths =
            {
                @"c:\filename.txt",
                @"c:\standard\case\to\filename.txt",
                @"c:\single\.\dots\to\.\filename.txt",
                @"c:\double\..\dots\to\..\filename.txt",
                @"\\network\drive\filename.txt",
            };


        [Test, TestCaseSource("RootedPaths")]
        public void RootedFile_FileName(string path)
        {
            Assert.That(_fileSystem.FileDescribing(path).FileName().FullName, Is.EqualTo(@"filename.txt"));
        }
    }
}