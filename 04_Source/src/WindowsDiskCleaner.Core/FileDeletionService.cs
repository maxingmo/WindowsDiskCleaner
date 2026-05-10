using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileDeletionService
    {
        private readonly IFileDeleteAdapter _adapter;
        private readonly FileRiskClassifier _riskClassifier;

        public FileDeletionService(IFileDeleteAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            _adapter = adapter;
            _riskClassifier = new FileRiskClassifier();
        }

        public FileDeleteResult DeleteToRecycleBin(FileEntry file, FileDeleteConfirmation confirmation)
        {
            if (file == null)
            {
                return FileDeleteResult.Failure(string.Empty, "No file is selected.");
            }

            if (string.IsNullOrWhiteSpace(file.FullPath))
            {
                return FileDeleteResult.Failure(string.Empty, "File path is empty.");
            }

            var risk = _riskClassifier.Classify(file);
            var isHighRisk = risk.Level == FileRiskLevel.System || risk.Level == FileRiskLevel.ProgramInstall;

            if (isHighRisk && confirmation != FileDeleteConfirmation.HighRiskConfirmed)
            {
                return FileDeleteResult.Failure(file.FullPath, "High-risk file requires second confirmation.");
            }

            if (!isHighRisk && confirmation == FileDeleteConfirmation.NotConfirmed)
            {
                return FileDeleteResult.Failure(file.FullPath, "Delete confirmation is required.");
            }

            try
            {
                _adapter.MoveToRecycleBin(file.FullPath);
                return FileDeleteResult.Success(file.FullPath, "File moved to recycle bin.");
            }
            catch (Exception exception)
            {
                return FileDeleteResult.Failure(file.FullPath, "Delete failed: " + exception.Message);
            }
        }
    }
}

