using System;
using System.Collections.Generic;
using System.IO;

namespace fs4net.Framework
{
    public interface IInternalFileSystem
    {
        /// <summary>Note: Subject to be removed.</summary>
        RootedFile FileDescribing(string fullPath);
        /// <summary>Note: Subject to be removed.</summary>
        RootedDirectory DirectoryDescribing(string fullPath);
        /// <summary>Note: Subject to be removed.</summary>
        RootedDirectory DirectoryDescribingTemporaryDirectory();
        /// <summary>Note: Subject to be removed.</summary>
        RootedDirectory DirectoryDescribingCurrentDirectory();
        /// <summary>Note: Subject to be removed.</summary>
        RootedDirectory DirectoryDescribingSpecialFolder(Environment.SpecialFolder folder);
        /// <summary>Note: Subject to be removed.</summary>
        Drive DriveDescribing(string driveName);
        /// <summary>Note: Subject to be removed.</summary>
        IEnumerable<Drive> AllDrives();

        bool IsFile(RootedCanonicalPath path);
        bool IsDirectory(RootedCanonicalPath path);

        long GetFileSize(RootedCanonicalPath path);

        DateTime GetFileLastWriteTime(RootedCanonicalPath path);
        void SetFileLastWriteTime(RootedCanonicalPath path, DateTime at);
        DateTime GetDirectoryLastWriteTime(RootedCanonicalPath path);
        void SetDirectoryLastWriteTime(RootedCanonicalPath path, DateTime at);
        DateTime GetFileLastAccessTime(RootedCanonicalPath path);
        void SetFileLastAccessTime(RootedCanonicalPath path, DateTime at);
        DateTime GetDirectoryLastAccessTime(RootedCanonicalPath path);
        void SetDirectoryLastAccessTime(RootedCanonicalPath path, DateTime at);

        IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path);
        IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path);

        void CreateDirectory(RootedCanonicalPath path);

        void DeleteFile(RootedCanonicalPath path);
        void DeleteDirectory(RootedCanonicalPath path, bool recursive);

        void MoveFile(RootedCanonicalPath source, RootedCanonicalPath destination);
        void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination);

        void CopyFile(RootedCanonicalPath source, RootedCanonicalPath destination);
        void CopyAndOverwriteFile(RootedCanonicalPath source, RootedCanonicalPath destination);

        /// <summary>Note: Might be replaced with a CreateStream() method.</summary>
        Stream CreateReadStream(RootedCanonicalPath path);
        /// <summary>Note: Might be replaced with a CreateStream() method.</summary>
        Stream CreateWriteStream(RootedCanonicalPath path);
        /// <summary>Note: Might be replaced with a CreateStream() method.</summary>
        Stream CreateAppendStream(RootedCanonicalPath path);
        /// <summary>Note: Might be replaced with a CreateStream() method.</summary>
        Stream CreateModifyStream(RootedCanonicalPath path);

        void SetAsCurrent(RootedCanonicalPath path);

        // <summary>
        // Should return true if the drive is ready to use. A drive that is not ready could be for example an ejected
        // USB device, a CD-ROM device without disk or a disconnected network device.
        // </summary>
        // <param name="drive">Contains the name of the drive/device without ending backslash. Examples are c: or
        // \\network\drive.</param>
//        bool IsReady(DriveName drive);
    }
}