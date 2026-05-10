$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$outDir = Join-Path $root 'artifacts\tests'
$outFile = Join-Path $outDir 'CoreTests.exe'

New-Item -ItemType Directory -Path $outDir -Force | Out-Null

$sources = @(
    (Join-Path $root 'src\WindowsDiskCleaner.Core\DebouncedAction.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeleteConfirmation.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeleteResult.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileDeletionService.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileEntry.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileFilter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileFilterOptions.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskAssessment.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskClassifier.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskLevel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileScanner.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\IDebounceTimer.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\IFileDeleteAdapter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\QuickFileFilter.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanError.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanOptions.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanProgress.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\ScanResult.cs'),
    (Join-Path $root 'tests\WindowsDiskCleaner.Core.Tests\Program.cs')
)

& $csc /nologo /target:exe /out:$outFile $sources
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& $outFile
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
