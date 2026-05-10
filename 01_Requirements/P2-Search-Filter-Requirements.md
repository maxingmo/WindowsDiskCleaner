# P2 Search and Filter Requirements

## Phase Name

P2: Search and Filter Enhancement

## Goal

Help users quickly narrow scanned results to files that are likely worth reviewing or cleaning.

## In Scope

- Filter current scan results without rescanning.
- Keyword search against file name and full path.
- Extension filter, such as `.zip`, `.mp4`, or `log`.
- Minimum file size filter in MB.
- Modified-before date filter for old files.
- Quick filters:
  - All files
  - Large files
  - Video files
  - Archives
  - Installers
  - Logs and temporary files
- Show filtered file count and filtered total size.
- Preserve the full scanned result set separately from the filtered table.

## Out of Scope

- Deleting files.
- Risk marking.
- Duplicate detection.
- Full-text content search.
- Persistent search index.
- NTFS journal indexing.

## Success Criteria

- Filters can be combined.
- Clearing filters restores all scanned files.
- Extension matching works with or without a leading dot.
- Large file quick filter finds files at or above 100 MB.
- Type quick filters use predictable extension sets.
- The app rebuilds successfully and scanner tests still pass.

