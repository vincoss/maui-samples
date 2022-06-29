using System.Timers;
using SystemTimer = System.Timers;

namespace GraphicsView_Samples.Pages;

public partial class TimerDrawLinesPage : ContentPage
{
    private SystemTimer.Timer _timer;

    public TimerDrawLinesPage()
    {
        InitializeComponent();

        _timer = new SystemTimer.Timer();
        _timer.Interval = 20;
        _timer.Elapsed += TimerElapsed;
        _timer.AutoReset = true;

         
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _timer.Enabled = true;
    }

    private void TimerElapsed(Object source, ElapsedEventArgs e)
    {
        canvas.Invalidate();
    }
}

public class TimerDrawLinesPageDrawable : IDrawable
{
    private static readonly Random _random = new Random();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Color.FromArgb("#003366");
        canvas.FillRectangle(dirtyRect);

        canvas.StrokeSize = 1;
        canvas.StrokeColor = Color.FromRgba(255, 255, 255, 100);
        for (int i = 0; i < 1000; i++)
        {
            canvas.DrawLine(
                x1: (float)_random.NextDouble() * dirtyRect.Width,
                y1: (float)_random.NextDouble() * dirtyRect.Height,
                x2: (float)_random.NextDouble() * dirtyRect.Width,
                y2: (float)_random.NextDouble() * dirtyRect.Height);
        }
    }
}