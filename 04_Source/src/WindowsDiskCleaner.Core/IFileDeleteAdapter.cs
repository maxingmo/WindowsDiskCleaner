namespace WindowsDiskCleaner.Core
{
    public interface IFileDeleteAdapter
    {
        void MoveToRecycleBin(string path);
    }
}

