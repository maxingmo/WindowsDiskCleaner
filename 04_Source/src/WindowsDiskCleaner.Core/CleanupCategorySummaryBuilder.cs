using System.Collections.Generic;
using System.Linq;

namespace WindowsDiskCleaner.Core
{
    public sealed class CleanupCategorySummaryBuilder
    {
        private const long LargeFileThresholdBytes = 100L * 1024L * 1024L;
        private readonly FileRiskClassifier _riskClassifier;

        public CleanupCategorySummaryBuilder()
        {
            _riskClassifier = new FileRiskClassifier();
        }

        public IEnumerable<CleanupCategorySummary> Build(IEnumerable<FileEntry> files)
        {
            var list = files == null ? new List<FileEntry>() : files.ToList();

            yield return Create(
                CleanupCategoryKind.HighRisk,
                "\u9ad8\u98ce\u9669",
                "\u7cfb\u7edf\u6216\u7a0b\u5e8f\u5b89\u88c5\u76ee\u5f55\uff0c\u5220\u9664\u524d\u9700\u8981\u6838\u5bf9\u3002",
                list.Where(IsHighRisk));

            yield return Create(
                CleanupCategoryKind.CacheTemporary,
                "\u7f13\u5b58/\u4e34\u65f6",
                "\u7f13\u5b58\u3001\u65e5\u5fd7\u3001\u4e34\u65f6\u6587\u4ef6\uff0c\u9002\u5408\u4f18\u5148\u68c0\u67e5\u3002",
                list.Where(file => _riskClassifier.Classify(file).Level == FileRiskLevel.CacheTemporary));

            yield return Create(
                CleanupCategoryKind.UserData,
                "\u7528\u6237\u6570\u636e",
                "\u4e0b\u8f7d\u3001\u56fe\u7247\u3001\u6587\u6863\u7b49\u7528\u6237\u6587\u4ef6\uff0c\u9700\u624b\u52a8\u786e\u8ba4\u3002",
                list.Where(file => _riskClassifier.Classify(file).Level == FileRiskLevel.UserData));

            yield return Create(
                CleanupCategoryKind.LargeFiles,
                "\u5927\u6587\u4ef6",
                "\u5927\u4e8e 100MB \u7684\u6587\u4ef6\uff0c\u9002\u5408\u505a\u7a7a\u95f4\u5206\u6790\u3002",
                list.Where(file => file.SizeBytes >= LargeFileThresholdBytes));

            yield return Create(
                CleanupCategoryKind.UnknownCaution,
                "\u672a\u77e5/\u8c28\u614e",
                "\u672a\u5339\u914d\u5230\u660e\u786e\u7c7b\u522b\u7684\u6587\u4ef6\uff0c\u5efa\u8bae\u8c28\u614e\u5904\u7406\u3002",
                list.Where(file => _riskClassifier.Classify(file).Level == FileRiskLevel.UnknownCaution));
        }

        private static CleanupCategorySummary Create(
            CleanupCategoryKind kind,
            string displayName,
            string description,
            IEnumerable<FileEntry> files)
        {
            var list = files.ToList();
            return new CleanupCategorySummary(kind, displayName, description, list.Count, list.Sum(file => file.SizeBytes));
        }

        private bool IsHighRisk(FileEntry file)
        {
            var level = _riskClassifier.Classify(file).Level;
            return level == FileRiskLevel.System || level == FileRiskLevel.ProgramInstall;
        }
    }
}
