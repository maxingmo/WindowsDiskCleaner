# P3 Risk Level Completion Notes

## Phase

P3: File Risk Level and Safety Hint

## Delivered

- Added file risk level classification in Core.
- Added table column: file level.
- Added conservative categories:
  - System Level
  - Program Install Level
  - User Data Level
  - Cache/Temporary Level
  - Unknown/Caution Level
- Restored main UI Chinese labels using Unicode escape strings to avoid source encoding corruption.
- Preserved P2 delayed text filtering.

## Current Classification Rules

- `C:\Windows\...`, selected Windows system paths, and recycle/system-volume paths are System Level.
- `C:\Program Files\...` and `C:\Program Files (x86)\...` are Program Install Level.
- paths containing temp, tmp, cache, log/logs, or extensions such as `.tmp`, `.log`, `.bak`, `.dmp` are Cache/Temporary Level.
- user known folders such as Desktop, Documents, Downloads, Pictures, Videos, and Music are User Data Level.
- unmatched paths are Unknown/Caution Level.

## Verification Result

Core tests passed:

```text
PASS ScanIncludesRootAndNestedFiles
PASS ScanMapsFileMetadata
PASS MissingRootIsReportedAsSkippedError
PASS CancellationStopsScan
PASS FilterMatchesKeywordInNameOrPath
PASS FilterNormalizesExtension
PASS FilterAppliesMinimumSize
PASS FilterAppliesModifiedBefore
PASS FilterAppliesQuickTypeAndCombinedRules
PASS DebouncedActionDoesNotRunBeforeTimerElapsed
PASS DebouncedActionRunNowStopsTimerAndRunsImmediately
PASS RiskClassifierMarksWindowsPathAsSystem
PASS RiskClassifierMarksProgramFilesAsProgramInstall
PASS RiskClassifierMarksUserKnownFoldersAsUserData
PASS RiskClassifierMarksCacheAndLogsAsTemporary
PASS RiskClassifierMarksUnknownPathsAsCaution
All tests passed.
```

Startup verification:

```text
FileScannerP2.exe opened with title: File Scanner P3 - Risk Levels
```

Run:

```powershell
& "D:\AI Workspaces\WindowsDiskCleaner\06_Build-Release\FileScannerP2.exe"
```

