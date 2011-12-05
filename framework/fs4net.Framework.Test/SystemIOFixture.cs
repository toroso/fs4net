using System;
using System.IO;
using fs4net.TestTemplates;
using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class SystemIOFixture
    {
        private string _tempFile;

        [SetUp]
        public void SetUp()
        {
            _tempFile = Path.Combine(Path.GetTempPath(), "mytempfile.tmp");
            using (System.IO.File.Create(_tempFile)) { }
        }

        [TearDown]
        public void TearDown()
        {
            System.IO.File.Delete(_tempFile);
        }

        [Test]
        public void GetPathRoot()
        {
            Should.Throw<ArgumentException>(() => Path.GetPathRoot(string.Empty));
            Should.Throw<ArgumentException>(() => Path.GetPathRoot(@"c :\space\between\letter\and\colon.txt"));
            Assert.That(Path.GetPathRoot(@"c:\path\to\file.txt"), Is.EqualTo(@"c:\"));
            Assert.That(Path.GetPathRoot(@"c:\path\to\"), Is.EqualTo(@"c:\"));
            Assert.That(Path.GetPathRoot(@"c: \space\after\colon\to\file.txt"), Is.EqualTo(@"c:\"));
            Assert.That(Path.GetPathRoot(@"c:\\doubleslash\path\to\"), Is.EqualTo(@"c:\"));
            Assert.That(Path.GetPathRoot(@"c:"), Is.EqualTo(@"c:"));
            Assert.That(Path.GetPathRoot(@"relative\path\to\file.txt"), Is.Empty);
            Assert.That(Path.GetPathRoot(@"..\relative\path\to\file.txt"), Is.Empty);
            Assert.That(Path.GetPathRoot(@".\relative\path\to\file.txt"), Is.Empty);
            Assert.That(Path.GetPathRoot(@"\relative\path\to\file.txt"), Is.EqualTo(@"\"));
            Assert.That(Path.GetPathRoot(@"\\network\path\to\file.txt"), Is.EqualTo(@"\\network\path"));
            Assert.That(Path.GetPathRoot(@"\\network"), Is.EqualTo(@"\\network"));
            Assert.That(Path.GetPathRoot(@"c:/path/to/file.txt"), Is.EqualTo(@"c:\"));
            Assert.That(Path.GetPathRoot(@"relative/path/to/file.txt"), Is.Empty);
            Assert.That(Path.GetPathRoot(@"/relative/path/to/file.txt"), Is.EqualTo(@"\"));
            Assert.That(Path.GetPathRoot(@"//network/path/to/file.txt"), Is.EqualTo(@"\\network\path"));
        }

        [Test]
        public void IsPathRooted()
        {
            Assert.That(Path.IsPathRooted(@"c:\path\to\file.txt"), Is.True);
            Assert.That(Path.IsPathRooted(@"c:\path\to\"), Is.True);
            Assert.That(Path.IsPathRooted(@"c:\\doubleslash\path\to\"), Is.True);
            Assert.That(Path.IsPathRooted(@"c:"), Is.True);
            Assert.That(Path.IsPathRooted(@"relative\path\to\file.txt"), Is.False);
            Assert.That(Path.IsPathRooted(@"..\relative\path\to\file.txt"), Is.False);
            Assert.That(Path.IsPathRooted(@".\relative\path\to\file.txt"), Is.False);
            Assert.That(Path.IsPathRooted(@"\relative\path\to\file.txt"), Is.True);
            Assert.That(Path.IsPathRooted(@"\\network\path\to\file.txt"), Is.True);
            Assert.That(Path.IsPathRooted(@"\\network"), Is.True);
            Assert.That(Path.IsPathRooted(@"c:/path/to/file.txt"), Is.True);
            Assert.That(Path.IsPathRooted(@"relative/path/to/file.txt"), Is.False);
            Assert.That(Path.IsPathRooted(@"//network/path/to/file.txt"), Is.True);
        }

        [Test]
        public void ListDrives()
        {
            DriveInfo.GetDrives().ForEach(Console.WriteLine);
        }

        [Test]
        public void FileAndDirectoryNames()
        {
            Should.NotThrow(() => new FileInfo(@"c:\path\to\file.txt"));
            Should.NotThrow(() => new FileInfo(@"c:\path\to\file."));
            Should.NotThrow(() => new FileInfo(@"c:\path\to\"));
            Should.NotThrow(() => new FileInfo(@"c:\path\to\."));
            Should.NotThrow(() => new FileInfo(@"c:\path\to\.."));

            Assert.That(Path.GetFileName(@"c:\path\to\file.txt"), Is.EqualTo(@"file.txt"));
            Assert.That(Path.GetFileName(@"c:\path\to\file"), Is.EqualTo(@"file"));
            Assert.That(Path.GetFileName(@"c:\path\to\file."), Is.EqualTo(@"file."));
            Assert.That(Path.GetFileName(@"c:\path\to\.txt"), Is.EqualTo(@".txt"));
            Assert.That(Path.GetFileName(@"c:\path\to\"), Is.Empty);
            Assert.That(Path.GetFileName(@"c:\path\to\."), Is.EqualTo(@"."));
            Assert.That(Path.GetFileName(@"c:\path\to\.."), Is.EqualTo(@".."));

            Assert.That(Path.GetFileName(string.Empty), Is.Empty);
            Assert.That(Path.GetFileName(@"\"), Is.Empty);
            Assert.That(Path.GetFileName(@"."), Is.EqualTo(@"."));
            Assert.That(Path.GetFileName(@".."), Is.EqualTo(@".."));
            Assert.That(Path.GetFileName(@"file"), Is.EqualTo(@"file"));

            Assert.That(Path.GetFileNameWithoutExtension(@"file.txt"), Is.EqualTo(@"file"));
            Assert.That(Path.GetFileNameWithoutExtension(@"file."), Is.EqualTo(@"file"));
            Assert.That(Path.GetFileNameWithoutExtension(@"file"), Is.EqualTo(@"file"));
            Assert.That(Path.GetFileNameWithoutExtension(@"file.with.dots."), Is.EqualTo(@"file.with.dots"));
            Assert.That(Path.GetFileNameWithoutExtension(@"file ."), Is.EqualTo(@"file "));
            Assert.That(Path.GetFileNameWithoutExtension(@"file.."), Is.EqualTo(@"file."));
            Assert.That(Path.GetFileNameWithoutExtension(@"..txt"), Is.EqualTo(@"."));
            Assert.That(Path.GetFileNameWithoutExtension(@".txt"), Is.Empty);
            Assert.That(Path.GetFileNameWithoutExtension(string.Empty), Is.Empty);

            Assert.That(Path.GetExtension(@"file.txt"), Is.EqualTo(@".txt"));
            Assert.That(Path.GetExtension(@"file."), Is.Empty);
            Assert.That(Path.GetExtension(@"file"), Is.Empty);
            Assert.That(Path.GetExtension(@"file.with.dots."), Is.Empty);
            Assert.That(Path.GetExtension(@"file ."), Is.Empty);
            Assert.That(Path.GetExtension(@"file.."), Is.Empty);
            Assert.That(Path.GetExtension(@"..txt"), Is.EqualTo(@".txt"));
            Assert.That(Path.GetExtension(@".txt"), Is.EqualTo(@".txt"));
            Assert.That(Path.GetExtension(string.Empty), Is.Empty);

            Assert.That(Path.GetDirectoryName(@"c:\path\to\file.txt"), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\file"), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\file."), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\.txt"), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\"), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\."), Is.EqualTo(@"c:\path\to"));
            Assert.That(Path.GetDirectoryName(@"c:\path\to\.."), Is.EqualTo(@"c:\path\to"));

            Should.Throw<ArgumentException>(() => Path.GetDirectoryName(string.Empty));
            //Assert.That(Path.GetDirectoryName(@"\"), Is.Empty);
            Assert.That(Path.GetDirectoryName(@"."), Is.Empty);
            Assert.That(Path.GetDirectoryName(@".."), Is.Empty);
            Assert.That(Path.GetDirectoryName(@"file"), Is.Empty);
        }

        [Test]
        public void ListInvalidPathChars()
        {
            Path.GetInvalidPathChars().ForEach(Console.WriteLine); // result: " < > |
        }

        [Test]
        public void ListInvalidFilenameChars()
        {
            Path.GetInvalidFileNameChars().ForEach(Console.WriteLine); // result: " < > |
        }

        [Test]
        public void LastWriteTimeTime()
        {
            Assert.That(System.IO.Directory.GetLastWriteTime(@"c:\this\path\clearly\does\not\exist"), Is.EqualTo(DateTime.FromFileTime(0)));
            Should.NotThrow(() => System.IO.Directory.GetLastWriteTime(@"c:\windows\regedit.exe"));

            //DateTime minimum = new DateTime(1601, 1, 1, 0, 0, 0);
            DateTime minimum = new DateTime(1601, 1, 1, 0, 0, 0).AddMilliseconds(1).ToLocalTime();
            SetLastWriteTimeWorksFine(minimum, "Higher");
            SetLastWriteTimeFails(minimum.AddMilliseconds(-1), "Lower");

            DateTime maximum = DateTime.MaxValue;
            SetLastWriteTimeWorksFine(maximum, "Lower");
        }

        private void SetLastWriteTimeWorksFine(DateTime at, string message)
        {
            Should.NotThrow(() => System.IO.Directory.SetLastWriteTime(_tempFile, at), message);
            Assert.That(System.IO.Directory.GetLastWriteTime(_tempFile), Is.EqualTo(at), message);
        }

        private void SetLastWriteTimeFails(DateTime at, string message)
        {
            Should.NotThrow(delegate
                {
                    try
                    {
                        System.IO.Directory.SetLastWriteTime(_tempFile, at);
                        Assert.That(System.IO.Directory.GetLastWriteTime(_tempFile), Is.Not.EqualTo(at), message);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // Fine!
                    }
                }, message);
        }

        [Test]
        [Ignore("Only works on my computer")]
        public void ValidPaths()
        {
            //using (File.Create(@"D:\crap\fs4net\fi*le.txt")) { }
            System.IO.Directory.CreateDirectory(@"D:\crap\fs4net\ space");

            using (System.IO.File.Create(@"D:\crap\fs4net\endingspace ")) { }
            Assert.IsTrue(System.IO.File.Exists(@"D:\crap\fs4net\endingspace"));

            using (System.IO.File.Create(@"D:\crap\fs4net\ending.space ")) { }
            Assert.IsTrue(System.IO.File.Exists(@"D:\crap\fs4net\ending.space"));

            using (System.IO.File.Create(@"D:\crap\fs4net\ beginspace")) { }
            Assert.IsTrue(System.IO.File.Exists(@"D:\crap\fs4net\ beginspace"));

            using (System.IO.File.Create(@"D:\crap\fs4net\emptyextension.")) { }
            Assert.IsTrue(System.IO.File.Exists(@"D:\crap\fs4net\emptyextension"));
        }

        [Test]
        [Ignore("Only works on my computer")]
        public void DriveFormats()
        {
            Console.WriteLine("c: -- '{0}'", new DriveInfo("c:").DriveFormat);
            Console.WriteLine("d: -- '{0}'", new DriveInfo("d:").DriveFormat);
            Console.WriteLine("k: -- '{0}'", new DriveInfo("k:").DriveFormat);
            //Console.WriteLine("z: -- '{0}'", new DriveInfo("z:").DriveFormat);

            Console.WriteLine("c: -- '{0}'", new DriveInfo("c:").DriveType);
            Console.WriteLine("d: -- '{0}'", new DriveInfo("d:").DriveType);
            Console.WriteLine("k: -- '{0}'", new DriveInfo("k:").DriveType);
            //Console.WriteLine("z: -- '{0}'", new DriveInfo("z:").DriveType);
        }
    }
}