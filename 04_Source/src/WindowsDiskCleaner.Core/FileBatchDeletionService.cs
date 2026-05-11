using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileBatchDeletionService
    {
        private readonly IFileDeleteAdapter _adapter;
        private readonly FileRiskClassifier _riskClassifier;

        public FileBatchDeletionService(IFileDeleteAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            _adapter = adapter;
            _riskClassifier = new FileRiskClassifier();
        }

        public FileBatchDeleteResult DeleteToRecycleBin(IEnumerable<FileEntry> files, FileDeleteConfirmation confirmation)
        {
            if (files == null)
            {
                return FileBatchDeleteResult.Failure(0, 0, 0, "No files are selected.", new string[0]);
            }

            var selectedFiles = files
                .Where(file => file != null && !string.IsNullOrWhiteSpace(file.FullPath))
                .ToList();

            if (selectedFiles.Count == 0)
            {
                return FileBatchDeleteResult.Failure(0, 0, 0, "No files are selected.", new string[0]);
            }

            if (confirmation == FileDeleteConfirmation.NotConfirmed)
            {
                return FileBatchDeleteResult.Failure(selectedFiles.Count, 0, 0, "Batch delete confirmation is required.", new string[0]);
            }

            var highRiskCount = selectedFiles.Count(IsHighRisk);
            if (highRiskCount > 0 && confirmation != FileDeleteConfirmation.HighRiskConfirmed)
            {
                return FileBatchDeleteResult.Failure(selectedFiles.Count, 0, 0, "High-risk files require second confirmation.", new string[0]);
            }

            var deletedCount = 0;
            var failedCount = 0;
            var deletedPaths = new List<string>();

            foreach (var file in selectedFiles)
            {
                try
                {
                    _adapter.MoveToRecycleBin(file.FullPath);
                    deletedCount++;
                    deletedPaths.Add(file.FullPath);
                }
                catch
                {
                    failedCount++;
                }
            }

            if (failedCount > 0)
            {
                return FileBatchDeleteResult.Failure(
                    selectedFiles.Count,
                    deletedCount,
                    failedCount,
                    "Some files could not be moved to recycle bin.",
                    deletedPaths);
            }

            return FileBatchDeleteResult.Success(
                selectedFiles.Count,
                deletedCount,
                "Selected files moved to recycle bin.",
                deletedPaths);
        }

        private bool IsHighRisk(FileEntry file)
        {
            var risk = _riskClassifier.Classify(file);
            return risk.Level == FileRiskLevel.System || risk.Level == FileRiskLevel.ProgramInstall;
        }
    }
}
