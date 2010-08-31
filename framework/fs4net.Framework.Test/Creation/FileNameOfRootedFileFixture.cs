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


        [Test]
        public void RootedFile_FileName()
        {
            RootedPaths.ForEach(RootedFile_FileName);
        }

        [Test, TestCaseSource("RootedPaths")]
        public void RootedFile_FileName(string path)
        {
            Assert.That(_fileSystem.CreateFileDescribing(path).FileName().FullName, Is.EqualTo(@"filename.txt"));
        }
    }
}