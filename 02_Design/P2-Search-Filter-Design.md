# P2 Search and Filter Design

## Approach

P2 filters the in-memory scan result from P1. It does not trigger a new filesystem scan. This keeps the phase small and makes the UI responsive enough for the current MVP.

## Core Additions

### FileFilterOptions

Represents user-selected filters.

Fields:

- `Keyword`
- `Extension`
- `MinimumSizeBytes`
- `ModifiedBefore`
- `QuickFilter`

### QuickFileFilter

Enum for common filters:

- `All`
- `LargeFiles`
- `Videos`
- `Archives`
- `Installers`
- `LogsAndTemporary`

### FileFilter

Pure Core service that applies `FileFilterOptions` to a file list.

Rules:

- Keyword matches file name or full path, case-insensitive.
- Extension accepts `.zip` and `zip`.
- Minimum size uses bytes internally.
- Modified-before includes files whose modified time is earlier than the selected date.
- Quick filters are ANDed with the other filters.

## UI Additions

Add a compact filter bar above the file table:

- Keyword textbox.
- Extension textbox.
- Minimum MB textbox.
- Modified-before date picker.
- Quick filter combo box.
- Clear filters button.

The table binds to the filtered collection. The view model keeps:

- `_allFiles`: full scan results.
- `Files`: currently displayed filtered results.

## Verification

Core tests cover:

- Keyword matching.
- Extension normalization.
- Minimum size matching.
- Modified-before matching.
- Quick filter extension groups.
- Combined filters.

