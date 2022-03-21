using Microsoft.Xna.Framework;

public class Collision
{
    public static Vector2 CalculateIntersectionDepth(Rectangle rectA, Rectangle rectB)
    {
        Vector2 minDistance = new Vector2(rectA.Width + rectB.Width,
            rectA.Height + rectB.Height) / 2;
        Vector2 centerA = new Vector2(rectA.Center.X, rectA.Center.Y);
        Vector2 centerB = new Vector2(rectB.Center.X, rectB.Center.Y);
        Vector2 distance = centerA - centerB;
        Vector2 depth = Vector2.Zero;
        if (distance.X > 0)
        {
            depth.X = minDistance.X - distance.X;
        }
        else
        {
            depth.X = -minDistance.X - distance.X;
        }
        if (distance.Y > 0)
        {
            depth.Y = minDistance.Y - distance.Y;
        }
        else
        {
            depth.Y = -minDistance.Y - distance.Y;
        }
        return depth;
    }

    public static Rectangle Intersection(Rectangle rect1, Rectangle rect2)
    {
        int xmin = (int)MathHelper.Max(rect1.Left, rect2.Left);
        int xmax = (int)MathHelper.Min(rect1.Right, rect2.Right);
        int ymin = (int)MathHelper.Max(rect1.Top, rect2.Top);
        int ymax = (int)MathHelper.Min(rect1.Bottom, rect2.Bottom);
        return new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin);
    }
}

