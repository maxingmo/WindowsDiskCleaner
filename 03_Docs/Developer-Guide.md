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

## Core Components

- `FileScanner`: recursive filesystem scanning with cancellation and IO error collection.
- `FileFilter`: in-memory filtering for scanned results.
- `FileRiskClassifier`: conservative file risk-level classification.
- `DebouncedAction`: test-covered debounce helper. The current WPF UI uses `Binding.Delay` for text input filtering.

## UI Components

- `MainWindow`: code-built WPF shell.
- `MainWindowViewModel`: scan, filter, cancel, and status orchestration.
- `FileEntryViewModel`: table row projection, including risk-level display.
- `QuickFilterOption`: UI model for common filter groups.

## Verification Rule

Before claiming a phase is done, run:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

Then launch `FileScannerP2.exe` and confirm the main window opens.

