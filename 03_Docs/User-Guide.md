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

## Basic Workflow

1. Click `选择目录`.
2. Pick a folder or disk path.
3. Click `开始扫描`.
4. Review the results in `列表视图` or `文件夹视图`.
5. Use filters to narrow results.
6. Select a file and click `删除文件` if you want to move it to the Recycle Bin.

## Filters

Available filters:

- `关键词`: matches file name and full path.
- `扩展名`: accepts values such as `zip` or `.zip`.
- `最小 MB`: shows files at or above the entered size.
- `早于修改日`: shows older files.
- `快捷筛选`: common file groups such as large files, videos, archives, installers, logs, and temporary files.

Text filters use a short delay to keep typing smooth.

## Result Views

- `列表视图`: the original sortable file table.
- `文件夹视图`: groups the current filtered results by folder. Folder rows show descendant file count and total size; file rows show size, risk level, and modified time.

Switching views does not rescan the disk. Filters affect both views.

## File Level Column

The `文件级别` column is a safety hint, not a delete decision.

Levels:

- `系统级`: Windows/system paths. Treat as high risk.
- `程序安装级`: installed application files or installer-like locations. Treat as high risk.
- `用户数据级`: documents, downloads, pictures, videos, desktop, and similar user files.
- `缓存/临时级`: cache, temp, log, backup, and temporary-looking files.
- `未知/谨慎级`: unmatched files. Review carefully.

## Delete Behavior

- Deletes use the Windows Recycle Bin by default.
- Normal files require one confirmation.
- `系统级` and `程序安装级` files require a second high-risk confirmation.
- Folder nodes in `文件夹视图` cannot be deleted in this phase. Select a file node before deleting.

## Current Limitation

Batch deletion and folder-level deletion are not implemented yet.
