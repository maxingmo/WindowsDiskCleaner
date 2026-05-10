using System;

namespace WindowsDiskCleaner.Core
{
    public sealed class DebouncedAction
    {
        private readonly IDebounceTimer _timer;
        private readonly Action _action;

        public DebouncedAction(IDebounceTimer timer, Action action)
        {
            if (timer == null)
            {
                throw new ArgumentNullException("timer");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            _timer = timer;
            _action = action;
            _timer.Elapsed += OnElapsed;
        }

        public void Schedule()
        {
            _timer.Restart();
        }

        public void Cancel()
        {
            _timer.Stop();
        }

        public void RunNow()
        {
            _timer.Stop();
            _action();
        }

        private void OnElapsed(object sender, EventArgs args)
        {
            _action();
        }
    }
}

