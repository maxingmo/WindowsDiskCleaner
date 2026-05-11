# P6 Batch Selection and Batch Delete Requirements

## Phase Name

P6: Batch Selection and Batch Delete

## Goal

Allow users to check multiple files in list view and move the checked files to the Windows Recycle Bin with the same safety rules used by single-file deletion.

## In Scope

- Add checkbox selection in `列表视图`.
- Add `全选当前结果`.
- Add `清空选择`.
- Add checked-file count summary.
- Add `批量删除`.
- Move checked files to the Windows Recycle Bin.
- Show a batch summary confirmation before deleting.
- Require a second high-risk confirmation if the checked files include `系统级` or `程序安装级`.
- Remove successfully deleted files from the in-memory scan results.
- Keep failed files visible if only part of a batch delete succeeds.

## Out of Scope

- Folder-level delete from `文件夹视图`.
- Selecting folder nodes to delete all descendants.
- Permanent delete bypassing the Recycle Bin.
- Undo inside the app.

## Success Criteria

- Batch delete cannot run with no checked files.
- Normal checked files require one confirmation.
- Mixed normal and high-risk checked files require second confirmation before anything is deleted.
- Successful delete updates list view and folder view through existing filtered results.
- Core batch delete behavior is covered by tests.
