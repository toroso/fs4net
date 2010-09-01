fs4net
======

fs4net is an encapsulation of the .NET framework file system functionality. If your application needs to access the file system you might want to consider using fs4net.

The .NET framework leaves much to ask for when it comes to working with the file system. fs4net tries to amend at least some of these shortcomings.

Goals
-----

The goals of fs4net are the following:

   * Support automatic testing by allowing you to have e.g. an in-memory file
     system in your test fixture and a real file system for your production
     code.
   * Avoid primitive obsession by representing paths by Value Objects.
   * Enable the user to adhere to good programming practices by using
     exceptions for exceptional situations only.
   * Provide good and specific exceptions when things go wrong.


Usage
-----

Using fs4net is a piece of cake. First you need to create a file system instance. Here's how you do it in your production code:

    IFileSystem fs = new FileSystem();

This line should preferrably be in the Main() method of your application, or better yet, be created by your IOC container. It's a good thing to have a single instance in your application, although there's nothing that actually stops you from having several.

Now that you have a file system you might want to do something with it, like create a directory. Tada:

    RootedDirectory dir = fs.CreateDirectoryDescribing(@"c:\my\path\to\heaven");
    dir.Create();

Here, the CreateDirectoryDescribing() method creates a reference to the directory, but it does not actually create the directory. You could say that the RootedDirectory is nothing but a fancy string containing the given path.

The Create() method on the other hand does create the directory. You could say corresponds to the .NET framework (static) method Directory.CreateDirectory().

The RootedDirectory class has got all kinds of handy methods on it, pretty much the same that you can find on the Directory and DirectoryInfo classes together.

The RootedDirectory has some sibling classes for representing other file system entities:

<table>
  <tr>
    <td>Drive</td><td>A mapped drive or a network share.</td>
  </tr><tr>
    <td>RootedDirectory</td><td>A path to a directory that starts with a drive.</td>
  </tr><tr>
    <td>RelativeDirectory</td><td>A path to a directory that doesn't start with a drive.</td>
  </tr><tr>
    <td>RootedFile</td><td>A path to a file that starts with a drive.</td>
  </tr><tr>
    <td>RelativeFile</td><td>A path to a file that doesn't start with a drive.</td>
  </tr><tr>
    <td>FileName</td><td>The name of a file.</td>
  </tr>
</table>

The rooted entities (including Drive) are either created from the file system like in the example above, or created from other rooted entities.

The relative entities (including FileName) are created using factory methods. For example:

    RelativeFile file = RelativeFile.FromString(@"path\to\file.txt");


Testing
-------

The biggest advantage of using fs4net is that it's really easy to mock the file system in automatic tests. The only thing you need to change is how you create your file system:

    IFileSystem fs = new MemoryFileSystem().WithDrives("c:", "d:");

This memory file system is a so-called [Fake Object](http://xunitpatterns.com/Fake%20Object.html): it works exactly the same as the real one except for that the folder structure and the file contents are cached in memory.

To start with, the memory file system is empty. With the WithDrives() thingy you tell it what drives it has, but you might want to populate it with files and folders as well. This is done with a FileSystemBuilder:

    FileSystemBuilder populateFileSystem = new FileSystemBuilder(fs);
    RootedFile file = populateFileSystem
        .WithFile(@"c:\path\to\file.txt")
        .Containing("Happy joy")
        .LastModifiedAt(DateTime.Now);
    RootedDirectory dir = populateFileSystem
        .WithDir(@"d:\another\path")
        .LastAccessedAt(DateTime.Now);

The builder operates through the normal file system interface so you can use it with the real FileSystem as well if you'd like.



Licensing and Copyright
-----------------------

Copyright (c) 2010 Torbj&ouml;rn Kalin.

Released under the [Apache 2.0 license](http://www.apache.org/licenses/LICENSE-2.0.html). (Briefly that means that you can do whatever you want with it at long as you give me credit and don't blame me if anything goes wrong. But please read the origial.)
