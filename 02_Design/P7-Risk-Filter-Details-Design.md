# P7 风险筛选与风险详情设计

## 总体思路

P7 继续沿用当前“扫描结果保存在内存中，筛选结果重新投影到列表和文件夹树”的结构。风险级别仍由 `FileRiskClassifier` 统一判断，避免 UI 层重复写路径判断规则。

## Core 设计

`FileFilterOptions` 增加：

- `FileRiskLevel? RiskLevel`

`FileFilter.Apply` 在已有筛选条件之后检查 `RiskLevel`。当该值为空时不启用风险过滤；当该值存在时，只保留分类结果等于该级别的文件。

新增测试：

- `FilterAppliesRiskLevel`
- `FilterDoesNotApplyRiskLevelWhenUnset`

## App 设计

新增 `RiskFilterOption` 作为下拉框选项模型：

- `Label`
- `FileRiskLevel? Value`

`MainWindowViewModel` 增加：

- `RiskFilters`
- `SelectedRiskFilter`
- `SelectedFileRiskDetail`
- `SelectedFilePathDetail`

`BuildFilterOptions()` 将 `SelectedRiskFilter.Value` 传入 Core 筛选选项。`ClearFilters()` 将风险筛选恢复到第一项“全部级别”。

## UI 设计

筛选栏在“快捷筛选”之后增加“文件级别”下拉框。底部状态区增加两行选中文件详情：

- 第一行显示风险级别、风险标记和原因。
- 第二行显示完整路径。

该设计保持界面紧凑，不新增删除入口，不改变 P6 的批量删除流程。
