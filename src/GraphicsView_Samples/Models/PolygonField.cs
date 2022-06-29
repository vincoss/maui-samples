using Microsoft.Maui.Graphics;

namespace GraphicsView_Samples.Models;

public class PolygonField : IDrawable
{
    public readonly PolygonHistory[] PolygonHistories;
    public Color BackgroundColor = Colors.Black;

    public PolygonField(float width, float height, int polygons, int corners)
    {
        Random rand = new(5);

        PolygonHistories = new PolygonHistory[polygons];

        for (int i = 0; i < polygons; i++)
        {
            Polygon initial = new(rand, width, height, corners, RandomColor(rand));
            PolygonHistories[i] = new PolygonHistory(initial);
        }
    }

    public Color RandomColor(Random rand) => Color.FromHsv((float)rand.NextDouble(), .5f, 1);

    public void Advance(float width, float height, float timeDelta, int maxHistory)
    {
        foreach (var poly in PolygonHistories)
            poly.Advance(width, height, timeDelta, maxHistory);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = BackgroundColor;

        // Maui-themed background
        var paint = new LinearGradientPaint
        {
            StartColor = Color.FromArgb("#7b1443"),
            EndColor = Color.FromArgb("#2a0056"),
            StartPoint = new Point(0, 0),
            EndPoint = new Point(0, 1),
        };
        canvas.SetFillPaint(paint, dirtyRect);
        canvas.FillRectangle(dirtyRect);

        foreach (var poly in PolygonHistories)
        {
            poly.Draw(canvas);
        }
    }
}