# P10A 分类筛选与文件夹视图精修设计

## 总体思路

P10A 在 P9B 的清理主页基础上做小步交互增强。分类卡片仍然不是删除入口，只作为筛选入口；真正删除仍然走列表/文件夹选中后的单文件删除或列表勾选后的批量删除。

## 分类筛选

Core 层在 `FileFilterOptions` 中新增 `CleanupCategory` 字段，并由 `FileFilter` 统一处理分类匹配：

- `HighRisk`: 系统级或程序安装级文件。
- `CacheTemporary`: 缓存/临时级文件。
- `UserData`: 用户数据级文件。
- `LargeFiles`: 大于等于 100MB 的文件。
- `UnknownCaution`: 未知/谨慎级文件。

这样 UI 不需要重复判断分类规则，也能通过 Core 测试保障后续行为。

## ViewModel 设计

`MainWindowViewModel` 新增 `_selectedCleanupCategory` 和 `SelectCleanupCategoryCommand`：

- 点击未选中的分类卡片：设置当前分类并重新筛选。
- 点击已选中的分类卡片：清空当前分类并重新筛选。
- 清除筛选时同时清空分类筛选。
- `ScanOverviewSummary` 追加当前分类筛选提示。

`CleanupCategoryCardViewModel` 新增：

- `IsActive`
- `ActionText`
- `BorderHex`

用于表现当前卡片是否处于筛选状态。

## UI 设计

分类卡片模板从静态 `Border` 改为 `Button` 包裹卡片内容，保留原有颜色和容量信息，并新增底部动作提示：

- 未选中：`点击筛选`
- 已选中：`已筛选`

文件夹视图的 `DisplayText` 去掉 `[DIR]` 和 `[FILE]` 前缀，保留名称、路径、数量、容量、风险和修改时间等信息。

## 测试

新增测试覆盖：

- Core 分类筛选：高风险、大文件。
- App 文件夹显示文本：文件夹和文件节点不再以技术前缀开头。

验证命令：

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```
