using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class ScanError
    {
        public ScanError(string path, string message, string errorType)
        {
            Path = path;
            Message = message;
            ErrorType = errorType;
        }

        public string Path { get; private set; }

        public string Message { get; private set; }

        public string ErrorType { get; private set; }

        public static ScanError FromException(string path, Exception exception)
        {
            return new ScanError(path, exception.Message, exception.GetType().Name);
        }
    }
}

