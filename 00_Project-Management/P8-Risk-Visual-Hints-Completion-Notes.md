# P8 风险等级视觉提示完成记录

## 已交付

- 新增统一的风险视觉配置 `FileRiskVisualProfile`。
- 新增风险视觉配置自动化测试。
- 列表视图的 `文件级别` 列改为带浅色背景和边框的风险提示。
- 文件夹视图的文件节点同步使用风险颜色提示。
- 新增 `HexBrushConverter` 支持 WPF 绑定十六进制颜色。
- 删除流程、批量删除流程和风险筛选流程保持不变。

## Core 变更

- `FileRiskVisualProfile`
- `RiskVisualProfileMarksHighRiskLevels`
- `RiskVisualProfileMarksCacheAsCleanupCandidate`

## App 变更

- `FileEntryViewModel` 暴露风险标记和颜色。
- `FolderTreeNodeViewModel` 暴露文件节点风险显示属性。
- `MainWindow.xaml.cs` 使用风险模板列和文件夹节点风险背景。
- `HexBrushConverter` 将十六进制颜色转换为 WPF Brush。

## 验证

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

最新观察结果：

- 33 个 Core 测试通过。
- App 编译成功并复制到 `06_Build-Release\FileScannerP2.exe`。
