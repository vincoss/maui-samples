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
            const int desiredFps = 60;
            int fps = 0;
            int framesRendered = 0;
            long delayTicks = (1000 / desiredFps) * TimeSpan.TicksPerMillisecond;
            var previousTicks = DateTime.Now.Ticks;
            var t2 = previousTicks;

            while (IsRunning)
            {
                var current = DateTime.Now.Ticks;
                var elapsedTicks = current - previousTicks;
                var elapsedSeconds = (current - t2) / TimeSpan.TicksPerSecond;
                previousTicks = current;

                // Fps seconds
                if (elapsedSeconds >= 1)
                {
                    fps = framesRendered;
                    framesRendered = 0;
                    t2 = current;
                }

                OnUpdateGame();
                OnRenderGame(fps);

                framesRendered++;
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
