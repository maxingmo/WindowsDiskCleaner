using System.Collections.Generic;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class RiskFilterOption
    {
        public RiskFilterOption(string label, FileRiskLevel? value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; private set; }

        public FileRiskLevel? Value { get; private set; }

        public static IEnumerable<RiskFilterOption> CreateDefaults()
        {
            return new[]
            {
                new RiskFilterOption("\u5168\u90e8\u7ea7\u522b", null),
                new RiskFilterOption("\u7cfb\u7edf\u7ea7", FileRiskLevel.System),
                new RiskFilterOption("\u7a0b\u5e8f\u5b89\u88c5\u7ea7", FileRiskLevel.ProgramInstall),
                new RiskFilterOption("\u7528\u6237\u6570\u636e\u7ea7", FileRiskLevel.UserData),
                new RiskFilterOption("\u7f13\u5b58/\u4e34\u65f6\u7ea7", FileRiskLevel.CacheTemporary),
                new RiskFilterOption("\u672a\u77e5/\u8c28\u614e\u7ea7", FileRiskLevel.UnknownCaution)
            };
        }
    }
}
