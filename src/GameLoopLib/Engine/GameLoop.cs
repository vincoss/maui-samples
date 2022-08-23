using System;
using System.Diagnostics;
using System.Threading;


namespace GameLoopLib.Engine
{
    public class GameLoop : IGameLoop
    {
        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            var t = new Thread(Loop);
            t.Start();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        #region Private methods

        private void Loop()
        {
            int fps = 60;
            int fpsTicNum = 0;
            int _framesRendered = 0;
            long delayTicks = (1000 / fps) * TimeSpan.TicksPerMillisecond;
            var previous = DateTime.Now.Ticks;
            var seconds = previous;

            while (IsRunning)
            {
                var current = DateTime.Now.Ticks;
                var elapsedTicks = current - previous;
                var elapsedSeconds = (current - seconds) / TimeSpan.TicksPerSecond;
                previous = current;

                // Fps seconds
                if (elapsedSeconds >= 1)
                {
                    fpsTicNum = _framesRendered;
                    _framesRendered = 0;
                    seconds = current;
                }

                OnUpdateGame();
                OnRenderGame(fpsTicNum);

                _framesRendered++;
                var delay = delayTicks - elapsedTicks;
                var delayMilliseconds = (int)(delay / TimeSpan.TicksPerMillisecond);

                if (delayMilliseconds > 0)
                {
                    Thread.Sleep(delayMilliseconds);
                }
            }
        }

        private void OnUpdateGame()
        {
            var handler = this.UpdateGame;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnRenderGame(int fps)
        {
            var handler = this.RenderGame;
            if (handler != null)
            {
                handler(this, new InterpolationEventArgs(fps));
            }
        }

        #endregion

        public event EventHandler UpdateGame;

        public event EventHandler<InterpolationEventArgs> RenderGame;

        public bool IsRunning { get; private set; }
    }
}
