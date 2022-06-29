using System;
using System.Threading;


namespace GameLoopLib.Engine
{
    public class GameLoop : IGameLoop
    {
        public void Start()
        {
            IsRunning = true;
            var t = new Thread(Loop);
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        #region Private methods

        private void Loop()
        {
            var MS_PER_UPDATE = 100;
            var previous = Environment.TickCount;
            var lag = 0;
            while (true)
            {
                var current = Environment.TickCount;
                var elapsed = current - previous;
                previous = current;
                lag += elapsed;

                ///processInput();

                while (lag >= MS_PER_UPDATE)
                {
                    OnUpdateGame();
                    lag -= MS_PER_UPDATE;
                }

                OnRenderGame(0.0F);
            }

            //const int ticksPerSecond = 25;
            //const int skipTicks = 1000 / ticksPerSecond;
            //const int maxFrameSkip = 5;

            //var nextGameTick = Environment.TickCount;

            //while (IsRunning)
            //{
            //    int loops = 0;

            //    while (Environment.TickCount > nextGameTick && loops < maxFrameSkip)
            //    {
            //        OnUpdateGame();

            //        nextGameTick += skipTicks;
            //        loops++;
            //    }

            //    // Usage view_position = position + (speed * interpolation)
            //    float interpolation = (Environment.TickCount + skipTicks - nextGameTick) / (float)skipTicks;
            //    OnRenderGame(interpolation);
            //}
        }

        private void OnUpdateGame()
        {
            var handler = this.UpdateGame;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void OnRenderGame(float interpolation)
        {
            var handler = this.RenderGame;
            if (handler != null)
            {
                handler(this, new InterpolationEventArgs(interpolation));
            }
        }

        #endregion

        public event EventHandler UpdateGame;

        public event EventHandler<InterpolationEventArgs> RenderGame;

        public bool IsRunning { get; private set; }
    }
}
