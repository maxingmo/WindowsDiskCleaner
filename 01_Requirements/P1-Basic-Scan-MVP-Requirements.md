# P1 Basic Scan MVP Requirements

## Phase Name

P1: Basic Scan MVP

## Goal

Build the first usable Windows desktop version that can scan a selected folder, list files, and sort them by size or time without freezing the interface.

## In Scope

- Select one local folder as the scan root.
- Recursively scan files under the selected folder.
- List files only; folder aggregation is not included in P1.
- Show these columns:
  - File name
  - Full path
  - File size
  - Created time
  - Modified time
  - Extension
- Sort by:
  - File size
  - Created time
  - Modified time
- Show scan progress status.
- Handle inaccessible folders and files without crashing.
- Keep a count of skipped entries caused by permission or IO errors.

## Out of Scope

- File deletion.
- Risk marking for system files.
- Batch selection.
- Duplicate file detection.
- Space visualization charts.
- Persistent scan database.
- NTFS USN Journal indexing.

## Success Criteria

- The app can scan a normal user folder.
- The app can display discovered files in a table.
- Sorting by size and time works.
- Permission errors are captured and surfaced as skipped counts.
- The UI remains responsive during scanning.

