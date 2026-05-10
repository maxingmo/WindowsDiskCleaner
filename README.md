# WindowsDiskCleaner

WindowsDiskCleaner is an early-stage Windows disk file scanner and cleanup-safety tool. The current build focuses on fast local file discovery, filtering, and deletion-risk hints before any delete feature is added.

The runnable prototype is currently published locally as:

```text
06_Build-Release\FileScannerP2.exe
```

> Note: On the current development machine, executables containing `WindowsDiskCleaner` in the assembly/file name were rejected by Windows before `Main()` ran. The prototype is therefore emitted as `FileScannerP2.exe`.

## Current Features

- Select a local folder to scan.
- Recursively list files under the selected folder.
- Show file name, file level, size, extension, created time, modified time, and full path.
- Sort table columns.
- Cancel an active scan.
- Skip inaccessible files/folders without crashing.
- Filter scanned results by:
  - keyword
  - extension
  - minimum size in MB
  - modified-before date
  - quick filter categories
- Delayed text filtering to reduce input lag.
- Classify files into risk levels:
  - System Level
  - Program Install Level
  - User Data Level
  - Cache/Temporary Level
  - Unknown/Caution Level

## Project Status

Completed:

- P1: Basic Scan MVP
- P2: Search and Filter Enhancement
- P3 partial: File Risk Level Column

Not yet implemented:

- file deletion
- recycle-bin integration
- batch selection
- second confirmation for high-risk files
- duplicate file detection
- charts and visual storage analysis
- installer packaging

## Repository Layout

```text
00_Project-Management   phase notes, handoff, progress records
01_Requirements         phase requirements
02_Design               technical design notes
03_Docs                 user and developer documentation
04_Source               source code and build scripts
05_Tests                reserved for test assets
06_Build-Release        local build output, ignored by git
07_Research-References  references and competitor notes
08_Assets               icons and visual assets
09_Logs                 local logs, ignored by git
10_Backups              local backups, ignored by git
```

## Build and Test

The original target stack is C# + .NET 8 + WPF. Because the development machine did not have enough C-drive space to install the .NET 8 SDK, the current prototype uses the existing .NET Framework compiler and WPF assemblies:

```text
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
```

Run tests:

```powershell
powershell -ExecutionPolicy Bypass -File "04_Source\build-core-tests.ps1"
```

Build app:

```powershell
powershell -ExecutionPolicy Bypass -File "04_Source\build-app.ps1"
```

Run app:

```powershell
& "06_Build-Release\FileScannerP2.exe"
```

## Latest Verified State

- 16 core tests passed.
- App startup verified with title: `File Scanner P3 - Risk Levels`.
- See `00_Project-Management\CURRENT-STATE-HANDOFF.md` for the latest handoff snapshot.

## Next Steps

Recommended next work:

1. Add selected-file risk detail display.
2. Add color/icon hints for risk levels.
3. Add risk-level filter.
4. Start P4: single-file delete with recycle-bin default and high-risk confirmation.

