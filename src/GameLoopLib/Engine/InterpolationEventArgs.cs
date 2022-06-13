using System;


namespace GameLoopLib.Engine
{
    public class InterpolationEventArgs : EventArgs
    {
        public InterpolationEventArgs(float interpolation)
        {
            if (interpolation < 0)
            {
                throw new ArgumentOutOfRangeException("interpolation");
            }
            this.Interpolation = interpolation;
        }

        public float Interpolation { get; private set; }
    }
}