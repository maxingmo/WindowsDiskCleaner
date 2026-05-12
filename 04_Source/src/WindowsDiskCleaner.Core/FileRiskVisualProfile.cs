namespace WindowsDiskCleaner.Core
{
    public sealed class FileRiskVisualProfile
    {
        private FileRiskVisualProfile(
            FileRiskLevel level,
            string badgeText,
            string foregroundHex,
            string backgroundHex,
            string borderHex,
            bool isHighRisk)
        {
            Level = level;
            BadgeText = badgeText;
            ForegroundHex = foregroundHex;
            BackgroundHex = backgroundHex;
            BorderHex = borderHex;
            IsHighRisk = isHighRisk;
        }

        public FileRiskLevel Level { get; private set; }

        public string BadgeText { get; private set; }

        public string ForegroundHex { get; private set; }

        public string BackgroundHex { get; private set; }

        public string BorderHex { get; private set; }

        public bool IsHighRisk { get; private set; }

        public static FileRiskVisualProfile ForLevel(FileRiskLevel level)
        {
            switch (level)
            {
                case FileRiskLevel.System:
                    return new FileRiskVisualProfile(level, "\u9ad8\u98ce\u9669", "#991b1b", "#fee2e2", "#fca5a5", true);
                case FileRiskLevel.ProgramInstall:
                    return new FileRiskVisualProfile(level, "\u9ad8\u98ce\u9669", "#9a3412", "#ffedd5", "#fdba74", true);
                case FileRiskLevel.UserData:
                    return new FileRiskVisualProfile(level, "\u7528\u6237\u6587\u4ef6", "#1e40af", "#dbeafe", "#93c5fd", false);
                case FileRiskLevel.CacheTemporary:
                    return new FileRiskVisualProfile(level, "\u53ef\u6e05\u7406", "#166534", "#dcfce7", "#86efac", false);
                default:
                    return new FileRiskVisualProfile(level, "\u8c28\u614e", "#374151", "#f3f4f6", "#d1d5db", false);
            }
        }
    }
}
