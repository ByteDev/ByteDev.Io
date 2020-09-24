[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Io?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Io/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Io.svg)](https://www.nuget.org/packages/ByteDev.Io)

# ByteDev.Io

Set of IO related .NET utility classes.

## Installation

ByteDev.Io has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Io is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Io`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Io/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Io/blob/master/docs/RELEASE-NOTES.md).

## Usage

To use these main public classes simply reference `ByteDev.Io`.

### FileSytem

Provides a small set of methods for working with files and directories.

Methods:

- IsFile
- IsDirectory
- FirstExists
- MoveFile
- CopyFile
- SwapFileNames

```csharp
// Initialize object
IFileSystem fs = new FileSystem();
```

```csharp
// Is it a file? Is it a directory?

bool isFile = fs.IsFile(@"C:\Temp\Something");
bool isDir = fs.IsDirectory(@"C:\Temp\Something");
```

```csharp
// Return first thing that exists

string[] paths =
{
    @"C:\Temp\Test1.txt",
    @"C:\Temp\Test2.txt",
    @"C:\Temp\TestDirectory",
};

var path = fs.FirstExists(paths);
```

```csharp
// Move a file

string sourceFile = @"C:\Temp\TestFile1.txt";
string destinationFile = @"C:\Windows\TestFile1.txt";

FileInfo info = fs.MoveFile(sourceFile, destinationFile, 
    FileOperationBehaviourType.DestExistsOverwrite);

// info.FullName = @"C:\Windows\TestFile1.txt" if successful
```

```csharp
// Copy a file

string sourceFile = @"C:\Temp\TestFile1.txt";
string destinationFile = @"C:\Windows\TestFile1.txt";

FileInfo info = fs.CopyFile(sourceFile, destinationFile, 
    FileOperationBehaviourType.DestExistsOverwrite);

// info.FullName = @"C:\Windows\TestFile1.txt" if successful
```

```csharp
// Swap two existing file's names

string file1 = @"C:\Temp\archive1.png";
string file2 = @"C:\Temp\cover.png";

fs.SwapFileNames(file1, file2);
```

### FileSize

Represents file size.

```csharp
long numberOfBytes = 1048576;

FileSize fileSize = new FileSize(numberOfBytes, FileSize.MultiplierType.DecimalMultiplier);

Console.Write(fileSize.ReadableSize);           // "1 MB"
Console.Write(fileSize.TotalBytes);             // 1048576
Console.Write(fileSize.TotalKiloBytes);         // 1048
Console.Write(fileSize.TotalMegaBytes);         // 1
```

### FileComparer

Provides functionality to compare two files.

Methods:

- IsSourceBigger
- IsSourceBiggerOrEqual
- IsSourceModifiedMoreRecently

### AssemblyEmbeddedResource

Provides functionality for easily retrieving embedded resources from assemblies.

Methods:

- CreateFromAssemblyContaining
- CreateFromAssembly
- Save

### IsolatedStorageIo

Provides functionality for isolated storage operations. To use reference namespace: `ByteDev.Io.IsolatedStorage`.

Methods:

- Exists
- Delete
- Write
- Read
- ReadAsXmlDoc
- ReadAsXDoc

```csharp
var io = new IsolatedStorageIo(IsolatedStorageFileType.UserStoreForApplication);

var fileName = new IsolatedStorageFileName("MyIsolatedFile", new Version(1, 0), ".txt");

io.Write(fileName, "Some data");

bool exists = io.Exists(fileName);

string data = io.Read(fileName);

io.Delete(fileName);
```

### StreamFactory

Provides simple functionality to create memory streams.

```csharp
MemoryStream stream = StreamFactory.Create("some text");
```

---

### AssemblyExtensions

Methods:

- GetManifestResourceName

### DirectoryInfoExtensions

Methods:

- CreateDirectory
- DeleteIfExists
- DeleteIfEmpty
- DeleteDirectories
- DeleteDirectoriesWithName
- DeleteEmptyDirectories
- DeleteFiles
- DeleteFilesExcept
- Empty
- EmptyIfExists
- GetFilesByExtensions
- GetAudioFiles
- GetImageFiles
- GetVideoFiles
- GetSize
- IsEmpty

### FileInfoExtensions

Methods:

- AddExtension
- DeleteIfExists
- GetExtension
- GetNextAvailableFileName
- HasExtension
- IsBinary
- RenameExtension
- RemoveExtension

### StreamExtensions

Methods:

- IsEmpty
- ReadAsBytes
- ReadAsMemoryStream
- ReadAsString
- WriteToFile
- WriteToFileAsync
