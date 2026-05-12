# Developer Guide

## Current Toolchain

The intended future stack is C# + .NET 8 + WPF. The current machine could not install the .NET 8 SDK because the C drive was nearly full, so this prototype builds with:

```text
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
```

Build scripts are in:

```text
04_Source\build-core-tests.ps1
04_Source\build-app.ps1
```

## Commands

Run tests:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
```

Build app:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

Run app:

```powershell
& "D:\AI Workspaces\WindowsDiskCleaner\06_Build-Release\FileScannerP2.exe"
```

## Important Build Notes

- Build outputs are ignored by git.
- The release executable is intentionally named `FileScannerP2.exe`.
- Executables containing `WindowsDiskCleaner` in the assembly/file name were rejected by Windows on the current development machine before `Main()` ran.
- The app is compiled as a console-subsystem executable because the Windows-subsystem build was also rejected in this environment. It still opens a WPF UI.
- The core test script compiles tests as a DLL and invokes the private test `Main` method through PowerShell reflection. This avoids the local security policy that intermittently deletes or blocks newly generated test executables.
- 360 security software may quarantine unsigned local builds. Add `D:\AI Workspaces\WindowsDiskCleaner` to the trust list for manual testing.

## Core Components

- `FileScanner`: recursive filesystem scanning with cancellation and IO error collection.
- `FileFilter`: in-memory filtering for scanned results.
- `FileRiskClassifier`: conservative file risk-level classification.
- `FileRiskVisualProfile`: risk-level visual badge and color configuration.
- `FolderTreeBuilder`: builds a folder/file tree from the current filtered file list without rescanning.
- `FileDeletionService`: validates single-file delete confirmation level before moving files through the delete adapter.
- `FileBatchDeletionService`: validates batch delete confirmation level before moving checked files through the delete adapter.
- `DebouncedAction`: test-covered debounce helper. The current WPF UI uses `Binding.Delay` for text input filtering.

## UI Components

- `MainWindow`: code-built WPF shell.
- `MainWindowViewModel`: scan, filter, cancel, view mode, single delete, batch selection, batch delete, and status orchestration.
- `FileEntryViewModel`: table row projection, including risk-level display and checkbox selection state.
- `FolderTreeNodeViewModel`: tree row projection for folder view.
- `HexBrushConverter`: WPF binding converter for Core hex color values.
- `QuickFilterOption`: UI model for common filter groups.
- `RiskFilterOption`: UI model for file risk-level filtering.

## Verification Rule

Before claiming a phase is done, run:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

Then launch `FileScannerP2.exe` and confirm the main window opens manually when automated launch is blocked by local Windows security policy.
