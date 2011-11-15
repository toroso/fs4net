using System;
using NUnit.Framework;

namespace fs4net.Framework.Test.Creation
{
    [TestFixture]
    public class FileNameOfRelativeFileFixture
    {
        private static readonly string[] RelativePaths =
            {
                @"filename.txt",
                @".\filename.txt",
                @"..\filename.txt",
                @"standard\case\to\filename.txt",
                @"single\.\dots\to\.\filename.txt",
                @"double\..\dots\to\..\filename.txt",
                @".\starting\with\single\dot\to\filename.txt",
                @"..\starting\with\double\dots\to\filename.txt",
            };


        [Test, TestCaseSource("RelativePaths")]
        public void RelativeFile_FileName(string path)
        {
            Console.WriteLine("Doing " + path);
            Assert.That(RelativeFile.FromString(path).FileName().FullName, Is.EqualTo(@"filename.txt"));
        }
    }
}