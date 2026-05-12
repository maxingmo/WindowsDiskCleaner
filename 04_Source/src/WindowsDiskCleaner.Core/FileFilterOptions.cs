using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileFilterOptions
    {
        public FileFilterOptions()
        {
            QuickFilter = QuickFileFilter.All;
        }

        public string Keyword { get; set; }

        public string Extension { get; set; }

        public long? MinimumSizeBytes { get; set; }

        public DateTime? ModifiedBefore { get; set; }

        public QuickFileFilter QuickFilter { get; set; }

        public FileRiskLevel? RiskLevel { get; set; }
    }
}
