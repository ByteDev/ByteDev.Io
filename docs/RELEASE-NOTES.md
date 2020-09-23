# Release Notes

## 6.0.0 - ?? September 2020

Breaking changes:
- Removed StringExtensions.AppendBackSlash
- DirectoryInfoExtensions.GetSize now takes optional `includeSubDirectories` param (default false)
- FileSystem.DeleteDirectoriesWithName is now a DirectoryInfoExtension
- FileInfo.RenameExtension is now FileInfo.AddOrRenameExtension

New features:
- Added FileInfoExtensions.IsBinary
- Added FileInfoExtensions.GetExtension
- Added FileSystem.IsFile
- Added StreamExtensions.IsEmpty
- Added StreamExtensions.ReadAsBytes
- Added StreamExtensions.WriteToFile
- Added StreamExtensions.WriteToFileAsync
- Added StreamFactory type

Bug fixes / internal changes:
- Package related fixes

## 5.0.0 - 12 April 2020

Breaking changes:
- IsolatedStorageIo now takes IsolatedStorageFileType in the constructor.
- IsolatedStorageIo.ReadAsXml is now ReadAsXmlDoc.

New features:
- Added IIsolatedStorageIo.
- Added IsolatedStorageIo.ReadAsXDoc.

Bug fixes / internal changes:
- Added .NET Standard package dependency.

## 4.0.0 - 29 Nov 2019

Breaking changes:
- IsolatedStorageFileName.FileType is now FileExtension.
- Deleted IAssemblyFileReader.

New features:
- (None)

Bug fixes / internal changes:
- Add package icon.
- XML documentation now part of package.
