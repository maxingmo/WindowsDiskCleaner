# P4 Single File Delete Requirements

## Phase Name

P4: Single File Delete with Safety Confirmation

## Goal

Allow users to delete one selected file safely while using the P3 file level to prevent accidental system/program file deletion.

## In Scope

- Select one file from the result table.
- Delete the selected file.
- Default deletion target is the Windows Recycle Bin.
- Show a confirmation dialog before deletion.
- Show an additional high-risk confirmation for:
  - System Level
  - Program Install Level
- Remove the file from the visible list and full scan result after successful deletion.
- Show deletion success/failure in the status text.
- Keep deletion service testable without touching real user files.

## Out of Scope

- Batch delete.
- Permanent delete.
- Restore from recycle bin.
- Admin privilege elevation.
- Deleting folders.

## Success Criteria

- Delete button is disabled when no file is selected.
- Normal files require one confirmation.
- system/program-level files require second confirmation.
- confirmed deletion calls the recycle-bin adapter.
- cancelled deletion does not call the adapter.
- successful deletion updates the table.
- failed deletion shows a clear status message.

