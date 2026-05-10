# P3 File Risk Level Requirements

## Phase Name

P3: File Risk Level and Safety Hint

## Goal

Add a visible file level column so users can understand deletion risk before any delete feature is introduced.

## In Scope

- Classify each scanned file into a deletion risk level.
- Show the level in the result table.
- Use path and extension rules only in this first version.
- Keep the feature read-only; no deletion is added in this phase.

## Levels

- System Level: Windows system paths and protected system-looking files.
- Program Install Level: application installation directories and installer packages.
- User Data Level: user documents, downloads, desktop, pictures, videos, music, and general personal files.
- Cache/Temporary Level: temp, cache, log, backup, and temporary extension patterns.
- Unknown/Caution Level: files that do not match a confident rule.

## Success Criteria

- `C:\Windows\...` is classified as System Level.
- `C:\Program Files\...` and `C:\Program Files (x86)\...` are classified as Program Install Level.
- user profile document/download paths are classified as User Data Level unless a stronger cache/temp rule applies.
- temp/cache/log files are classified as Cache/Temporary Level.
- unmatched paths are classified as Unknown/Caution Level.

