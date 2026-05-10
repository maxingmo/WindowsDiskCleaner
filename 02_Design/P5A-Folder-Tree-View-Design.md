# P5A Folder Tree View Design

## Approach

Build a folder tree from the same filtered in-memory file list used by the flat table. This avoids rescanning the filesystem and keeps behavior consistent with existing filters.

## Core Additions

### FolderTreeNodeType

Enum:

- `Folder`
- `File`

### FolderTreeNode

Fields:

- `Name`
- `FullPath`
- `NodeType`
- `SizeBytes`
- `FileCount`
- `File`
- `Children`

For folder nodes:

- `SizeBytes` is the sum of descendant files.
- `FileCount` is the count of descendant files.

For file nodes:

- `SizeBytes` is file size.
- `FileCount` is 1.
- `File` points to the `FileEntry`.

### FolderTreeBuilder

Builds a list of root folder nodes from a file list.

Rules:

- Parent folders are created only when they contain filtered files.
- File nodes are placed below their containing folder.
- Folder totals roll up nested descendants.
- Children are sorted with folders first, then files by name.

## UI Additions

- Add a view mode selector above the results.
- Keep `DataGrid` for List View.
- Add `TreeView` for Folder View.
- Use a simple text row per node:
  - folders: `FolderName (N files, size)`
  - files: `FileName - size - risk level - modified time`
- Bind file-node selection to the existing `SelectedFile` so delete still works for a selected file.

