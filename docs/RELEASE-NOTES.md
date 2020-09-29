# Release Notes

## 7.0.0 - ??

Breaking changes:
- Deleted `AssemblyExtensions` (use `ByteDev.Reflection` package instead)
- Deleted `AssemblyEmbeddedResource` (use `ByteDev.Reflection` package instead)

New features:
- Added `FileInfoExtensions.DeleteLines`
- Added `StreamReaderExtensions.ReadLineKeepNewLineChars`

Bug fixes / internal changes:
- (None)

## 6.0.0 - 24 September 2020

Breaking changes:
- Removed `StringExtensions.AppendBackSlash`
- `FileSystem.DeleteDirectoriesWithName` is now a `DirectoryInfoExtension`

New features:
- Added `DirectoryInfoExtensions.EmptyIfExists`
- Added `DirectoryInfoExtensions.DeleteIfExists`
- Added `DirectoryInfoExtensions.DeleteIfEmpty`
- Added `DirectoryInfoExtensions.DeleteFilesExcept`
- Added `DirectoryInfoExtensions.DeleteEmptyDirectories`
- Added `FileInfoExtensions.DeleteIfExists`
- Added `FileInfoExtensions.IsBinary`
- Added `FileInfoExtensions.GetExtension`
- Added `FileInfoExtensions.RenameExtensionToLower`
- Added `FileSystem.IsFile`
- Added `StreamExtensions.IsEmpty`
- Added `StreamExtensions.ReadAsBytes`
- Added `StreamExtensions.WriteToFile`
- Added `StreamExtensions.WriteToFileAsync`
- Added `StreamFactory type`
- `DirectoryInfoExtensions.GetSize` now takes optional `includeSubDirectories` param (default false)
- `FileInfoExtensions.IsBinary` now takes optional `requiredConsecutiveNul` param (default 1)

Bug fixes / internal changes:
- Package related fixes

## 5.0.0 - 12 April 2020

Breaking changes:
- `IsolatedStorageIo` now takes `IsolatedStorageFileType` in the constructor
- Renamed `IsolatedStorageIo.ReadAsXml` to `ReadAsXmlDoc`

New features:
- Added `IIsolatedStorageIo`
- Added `IsolatedStorageIo.ReadAsXDoc`

Bug fixes / internal changes:
- Added .NET Standard package dependency

## 4.0.0 - 29 Nov 2019

Breaking changes:
- Renamed `IsolatedStorageFileName.FileType` to `FileExtension`
- Deleted `IAssemblyFileReader`

New features:
- (None)

Bug fixes / internal changes:
- Add package icon
- XML documentation now part of package
