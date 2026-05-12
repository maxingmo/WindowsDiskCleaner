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
                new QuickFilterOption("\u5168\u90e8\u6587\u4ef6", QuickFileFilter.All),
                new QuickFilterOption("\u5927\u6587\u4ef6 >= 100MB", QuickFileFilter.LargeFiles),
                new QuickFilterOption("\u89c6\u9891\u6587\u4ef6", QuickFileFilter.Videos),
                new QuickFilterOption("\u538b\u7f29\u5305", QuickFileFilter.Archives),
                new QuickFilterOption("\u5b89\u88c5\u5305", QuickFileFilter.Installers),
                new QuickFilterOption("\u65e5\u5fd7/\u4e34\u65f6\u6587\u4ef6", QuickFileFilter.LogsAndTemporary)
            };
        }
    }
}
