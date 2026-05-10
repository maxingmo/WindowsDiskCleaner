# P1 Basic Scan MVP Design

## Recommended Stack

- Language: C#
- Runtime: .NET 8
- Desktop UI: WPF
- Test Framework: xUnit
- Pattern: MVVM-light style without introducing a large framework in P1

## Project Layout

Source code will live under:

```text
04_Source/
  WindowsDiskCleaner.sln
  src/
    WindowsDiskCleaner.App/
    WindowsDiskCleaner.Core/
  tests/
    WindowsDiskCleaner.Core.Tests/
```

P1 separates scanning logic from the UI:

- `WindowsDiskCleaner.Core` owns file scanning, file metadata models, formatting helpers, and error reporting.
- `WindowsDiskCleaner.App` owns WPF windows, view models, commands, and table binding.
- `WindowsDiskCleaner.Core.Tests` verifies scanner behavior without launching the UI.

## Core Components

### FileEntry

Represents one scanned file.

Fields:

- `Name`
- `FullPath`
- `SizeBytes`
- `CreatedAt`
- `ModifiedAt`
- `Extension`

### ScanOptions

Represents scan input.

Fields:

- `RootPath`

### ScanResult

Represents completed scan output.

Fields:

- `Files`
- `SkippedCount`
- `Errors`

### FileScanner

Recursively scans files with safe error handling.

Responsibilities:

- Walk directories.
- Read file metadata.
- Report files as they are found.
- Skip inaccessible paths.
- Respect cancellation.

## UI Behavior

The main window contains:

- Folder path textbox.
- Browse button.
- Scan button.
- Cancel button.
- Status text.
- File table.

The table supports sorting by clicking column headers. P1 can rely on WPF `DataGrid` sorting instead of building custom sorting logic.

## Error Handling

Expected IO problems are captured and displayed as counts:

- Unauthorized access.
- File not found during scan.
- Directory not found during scan.
- Path too long where the runtime cannot handle it.

Unexpected exceptions should be caught at the scan boundary and shown as a scan failure message.

## Verification

P1 verification should include:

- Unit tests for scanning normal files.
- Unit tests for file metadata mapping.
- Unit tests for skipped inaccessible/missing paths where practical.
- Build check for the whole solution.
- Manual run against a small local folder.

