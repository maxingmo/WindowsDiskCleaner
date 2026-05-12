using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class CleanupCategoryCardViewModel
    {
        private readonly CleanupCategorySummary _summary;

        public CleanupCategoryCardViewModel(CleanupCategorySummary summary)
        {
            _summary = summary;
        }

        public CleanupCategoryKind Kind
        {
            get { return _summary.Kind; }
        }

        public string Title
        {
            get { return _summary.DisplayName; }
        }

        public string Description
        {
            get { return _summary.Description; }
        }

        public int FileCount
        {
            get { return _summary.FileCount; }
        }

        public long TotalBytes
        {
            get { return _summary.TotalBytes; }
        }

        public string SizeDisplay
        {
            get { return FormatSize(_summary.TotalBytes); }
        }

        public string CountDisplay
        {
            get { return _summary.FileCount + " \u4e2a\u6587\u4ef6"; }
        }

        public string IconText
        {
            get
            {
                switch (_summary.Kind)
                {
                    case CleanupCategoryKind.HighRisk:
                        return "!";
                    case CleanupCategoryKind.CacheTemporary:
                        return "~";
                    case CleanupCategoryKind.UserData:
                        return "U";
                    case CleanupCategoryKind.LargeFiles:
                        return "B";
                    default:
                        return "?";
                }
            }
        }

        public string AccentHex
        {
            get
            {
                switch (_summary.Kind)
                {
                    case CleanupCategoryKind.HighRisk:
                        return "#ef4444";
                    case CleanupCategoryKind.CacheTemporary:
                        return "#10b981";
                    case CleanupCategoryKind.UserData:
                        return "#3b82f6";
                    case CleanupCategoryKind.LargeFiles:
                        return "#f97316";
                    default:
                        return "#6b7280";
                }
            }
        }

        public string BackgroundHex
        {
            get
            {
                switch (_summary.Kind)
                {
                    case CleanupCategoryKind.HighRisk:
                        return "#fef2f2";
                    case CleanupCategoryKind.CacheTemporary:
                        return "#ecfdf5";
                    case CleanupCategoryKind.UserData:
                        return "#eff6ff";
                    case CleanupCategoryKind.LargeFiles:
                        return "#fff7ed";
                    default:
                        return "#f9fafb";
                }
            }
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB" };
            var size = (double)bytes;
            var unit = 0;

            while (size >= 1024 && unit < units.Length - 1)
            {
                size = size / 1024;
                unit++;
            }

            return unit == 0
                ? bytes + " " + units[unit]
                : size.ToString("0.##") + " " + units[unit];
        }
    }
}
