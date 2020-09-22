# Release Notes

## 6.0.0 - ?? May 2020

Breaking changes:
- Remove StringExtensions.AppendBackSlash
- DirectoryInfoExtensions.GetSize now take optional `includeSubDirectories` param (default false)

New features:
- Added FileInfoExtensions.IsBinary

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
