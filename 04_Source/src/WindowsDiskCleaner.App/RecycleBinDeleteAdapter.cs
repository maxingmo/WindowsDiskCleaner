using Microsoft.VisualBasic.FileIO;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class RecycleBinDeleteAdapter : IFileDeleteAdapter
    {
        public void MoveToRecycleBin(string path)
        {
            FileSystem.DeleteFile(
                path,
                UIOption.OnlyErrorDialogs,
                RecycleOption.SendToRecycleBin,
                UICancelOption.ThrowException);
        }
    }
}

