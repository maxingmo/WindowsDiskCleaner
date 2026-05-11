# P6 Batch Selection and Batch Delete Design

## Approach

Batch delete reuses the existing conservative risk classification and recycle-bin adapter. The UI owns checkbox state; the Core layer owns delete confirmation validation and per-file delete execution.

## Core Additions

### FileBatchDeleteResult

Fields:

- `Succeeded`
- `SelectedCount`
- `DeletedCount`
- `FailedCount`
- `Message`
- `DeletedPaths`

`DeletedPaths` lets the UI remove only files that were actually moved to the Recycle Bin.

### FileBatchDeletionService

Rules:

- Null or empty selection fails.
- `NotConfirmed` fails before deleting anything.
- If any selected file is high risk, anything short of `HighRiskConfirmed` fails before deleting anything.
- After confirmation passes, files are moved one by one through `IFileDeleteAdapter`.
- Partial failures are reported without hiding failed files from the UI.

## UI Additions

- `FileEntryViewModel.IsSelected`.
- Checkbox column in list view.
- `全选当前结果` command.
- `清空选择` command.
- `批量删除` command.
- `CheckedFileSummary` display.

## Safety Behavior

Batch delete shows:

- checked file count
- total checked size
- high-risk checked file count

If high-risk files are present, a second warning dialog is required before deleting any file.
