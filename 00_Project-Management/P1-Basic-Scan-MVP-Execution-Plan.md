# P1 Basic Scan MVP Execution Plan

## Phase

P1: Basic Scan MVP

## Current Environment Finding

The project directory exists, but the `dotnet` CLI is not currently available in PowerShell. Installing the .NET 8 SDK is required before creating, building, and testing the C# WPF solution.

Recommended install command:

```powershell
winget install Microsoft.DotNet.SDK.8 --accept-package-agreements --accept-source-agreements
```

## Implementation Tasks

### Task 1: Create Solution Structure

- Create `04_Source/WindowsDiskCleaner.sln`.
- Create `04_Source/src/WindowsDiskCleaner.Core`.
- Create `04_Source/src/WindowsDiskCleaner.App`.
- Create `04_Source/tests/WindowsDiskCleaner.Core.Tests`.
- Add all projects to the solution.
- Add a test reference from `WindowsDiskCleaner.Core.Tests` to `WindowsDiskCleaner.Core`.

### Task 2: Add Core Scanner Tests First

Test behaviors:

- Scanner returns files from a selected root folder.
- Scanner includes name, full path, size, created time, modified time, and extension.
- Scanner scans nested directories.
- Scanner supports cancellation.
- Scanner records skipped paths/errors instead of crashing.

### Task 3: Implement Core Scanner

Create:

- `FileEntry`
- `ScanOptions`
- `ScanError`
- `ScanResult`
- `FileScanner`

The scanner should use safe recursive enumeration and catch expected IO exceptions around directory and file access.

### Task 4: Add WPF Shell

Create a main WPF window with:

- Folder path field.
- Browse button.
- Scan button.
- Cancel button.
- Status text.
- File result `DataGrid`.

### Task 5: Wire UI to Scanner

Create:

- `MainWindowViewModel`
- Async scan command.
- Cancel command.
- Observable file collection.
- Scan status fields.

### Task 6: Verify P1

Run:

```powershell
dotnet test .\WindowsDiskCleaner.sln
dotnet build .\WindowsDiskCleaner.sln
```

Manual check:

- Launch the app.
- Select a small folder.
- Run scan.
- Confirm file table populates.
- Sort by size and modified time.
- Confirm status text shows file count and skipped count.

