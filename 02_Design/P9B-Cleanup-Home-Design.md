# P9B 清理主页风格布局设计

## 总体思路

P9B 将现有工具型窗口改造成“清理主页 + 详细结果”的结构。界面层仍然由 `MainWindow.xaml.cs` 以代码方式构建，业务状态仍由 `MainWindowViewModel` 管理。扫描、筛选、删除和风险确认逻辑不下沉到 UI，也不在本阶段加入真实的一键清理。

## 信息架构

主窗口分为两列：

- 左侧导航栏：绿色背景，包含磁盘扫描、重复文件、大文件分析、缓存/临时、隐私痕迹、设置等入口。P9B 仅高亮“磁盘扫描”，其它入口暂不绑定行为。
- 右侧主内容：从上到下依次是扫描概览、筛选条件、清理分类卡片、详细结果、底部详情/状态。

这种结构保留了当前工具的专业结果表，同时给用户一个更明确的“先看整体，再看分类，最后处理文件”的路径。

## 清理分类模型

Core 新增：

- `CleanupCategoryKind`
- `CleanupCategorySummary`
- `CleanupCategorySummaryBuilder`

分类顺序固定为：

1. 高风险
2. 缓存/临时
3. 用户数据
4. 大文件
5. 未知/谨慎

其中“高风险”合并系统级和程序安装级文件；“大文件”按 `100MB` 阈值统计，属于空间分析维度，允许与其它风险分类重叠。

## ViewModel 设计

App 新增 `CleanupCategoryCardViewModel`，负责把 Core 的分类汇总转换为卡片展示所需数据：

- 标题
- 描述
- 文件数量
- 容量显示
- 简单图标文字
- 强调色和背景色

`MainWindowViewModel` 新增：

- `CleanupCategories`
- `ReclaimableSummary`
- `SelectedSizeSummary`
- `ScanOverviewSummary`

每次扫描、筛选、删除或勾选变化后，相关概览属性和分类卡片会刷新。

## UI 设计

- 主窗口尺寸调整为 `1380 x 820`，最小尺寸 `1120 x 680`。
- 左侧导航使用绿色主色，当前页面使用浅绿色高亮。
- 顶部概览突出当前扫描总量和已勾选总量，并将开始扫描作为主要操作。
- 中部清理分类卡片使用不同强调色区分风险、缓存、用户数据、大文件和未知文件。
- 详细结果区继续保留列表视图、文件夹视图、全选当前结果、清空选择、批量删除和已选数量提示。
- 底部继续显示选中文件风险、路径和运行状态。

## 风险控制

- 删除仍然只走 `FileDeletionService` 和 `FileBatchDeletionService`。
- 高风险文件仍需二次确认。
- 分类卡片只做统计和提醒，不直接触发删除。
- 左侧导航的未实现模块不绑定命令，避免用户误以为功能已完成。

## 验证

验证命令：

```powershell
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-core-tests.ps1"
powershell -ExecutionPolicy Bypass -File "D:\AI Workspaces\WindowsDiskCleaner\04_Source\build-app.ps1"
```
