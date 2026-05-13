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
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app-tests.ps1"
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
- `CleanupCategorySummaryBuilder`: builds cleanup-home category summaries from the current visible file list.
- `FileFilterOptions.CleanupCategory`: optional category filter used by cleanup-home cards.
- `FolderTreeBuilder`: builds a folder/file tree from the current filtered file list without rescanning.
- `FileDeletionService`: validates single-file delete confirmation level before moving files through the delete adapter.
- `FileBatchDeletionService`: validates batch delete confirmation level before moving checked files through the delete adapter.
- `DebouncedAction`: test-covered debounce helper. The current WPF UI uses `Binding.Delay` for text input filtering.

## UI Components

- `MainWindow`: code-built WPF shell, organized into left navigation, scan overview, filter, cleanup category, detailed results, and details sections.
- `MainWindowViewModel`: scan, filter, cleanup category summaries, cancel, view mode, single delete, batch selection, batch delete, and status orchestration.
- `CleanupCategoryCardViewModel`: app-facing projection for cleanup category cards.
- `FileEntryViewModel`: table row projection, including risk-level display and checkbox selection state.
- `FolderTreeNodeViewModel`: tree row projection for folder view, with user-facing display text.
- `HexBrushConverter`: WPF binding converter for Core hex color values.
- `QuickFilterOption`: UI model for common filter groups.
- `RiskFilterOption`: UI model for file risk-level filtering.

## Verification Rule

Before claiming a phase is done, run:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

Then launch `FileScannerP2.exe` and confirm the main window opens manually when automated launch is blocked by local Windows security policy.

## P9B Notes

- Cleanup category cards are derived from the current visible filtered results.
- The large-file category uses a 100 MB threshold and is allowed to overlap with risk-based categories.
- Category cards can filter detailed results in P10A; they still never invoke delete commands.
- Keep all Chinese UI strings in `.cs` files as Unicode escape sequences to avoid source encoding corruption.
