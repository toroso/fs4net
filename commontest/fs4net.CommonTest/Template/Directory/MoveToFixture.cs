using fs4net.Framework;
using NUnit.Framework;

namespace fs4net.CommonTest.Template.Directory
{
    public abstract class MoveToFixture : PopulatedFileSystem
    {
        //[Test]
        //public void Change_Name_Of_Directory_Containing_FileSystemItems()
        //{
        //    var source = ParentOfExistingLeafDirectory;
        //    var destination = source.ParentDirectory() + RelativeDirectory.FromString("new name");
        //    source.MoveTo(destination);
        //    Assert.That(source.Exists(), Is.False);
        //    Assert.That(destination.Exists(), Is.True);
        //    var movedFile = destination + ExistingFile.RelativeFrom(source);
        //    Assert.That(movedFile.Exists(), Is.True);
        //}

        //* Source contains files and folders
        //* Non-existing source
        //* Source represents a file
        //* Non-existing destination
        //* Destination already exists (folder or file)
        //* Move into itself
        //* Different file systems throws
        //* Access denied (source and destination)
        //- Do not support only specifying destination folder. Should be new name!
    }
}