using System;
using System.Collections.Generic;
using System.IO;

namespace fs4net.Framework
{
    public interface IInternalFileSystem : IFileSystem
    {
        bool IsFile(RootedCanonicalPath path);
        bool IsDirectory(RootedCanonicalPath path);

        DateTime GetFileLastModified(RootedCanonicalPath path);
        void SetFileLastModified(RootedCanonicalPath path, DateTime at);
        DateTime GetDirectoryLastModified(RootedCanonicalPath path);
        void SetDirectoryLastModified(RootedCanonicalPath path, DateTime at);
        DateTime GetFileLastAccessed(RootedCanonicalPath path);
        void SetFileLastAccessed(RootedCanonicalPath path, DateTime at);
        DateTime GetDirectoryLastAccessed(RootedCanonicalPath path);

        IEnumerable<RootedFile> GetFilesInDirectory(RootedCanonicalPath path);
        IEnumerable<RootedDirectory> GetDirectoriesInDirectory(RootedCanonicalPath path);

        void CreateDirectory(RootedCanonicalPath path);

        void DeleteFile(RootedCanonicalPath path);
        void DeleteDirectory(RootedCanonicalPath path, bool recursive);

        void MoveDirectory(RootedCanonicalPath source, RootedCanonicalPath destination);

        Stream CreateReadStream(RootedCanonicalPath path);
        Stream CreateWriteStream(RootedCanonicalPath path);

        /// <summary>
        /// Should return true if the drive is ready to use. A drive that is not ready could be for example an ejected
        /// USB device, a CD-ROM device without disk or a disconnected network device.
        /// </summary>
        /// <param name="drive">Contains the name of the drive/device without ending backslash. Examples are c: or
        /// \\network\drive.</param>
//        bool IsReady(DriveName drive);
    }
}