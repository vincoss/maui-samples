using Microsoft.Maui.Graphics;

namespace GraphicsView_Samples.Models;

public class PolygonHistory
{
    private readonly List<Polygon> Polygons = new();

    public PolygonHistory(Polygon initial)
    {
        Polygons.Add(initial);
    }

    public void Advance(float width, float height, float timeDelta, int maxHistory)
    {
        var nextState = new Polygon(Polygons.Last(), width, height, timeDelta);
        Polygons.Add(nextState);
        while (Polygons.Count > maxHistory)
            Polygons.RemoveAt(0);
    }

    public void Draw(ICanvas canvas)
    {
        for(int i = 0; i < Polygons.Count; i++)
        {
            var poly = Polygons[i];
            poly.Draw(canvas);
        }
    }
}