using Font = Microsoft.Maui.Graphics.Font;

namespace GraphicsView_Samples.Pages;

public partial class DrawGraphicalObjectsPage : ContentPage
{
    private double _width = 0;
    private double _height = 0;

    public DrawGraphicalObjectsPage()
	{
		InitializeComponent();
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
}

public class DrawGraphicalObjectsPageDrawable : IDrawable
{
    private static readonly Random _random = new Random();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = Color.FromArgb("#003366");
        canvas.FillRectangle(dirtyRect);

        canvas.StrokeColor = Colors.Red;
        canvas.StrokeSize = 6;
        canvas.DrawLine(10, 10, 90, 100);
        canvas.DrawLine(300, 10, 390, 100);

        canvas.Font = Font.Default;
        canvas.FontColor = Colors.White;
        canvas.DrawString($"Canvas size: w: {dirtyRect.Width} h: {dirtyRect.Height}", 10, 10, 380, 100, HorizontalAlignment.Left, VerticalAlignment.Top);

    }
}