using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WindowsDiskCleaner.Core
{
    public sealed class FileBatchDeleteResult
    {
        private FileBatchDeleteResult(
            bool succeeded,
            int selectedCount,
            int deletedCount,
            int failedCount,
            string message,
            IEnumerable<string> deletedPaths)
        {
            Succeeded = succeeded;
            SelectedCount = selectedCount;
            DeletedCount = deletedCount;
            FailedCount = failedCount;
            Message = message;
            DeletedPaths = new ReadOnlyCollection<string>(new List<string>(deletedPaths ?? new string[0]));
        }

        public bool Succeeded { get; private set; }

        public int SelectedCount { get; private set; }

        public int DeletedCount { get; private set; }

        public int FailedCount { get; private set; }

        public string Message { get; private set; }

        public IReadOnlyList<string> DeletedPaths { get; private set; }

        public static FileBatchDeleteResult Success(int selectedCount, int deletedCount, string message, IEnumerable<string> deletedPaths)
        {
            return new FileBatchDeleteResult(true, selectedCount, deletedCount, 0, message, deletedPaths);
        }

        public static FileBatchDeleteResult Failure(int selectedCount, int deletedCount, int failedCount, string message, IEnumerable<string> deletedPaths)
        {
            return new FileBatchDeleteResult(false, selectedCount, deletedCount, failedCount, message, deletedPaths);
        }
    }
}
