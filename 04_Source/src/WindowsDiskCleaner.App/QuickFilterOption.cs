using System.Collections.Generic;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class QuickFilterOption
    {
        public QuickFilterOption(string label, QuickFileFilter value)
        {
            Label = label;
            Value = value;
        }

        public string Label { get; private set; }

        public QuickFileFilter Value { get; private set; }

        public static IEnumerable<QuickFilterOption> CreateDefaults()
        {
            return new[]
            {
                new QuickFilterOption("全部文件", QuickFileFilter.All),
                new QuickFilterOption("大文件 >= 100MB", QuickFileFilter.LargeFiles),
                new QuickFilterOption("视频文件", QuickFileFilter.Videos),
                new QuickFilterOption("压缩包", QuickFileFilter.Archives),
                new QuickFilterOption("安装包", QuickFileFilter.Installers),
                new QuickFilterOption("日志/临时文件", QuickFileFilter.LogsAndTemporary)
            };
        }
    }
}

