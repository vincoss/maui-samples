using Microsoft.Maui.Graphics;
using System.Numerics;

namespace GraphicsView_Samples.Models;

/// <summary>
/// This class represents a single wire polygon.
/// </summary>
public class Polygon
{
    private readonly Color Color;
    public readonly Vector2[] Corners;
    public readonly Vector2[] Velocities;

    /// <summary>
    /// Create a random polygon within a field
    /// </summary>
    public Polygon(Random rand, float width, float height, int corners, Color color)
    {
        Color = color;

        Corners = Enumerable.Range(0, corners)
            .Select(x => new Vector2(
                x: (float)rand.NextDouble() * width,
                y: (float)rand.NextDouble() * height))
            .ToArray();
        
        Velocities = Enumerable.Range(0, corners)
            .Select(x => new Vector2(
                x: (float)Math.Clamp(rand.NextDouble(), .1, 1) * (rand.NextDouble() > .5 ? 1 : -1),
                y: (float)Math.Clamp(rand.NextDouble(), .1, 1) * (rand.NextDouble() > .5 ? 1 : -1)))
            .ToArray();
    }

    /// <summary>
    /// Create the next polygon given an existing moving polygon
    /// </summary>
    public Polygon(Polygon previous, float width, float height, float timeDelta)
    {
        Color = previous.Color;

        Corners = new Vector2[previous.Corners.Length];
        Velocities = new Vector2[previous.Corners.Length];

        for (int i = 0; i < previous.Corners.Length; i++)
        {
            Velocities[i] = previous.Velocities[i];
            Corners[i] = previous.Corners[i];

            Corners[i] += Velocities[i] * timeDelta;

            if (Corners[i].X < 0)
            {
                Corners[i].X = 0;
                Velocities[i].X *= -1;
            }
            else if (Corners[i].X > width)
            {
                Corners[i].X = width;
                Velocities[i].X *= -1;
            }

            if (Corners[i].Y < 0)
            {
                Corners[i].Y = 0;
                Velocities[i].Y *= -1;
            }
            else if (Corners[i].Y > height)
            {
                Corners[i].Y = height;
                Velocities[i].Y *= -1;
            }
        }
    }

    public void Draw(ICanvas canvas)
    {
        PathF path = new(Corners.First().X, Corners.First().Y);

        foreach (Vector2 corner in Corners)
        {
            path.LineTo(corner.X, corner.Y);
        }

        path.Close();

        canvas.StrokeColor = Color;
        canvas.DrawPath(path);
    }
}