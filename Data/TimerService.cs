using System.Timers;
using Timer = System.Timers.Timer;

namespace CbeViewer.Data
{
    public class TimerService : IDisposable
    {
        private readonly Timer _timer;

        public event Action OnElapsed;

        public TimerService()
        {
            _timer = new Timer(1000); // Set the interval to 1 second (1000 ms)
            _timer.Elapsed += (sender, e) =>
            {
                ElapsedSeconds++;
                OnElapsed?.Invoke();
            };
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public int ElapsedSeconds { get; private set; }

        public void Dispose() => _timer?.Dispose();
    }
}