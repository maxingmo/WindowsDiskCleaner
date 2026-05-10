namespace WindowsDiskCleaner.Core
{
    public sealed class ScanProgress
    {
        public ScanProgress(string currentPath, int fileCount, int skippedCount)
        {
            CurrentPath = currentPath;
            FileCount = fileCount;
            SkippedCount = skippedCount;
        }

        public string CurrentPath { get; private set; }

        public int FileCount { get; private set; }

        public int SkippedCount { get; private set; }
    }
}

