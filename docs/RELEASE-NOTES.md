# Release Notes

## 7.4.0 - 05 January 2022

Breaking changes:
- (None)

New features:
- Added `FileSystem.Exists` method (returns collection of new type `ExistsInfo`).

Bug fixes / internal changes:
- (None)

## 7.3.0 - 11 October 2021

Breaking changes:
- (None)

New features:
- Added file locking functionality (`FileLocker` & `FileLockInfo` types).

Bug fixes / internal changes:
- (None)

## 7.2.1 - 24 May 2021

Breaking changes:
- (None)

New features:
- (None)

Bug fixes / internal changes:
- Updated `ByteDev.Strings` package dependency to 9.1.0.

## 7.2.0 - 16 March 2021

Breaking changes:
- (None)

New features:
- Added `FileSystem.GetPathExists`.

Bug fixes / internal changes:
- Argument null check improvements within `FileSystem`.

## 7.1.0 - 19 November 2020

Breaking changes:
- (None)

New features:
- Added `StreamExtensions.ReadAsBase64`.

Bug fixes / internal changes:
- (None)

## 7.0.0 - 30 September 2020

Breaking changes:
- Deleted `AssemblyExtensions` (use `ByteDev.Reflection` package instead).
- Deleted `AssemblyEmbeddedResource` (use `ByteDev.Reflection` package instead).

New features:
- Added `FileInfoExtensions.DeleteLine`.
- Added `FileInfoExtensions.DeleteLines`.
- Added `FileInfoExtensions.DeleteLastLine`.
- Added `FileInfoExtensions.ReplaceLine`.
- Added `FileInfoExtensions.ReplaceLines`.
- Added `StreamReaderExtensions.ReadLineKeepNewLineChars`.

Bug fixes / internal changes:
- (None)

## 6.0.0 - 24 September 2020

Breaking changes:
- Deleted `StringExtensions.AppendBackSlash`.
- Moved `FileSystem.DeleteDirectoriesWithName` to `DirectoryInfoExtensions.DeleteDirectoriesWithName`.

New features:
- Added `DirectoryInfoExtensions.EmptyIfExists`.
- Added `DirectoryInfoExtensions.DeleteIfExists`.
- Added `DirectoryInfoExtensions.DeleteIfEmpty`.
- Added `DirectoryInfoExtensions.DeleteFilesExcept`.
- Added `DirectoryInfoExtensions.DeleteEmptyDirectories`.
- Added `FileInfoExtensions.DeleteIfExists`.
- Added `FileInfoExtensions.IsBinary`.
- Added `FileInfoExtensions.GetExtension`.
- Added `FileInfoExtensions.RenameExtensionToLower`.
- Added `FileSystem.IsFile`.
- Added `StreamExtensions.IsEmpty`.
- Added `StreamExtensions.ReadAsBytes`.
- Added `StreamExtensions.WriteToFile`.
- Added `StreamExtensions.WriteToFileAsync`.
- Added `StreamFactory type`.
- `DirectoryInfoExtensions.GetSize` now takes optional `includeSubDirectories` param (default `false`).
- `FileInfoExtensions.IsBinary` now takes optional `requiredConsecutiveNul` param (default `1`).

Bug fixes / internal changes:
- Package related fixes

## 5.0.0 - 12 April 2020

Breaking changes:
- `IsolatedStorageIo` now takes `IsolatedStorageFileType` in the constructor.
- Renamed `IsolatedStorageIo.ReadAsXml` to `ReadAsXmlDoc`.

New features:
- Added `IIsolatedStorageIo`.
- Added `IsolatedStorageIo.ReadAsXDoc`.

Bug fixes / internal changes:
- Added .NET Standard package dependency.

## 4.0.0 - 29 Nov 2019

Breaking changes:
- Renamed `IsolatedStorageFileName.FileType` to `FileExtension`.
- Deleted `IAssemblyFileReader`.

New features:
- (None)

Bug fixes / internal changes:
- Add package icon.
- XML documentation now part of package.
