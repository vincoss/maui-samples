using Microsoft.Maui.Graphics;

namespace GraphicsView_Samples.Models;

public class Ball
{
    public double X;
    public double Y;
    public double Radius = 5;
    public double XVel;
    public double YVel;
    public byte R, G, B;

    public void Advance(double timeDelta, double width, double height)
    {
        MoveForward(timeDelta);
        Bounce(width, height);
    }

    private void MoveForward(double timeDelta)
    {
        X += XVel * timeDelta;
        Y += YVel * timeDelta;
    }

    private void Bounce(double width, double height)
    {
        double minX = Radius;
        double minY = Radius;
        double maxX = width - Radius;
        double maxY = height - Radius;

        if (X < minX)
        {
            double over = minX - X;
            X = minX + over;
            XVel = -XVel;
        }
        else if (X > maxX)
        {
            double over = X - maxX;
            X = maxX - over;
            XVel = -XVel;
        }

        if (Y < minY)
        {
            double over = minY - Y;
            Y = minY + over;
            YVel = -YVel;
        }
        else if (Y > maxY)
        {
            double over = Y - maxY;
            Y = maxY - over;
            YVel = -YVel;
        }
    }

    public void Draw(ICanvas canvas)
    {
        canvas.FillColor = Color.FromRgb(R, G, B);
        canvas.FillCircle((float)X, (float)Y, (float)Radius);
    }
}
