# P4 Single File Delete Design

## Approach

Deletion is split into two layers:

- Core policy/service decides whether deletion is allowed and calls an adapter.
- WPF UI handles user confirmation dialogs and visible list updates.

This keeps deletion behavior testable without deleting real files during tests.

## Core Additions

### FileDeleteConfirmation

Enum:

- `NotConfirmed`
- `Confirmed`
- `HighRiskConfirmed`

### FileDeleteResult

Contains:

- `Succeeded`
- `Message`
- `DeletedPath`

### IFileDeleteAdapter

Interface:

- `MoveToRecycleBin(string path)`

### FileDeletionService

Rules:

- missing or empty path fails.
- if file is system/program-level and confirmation is not `HighRiskConfirmed`, deletion is rejected.
- other files require at least `Confirmed`.
- successful delete uses the adapter to move the file to recycle bin.
- expected adapter exceptions become failed results.

## UI Additions

- Add a delete button.
- Bind selected table row to `SelectedFile`.
- On delete:
  - ask normal confirmation.
  - if high risk, ask second confirmation.
  - call Core deletion service.
  - remove deleted entry from `_allFiles` and refresh current filter.

## Windows Adapter

Use `Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile` with:

- `UIOption.OnlyErrorDialogs`
- `RecycleOption.SendToRecycleBin`
- `UICancelOption.ThrowException`

