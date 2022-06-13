using System;


namespace GameLoopLib.Engine
{
    public interface IGameLoop
    {
        void Start();
        void Stop();
        bool IsRunning { get; }
        event EventHandler UpdateGame;
        event EventHandler<InterpolationEventArgs> RenderGame;
    }
}