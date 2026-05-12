# P7 风险筛选与风险详情完成记录

## 已交付

- 新增“文件级别”下拉筛选。
- 风险级别筛选可与其它筛选条件组合。
- “清除筛选”会恢复到“全部级别”。
- 底部状态区新增选中文件风险说明。
- 底部状态区新增选中文件完整路径。
- 修复快捷筛选下拉项中文乱码。
- 继续使用 `FileScannerP2.exe` 作为本机可运行文件名。

## Core 变更

- `FileFilterOptions.RiskLevel`
- `FileFilter` 按 `RiskLevel` 过滤。
- `FileRiskClassifier` 显示文本改为 Unicode escape，降低 C# 源码中文编码损坏风险。

## App 变更

- 新增 `RiskFilterOption`。
- `MainWindowViewModel` 增加风险筛选集合、选中项和选中文件详情属性。
- `MainWindow.xaml.cs` 增加风险级别下拉框和底部详情显示。
- `build-app.ps1` 纳入新增 App 源文件。

## 验证

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```

最新观察结果：

- 31 个 Core 测试通过。
- App 编译成功并复制到 `06_Build-Release\FileScannerP2.exe`。
