$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$csc = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$outDir = Join-Path $root 'artifacts\check'
$outFile = Join-Path $outDir 'WindowsDiskCleanerAppTests.dll'

New-Item -ItemType Directory -Path $outDir -Force | Out-Null

$sources = @(
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileEntry.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskAssessment.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskClassifier.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskLevel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FileRiskVisualProfile.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeBuilder.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeNode.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.Core\FolderTreeNodeType.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\FileEntryViewModel.cs'),
    (Join-Path $root 'src\WindowsDiskCleaner.App\FolderTreeNodeViewModel.cs'),
    (Join-Path $root 'tests\WindowsDiskCleaner.App.Tests\Program.cs')
)

& $csc /nologo /target:library /out:$outFile $sources
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

$assembly = [System.Reflection.Assembly]::LoadFile($outFile)
$programType = $assembly.GetType('WindowsDiskCleaner.App.Tests.Program', $true)
$main = $programType.GetMethod('Main', [System.Reflection.BindingFlags]'NonPublic, Static')
$exitCode = [int]$main.Invoke($null, @())
if ($exitCode -ne 0) {
    exit $exitCode
}
