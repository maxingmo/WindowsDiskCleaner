using System;

namespace WindowsDiskCleaner.Core
{
    public interface IDebounceTimer
    {
        event EventHandler Elapsed;

        void Restart();

        void Stop();
    }
}

