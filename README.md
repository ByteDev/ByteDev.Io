[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Io?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Io/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Io.svg)](https://www.nuget.org/packages/ByteDev.Io)

# ByteDev.Io

Set of IO related .NET utility classes.

## Installation

ByteDev.Io has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Io is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Io`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Io/).

## Code

The repo can be cloned from git bash:

`git clone https://github.com/ByteDev/ByteDev.Io`

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Io/blob/master/docs/RELEASE-NOTES.md).

## Usage

### Main public classes

To use these main public classes simply reference `ByteDev.Io`.

**FileSytem**: Provides a small set of methods for working with files and directories.

Methods:
- IsFile
- IsDirectory
- FirstExists
- MoveFile
- CopyFile
- SwapFileNames

**FileSize:** Represents a binary file size.

**FileComparer**: Provides functionality to compare two files.

Methods:
- IsSourceBigger
- IsSourceBiggerOrEqual
- IsSourceModifiedMoreRecently

**AssemblyEmbeddedResource:** Provides functionality for easily retrieving embedded resources from assemblies.

Methods:
- CreateFromAssemblyContaining
- CreateFromAssembly
- Save

**IsolatedStorageIo:** Provides isolated storage operations.

Methods:
- Exists
- Delete
- Write
- Read
- ReadAsXmlDoc
- ReadAsXDoc

**StreamFactory**: Provides simple functionality to create memory streams.

---

### Extension method classes

To use the extension methods simply reference `ByteDev.Io`.

AssemblyExtensions
- GetManifestResourceName

DirectoryInfoExtensions
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

FileInfoExtensions
- DeleteIfExists
- AddExtension
- GetNextAvailableFileName
- RenameExtension
- RemoveExtension
- HasExtension
- IsBinary

StreamExtensions
- IsEmpty
- ReadAsBytes
- ReadAsString
- ReadAsMemoryStream
- WriteToFile
- WriteToFileAsync
