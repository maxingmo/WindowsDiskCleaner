using System.Collections.Generic;

namespace WindowsDiskCleaner.Core
{
    public sealed class ScanResult
    {
        public ScanResult(IReadOnlyList<FileEntry> files, IReadOnlyList<ScanError> errors, bool wasCancelled)
        {
            Files = files;
            Errors = errors;
            WasCancelled = wasCancelled;
        }

        public IReadOnlyList<FileEntry> Files { get; private set; }

        public IReadOnlyList<ScanError> Errors { get; private set; }

        public int SkippedCount
        {
            get { return Errors.Count; }
        }

        public bool WasCancelled { get; private set; }
    }
}

