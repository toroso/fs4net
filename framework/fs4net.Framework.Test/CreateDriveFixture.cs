using System;
using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class CreateDriveFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        [Test]
        public void Throws_If_FileSystem_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new Drive(null, "c:"));
        }
        
        [Test]
        public void Throws_If_Name_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => _fileSystem.CreateDriveDescribing(null));
        }

        [Test]
        public void Throws_If_Name_Is_Empty()
        {
            Assert.Throws<InvalidPathException>(() => _fileSystem.CreateDriveDescribing(string.Empty));
        }


        private static readonly string[] InvalidDrives =
            {
                @"c",
                @"cc",
                @"c:\",
                @"ö:",
                @"c:c",
                @"\\colon:in\network\name",
                @"\\colon\in:share\name",
                @"\\network",
                @"\\network\",
            };

        [Test]
        public void Throws_If_Is_Invalid()
        {
            InvalidDrives.ForEach(Throws_If_Is_Invalid);
        }

        [Test, TestCaseSource("InvalidDrives")]
        public void Throws_If_Is_Invalid(string invalidDrive)
        {
            AssertThrowsInvalidPathExceptionFor(invalidDrive);
        }


        private static readonly string[] ValidDrives =
            {
                @"c:",
                @"C:",
                @"d:",
                @"z:",
                @"\\network\name",
            };

        [Test]
        public void Create_With_Valid_Drive()
        {
            ValidDrives.ForEach(Create_With_Valid_Drive);
        }

        [Test, TestCaseSource("ValidDrives")]
        public void Create_With_Valid_Drive(string validDrive)
        {
            Assert.DoesNotThrow(() => _fileSystem.CreateDriveDescribing(validDrive), string.Format("for '{0}'", validDrive));
        }


        private void AssertThrowsInvalidPathExceptionFor(string invalidDrive)
        {
            Assert.Throws<InvalidPathException>(() => _fileSystem.CreateDriveDescribing(invalidDrive), string.Format("for '{0}'", invalidDrive));
        }
    }
}