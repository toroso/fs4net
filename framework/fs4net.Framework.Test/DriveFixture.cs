using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class DriveFixture
    {
        private IFileSystem _fileSystem;


        [SetUp]
        public void CreateMockFileSystem()
        {
            _fileSystem = new MockFileSystem();
        }


        [Test]
        public void Mapped_Drive_Name_Is_Intact()
        {
            AssertRemainsIntact("c:");
        }

        [Test]
        public void UpperCase_Mapped_Drive_Name_Is_Intact()
        {
            AssertRemainsIntact("C:");
        }

        [Test]
        public void Network_Drive_Name_Is_Intact()
        {
            AssertRemainsIntact(@"\\network\share");
        }

        private void AssertRemainsIntact(string driveName)
        {
            Assert.That(_fileSystem.DriveDescribing(driveName).Name, Is.EqualTo(driveName));
        }
    }
}