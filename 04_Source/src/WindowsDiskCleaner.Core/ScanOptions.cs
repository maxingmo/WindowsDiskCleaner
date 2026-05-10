using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class ScanOptions
    {
        public ScanOptions(string rootPath)
        {
            if (string.IsNullOrWhiteSpace(rootPath))
            {
                throw new ArgumentException("Scan root path is required.", "rootPath");
            }

            RootPath = rootPath;
        }

        public string RootPath { get; private set; }
    }
}

