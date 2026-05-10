# P3 File Risk Level Design

## Approach

Add a pure Core classifier so the UI can display file level consistently and future delete confirmation logic can reuse the same classification.

## Core Additions

### FileRiskLevel

Enum values:

- `System`
- `ProgramInstall`
- `UserData`
- `CacheTemporary`
- `UnknownCaution`

### FileRiskAssessment

Contains:

- `Level`
- `DisplayName`
- `Reason`

### FileRiskClassifier

Classifies a `FileEntry` using conservative rules:

- System paths win first.
- Program install paths are second.
- Cache/temp/log paths and extensions are marked as cache/temporary.
- User profile known folders become user data.
- Everything else becomes unknown/caution.

## UI Additions

- Add a `文件级别` column near the left side of the table.
- Add a `RiskLevelDisplay` property to `FileEntryViewModel`.
- Preserve existing scan/search/filter behavior.

