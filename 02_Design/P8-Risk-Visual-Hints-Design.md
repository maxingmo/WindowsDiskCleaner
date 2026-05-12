# P8 风险等级视觉提示设计

## 总体思路

风险等级的颜色和标记文案放在 Core 层统一管理，App 层只负责展示。这样列表视图、文件夹视图和后续可能新增的面板都能使用同一套风险视觉语义。

## Core 设计

新增 `FileRiskVisualProfile`：

- `Level`
- `BadgeText`
- `ForegroundHex`
- `BackgroundHex`
- `BorderHex`
- `IsHighRisk`

`ForLevel(FileRiskLevel level)` 根据风险等级返回对应视觉配置。

## App 设计

`FileEntryViewModel` 保存 `FileRiskVisualProfile`，并暴露：

- `RiskBadgeText`
- `RiskForegroundHex`
- `RiskBackgroundHex`
- `RiskBorderHex`

`FolderTreeNodeViewModel` 对文件节点暴露风险颜色属性，让文件夹视图可以复用同一套提示。

WPF 侧新增 `HexBrushConverter`，把 Core 提供的十六进制颜色转换为 `Brush`。

## UI 设计

列表视图的 `文件级别` 列从纯文本列改为模板列，用浅色背景、边框和加粗文字呈现风险级别。

文件夹视图的文件节点使用对应风险等级的前景色和浅色背景。文件夹节点保持中性样式。
