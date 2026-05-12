$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$wpf = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\WPF'
$outDir = Join-Path $root 'artifacts\app'
$outFile = Join-Path $outDir 'FileScannerP2.exe'
$projectRoot = Split-Path -Parent $root
$releaseDir = Join-Path $projectRoot '06_Build-Release'
$releaseFile = Join-Path $releaseDir 'FileScannerP2.exe'
$presentationCore = Join-Path $wpf 'PresentationCore.dll'
$presentationFramework = Join-Path $wpf 'PresentationFramework.dll'
$windowsBase = Join-Path $wpf 'WindowsBase.dll'

New-Item -ItemType Directory -Path $outDir -Force | Out-Null

$references = @(
    '/reference:System.dll',
    '/reference:System.Core.dll',
    '/reference:System.Xaml.dll',
    '/reference:System.Windows.Forms.dll',
    '/reference:Microsoft.VisualBasic.dll',
    "/reference:$presentationCore",
    "/reference:$presentationFramework",
    "/reference:$windowsBase"
)

$sources = @(
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileBatchDeleteResult.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileBatchDeletionService.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeleteConfirmation.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeleteResult.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeletionService.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileEntry.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileFilter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileFilterOptions.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskAssessment.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskClassifier.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskLevel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskVisualProfile.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileScanner.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeBuilder.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeNode.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeNodeType.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\IFileDeleteAdapter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\QuickFileFilter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanError.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanOptions.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanProgress.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanResult.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\App.xaml.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\FileEntryViewModel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\FolderTreeNodeViewModel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\HexBrushConverter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\MainWindow.xaml.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\MainWindowViewModel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\Program.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\QuickFilterOption.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\RecycleBinDeleteAdapter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\RelayCommand.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\RiskFilterOption.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\StartupDiagnostics.cs')
)

# This environment rejects the generated Windows-subsystem executable before Main()
# runs. Building as a console-subsystem executable keeps the same WPF UI usable.
& $csc /nologo /target:exe /out:$outFile $references $sources
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

Write-Host "Built $outFile"

New-Item -ItemType Directory -Path $releaseDir -Force | Out-Null
Copy-Item -LiteralPath $outFile -Destination $releaseFile -Force
Write-Host "Copied release exe to $releaseFile"
