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

## Usage

Main public classes:

- FileSytem: provides a small set of methods for working with files and directories. Including moving, copying, swapping, etc.
- FileSize: represents a binary file size.
- FileComparer: provides functionality to compare two files.
- AssemblyEbeddedResource: provides functionality for easily retrieving embedded resources from assemblies.
- IsolatedStorageIo: provides isolated storage operations.

Extension method classes:
- FileInfoExtensions
- DirectoryInfoExtensions
- StreamExtensions
- AssemblyExtensions
