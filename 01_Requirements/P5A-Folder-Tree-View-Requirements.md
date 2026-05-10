# P5A Folder Tree View Requirements

## Phase Name

P5A: Folder Tree View Mode

## Goal

Allow users to view the current filtered file results grouped by folder, similar to a file comparison or file manager tree.

## In Scope

- Keep the existing flat file list view.
- Add a view mode switch:
  - List View
  - Folder View
- Folder View groups the current filtered files by directory.
- Folder nodes show:
  - folder name/path
  - total file count under that folder
  - total size under that folder
- File nodes show:
  - file name
  - size
  - file risk level
  - modified time
- Filters continue to affect both views.
- Empty folders are not shown.
- Selecting a file node should allow the existing single-file delete command.

## Out of Scope

- Deleting a folder node.
- Batch deleting all files under a folder.
- Folder-level risk classification.
- Persisting expanded/collapsed tree state.

## Success Criteria

- Switching views does not rescan the disk.
- Folder View shows only files from the current filtered result set.
- Folder totals include nested file descendants.
- Selecting a file in Folder View enables single-file delete.
- List View behavior remains unchanged.

