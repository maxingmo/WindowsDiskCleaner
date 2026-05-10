namespace WindowsDiskCleaner.Core
{
    public sealed class FileDeleteResult
    {
        private FileDeleteResult(bool succeeded, string deletedPath, string message)
        {
            Succeeded = succeeded;
            DeletedPath = deletedPath;
            Message = message;
        }

        public bool Succeeded { get; private set; }

        public string DeletedPath { get; private set; }

        public string Message { get; private set; }

        public static FileDeleteResult Success(string deletedPath, string message)
        {
            return new FileDeleteResult(true, deletedPath, message);
        }

        public static FileDeleteResult Failure(string deletedPath, string message)
        {
            return new FileDeleteResult(false, deletedPath, message);
        }
    }
}

