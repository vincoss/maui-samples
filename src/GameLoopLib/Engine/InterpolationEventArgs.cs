using System;


namespace GameLoopLib.Engine
{
    public class InterpolationEventArgs : EventArgs
    {
        public InterpolationEventArgs(int fps)
        {
            if (fps < 0)
            {
                throw new ArgumentOutOfRangeException("interpolation");
            }
            this.Fps = fps;
        }

        public int Fps { get; private set; }
    }
}