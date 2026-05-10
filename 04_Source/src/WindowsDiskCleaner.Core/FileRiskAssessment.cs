namespace WindowsDiskCleaner.Core
{
    public sealed class FileRiskAssessment
    {
        public FileRiskAssessment(FileRiskLevel level, string displayName, string reason)
        {
            Level = level;
            DisplayName = displayName;
            Reason = reason;
        }

        public FileRiskLevel Level { get; private set; }

        public string DisplayName { get; private set; }

        public string Reason { get; private set; }
    }
}

