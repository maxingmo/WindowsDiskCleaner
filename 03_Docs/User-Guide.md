# User Guide

## Run the Prototype

Build output is local and not committed to git.

Run:

```powershell
& "D:\AI Workspaces\WindowsDiskCleaner\06_Build-Release\FileScannerP2.exe"
```

If the file is missing, build it:

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

If 360 or another security tool removes the exe, add this project folder to its trust list:

```text
D:\AI Workspaces\WindowsDiskCleaner
```

## Basic Workflow

1. Click `选择目录`.
2. Pick a folder or disk path.
3. Click `开始扫描`.
4. Review the results in `列表视图` or `文件夹视图`.
5. Use filters to narrow results.
6. Select a file to read its risk details in the bottom details area.
7. Select a single file and click `删除文件`, or check multiple files and click `批量删除`.

## Main Window Layout

- Left navigation: shows the current cleanup-oriented information architecture. In this phase, only the disk scan page is active.
- Scan overview: shows scanned/visible capacity, checked capacity, file count, scan path, and primary actions.
- Filter area: keyword, extension, size, date, quick filter, and risk-level filter.
- Cleanup category area: summarizes the current visible results into high-risk, cache/temporary, user data, large files, and unknown/caution cards.
- Detailed results area: view switching, batch actions, list view, and folder view.
- Details area: selected-file risk/path information and current running status.

## Cleanup Category Cards

The cleanup category cards are summary cards only. They help you understand what kind of files are visible after the current scan and filters.

- `高风险`: system-level and program-install-level files. Review carefully before deleting.
- `缓存/临时`: cache, log, and temporary files.
- `用户数据`: downloads, pictures, documents, and other user data.
- `大文件`: files at or above 100 MB. This category is for space analysis and may overlap with other categories.
- `未知/谨慎`: files that do not match a clearer category.

Click a category card to filter the detailed results to that category. Click the same active card again to clear the category filter.

## Filters

Available filters:

- `关键词`: matches file name and full path.
- `扩展名`: accepts values such as `zip` or `.zip`.
- `最小 MB`: shows files at or above the entered size.
- `早于修改日`: shows older files.
- `快捷筛选`: common file groups such as large files, videos, archives, installers, logs, and temporary files.
- `文件级别`: shows only one risk level, or `全部级别`.

Text filters use a short delay to keep typing smooth.

## Result Views

- `列表视图`: sortable file table. Batch selection and batch delete are available here.
- `文件夹视图`: groups the current filtered results by folder. Folder rows show descendant file count and total size; file rows show size, risk level, and modified time.

Switching views does not rescan the disk. Filters affect both views.

## Risk Visual Hints

The `文件级别` column uses visual hints:

- `系统级`: red high-risk hint.
- `程序安装级`: orange high-risk hint.
- `用户数据级`: blue user-file hint.
- `缓存/临时级`: green cleanup-candidate hint.
- `未知/谨慎级`: neutral caution hint.

In `文件夹视图`, file nodes use the same risk color family. Folder nodes remain neutral.

## Batch Delete

In `列表视图`:

- Use the checkbox column to select individual files.
- Click `全选当前结果` to check all currently visible filtered files.
- Click `清空选择` to clear checked files.
- Click `批量删除` to move checked files to the Windows Recycle Bin.

Batch delete shows a summary with selected count, total size, and high-risk count. If any checked file is `系统级` or `程序安装级`, a second high-risk confirmation is required before anything is deleted.

## Delete Behavior

- Deletes use the Windows Recycle Bin by default.
- Normal single-file delete requires one confirmation.
- Batch delete requires one summary confirmation.
- `系统级` and `程序安装级` files require a second high-risk confirmation.
- Folder nodes in `文件夹视图` cannot be deleted in this phase. Select or check files from `列表视图` for batch deletion.

## Current Limitation

Folder-level deletion is not implemented yet.
Category-card click filtering is not implemented yet.
