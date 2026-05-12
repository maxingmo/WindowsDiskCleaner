namespace WindowsDiskCleaner.Core
{
    public sealed class CleanupCategorySummary
    {
        public CleanupCategorySummary(CleanupCategoryKind kind, string displayName, string description, int fileCount, long totalBytes)
        {
            Kind = kind;
            DisplayName = displayName;
            Description = description;
            FileCount = fileCount;
            TotalBytes = totalBytes;
        }

        public CleanupCategoryKind Kind { get; private set; }

        public string DisplayName { get; private set; }

        public string Description { get; private set; }

        public int FileCount { get; private set; }

        public long TotalBytes { get; private set; }
    }
}
