using System;
using System.Windows.Threading;
using WindowsDiskCleaner.Core;

namespace WindowsDiskCleaner.App
{
    public sealed class DispatcherDebounceTimer : IDebounceTimer
    {
        private readonly DispatcherTimer _timer;

        public DispatcherDebounceTimer(TimeSpan delay)
        {
            _timer = new DispatcherTimer
            {
                Interval = delay
            };
            _timer.Tick += OnTick;
        }

        public event EventHandler Elapsed;

        public void Restart()
        {
            _timer.Stop();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTick(object sender, EventArgs args)
        {
            _timer.Stop();
            var handler = Elapsed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}

