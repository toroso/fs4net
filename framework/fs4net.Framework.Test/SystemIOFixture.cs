using System;
using System.IO;
using NUnit.Framework;

namespace fs4net.Framework.Test
{
    [TestFixture]
    public class SystemIOFixture
    {
        [Test]
        public void GetPathRoot()
        {
            Assert.Throws<ArgumentException>(() => Path.GetPathRoot(string.Empty));
            Assert.Throws<ArgumentException>(() => Path.GetPathRoot(@"c :\space\between\letter\and\colon.txt"));
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
            Assert.DoesNotThrow(() => new FileInfo(@"c:\path\to\file.txt"));
            Assert.DoesNotThrow(() => new FileInfo(@"c:\path\to\file."));
            Assert.DoesNotThrow(() => new FileInfo(@"c:\path\to\"));
            Assert.DoesNotThrow(() => new FileInfo(@"c:\path\to\."));
            Assert.DoesNotThrow(() => new FileInfo(@"c:\path\to\.."));

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

            Assert.Throws<ArgumentException>(() => Path.GetDirectoryName(string.Empty));
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
        public void LastModifiedTime()
        {
            Assert.That(Directory.GetLastWriteTime(@"c:\this\path\clearly\does\not\exist"), Is.EqualTo(DateTime.FromFileTime(0)));
        }

        [Test]
        public void ValidPaths()
        {
            //using (File.Create(@"C:\Users\busen\temp\fi*le.txt")) { }
            Directory.CreateDirectory(@"C:\Users\busen\temp\ space");
        }
    }
}