using GraphicsView_Samples.Models;
using System.Timers;
using SystemTimer = System.Timers;


namespace GraphicsView_Samples.Pages;

public partial class TimerMistifyLinesPage : ContentPage
{
    private double _width = 0;
    private double _height = 0;
    private SystemTimer.Timer _timer;
    private readonly PolygonField Field;

    public TimerMistifyLinesPage()
    {
        InitializeComponent();

        _timer = new SystemTimer.Timer();
        _timer.Interval = 20;
        _timer.Elapsed += TimerElapsed;
        _timer.AutoReset = true;

        Field = new PolygonField((float)canvas.Width, (float)canvas.Height, 50, 2);
        canvas.Drawable = Field;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height); //must be called

        if (this._width != width || this._height != height)
        {
            this._width = width;
            this._height = height;

            canvas.Invalidate();
        }
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        _timer.Enabled = true;
    }

    private void TimerElapsed(Object source, ElapsedEventArgs e)
    {
        Field.Advance((float)canvas.Width, (float)canvas.Height, 10, 10);
        canvas.Invalidate();
    }
}