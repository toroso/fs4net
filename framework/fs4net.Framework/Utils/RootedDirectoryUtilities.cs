using fs4net.Framework.Impl;

namespace fs4net.Framework.Utils
{
    public static class RootedDirectoryUtilities
    {
        /// <summary>
        /// NOTE: Experimental. Subject to change.
        /// Copies the directory and all its contents to a new location. The new copy will have the name specified by
        /// the destination parameter.
        /// This operation is not atomic. This means that if the operation fails halfways (e.g. out of disk space) half
        /// of the destination files and directories will exist.
        /// </summary>
        /// TODO: Exceptions
        public static void CopyTo(this RootedDirectory me, RootedDirectory destination)
        {
            // TODO: Shouldn't me be IRootedDirectory?
            // TODO: Isn't it ok if destination exists? What if you want to copy c: to d:?
            me.VerifyOnSameFileSystemAs(destination);
            me.VerifyIsNotAFile(ThrowHelper.DirectoryNotFoundException("Can't copy the directory '{0}' since it denotes a file.", me));
            me.VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't copy the directory '{0}' since it does not exist.", me));
            destination.Parent().VerifyIsADirectory(ThrowHelper.DirectoryNotFoundException("Can't copy the directory since the destination's parent directory '{0}' does not exist.", destination.Parent()));
            destination.VerifyIsNotAFile(ThrowHelper.IOException("Can't copy the directory to the destination '{0}' since a file with that name already exists.", destination));
            destination.VerifyIsNotADirectory(ThrowHelper.IOException("Can't copy the directory to the destination '{0}' since a directory with that name already exists.", destination));
            me.VerifyIsNotTheSameAs(destination, ThrowHelper.IOException("Can't copy the directory '{0}' since the source and destination denotes the same directory.", destination));
            me.VerifyIsNotAParentOf(destination, ThrowHelper.IOException("Can't copy the directory to the destination '{0}' since it is located inside the source directory.", destination));

            me.InternalCopyTo(destination);
        }

        private static void InternalCopyTo(this RootedDirectory source, RootedDirectory destination)
        {
            destination.Create();
            foreach (var file in source.Files())
            {
                file.CopyTo(destination + file.FileName());
            }
            foreach (var directory in source.Directories())
            {
                directory.InternalCopyTo(destination + directory.LeafFolder());
            }
        }
    }
}