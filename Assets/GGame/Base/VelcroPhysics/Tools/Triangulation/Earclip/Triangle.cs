using Microsoft.Xna.Framework;
using VelcroPhysics.Shared;

namespace VelcroPhysics.Tools.Triangulation.Earclip {
    public class Triangle : Vertices
    {
        //Constructor automatically fixes orientation to ccw
        public Triangle(GGame.Math.Fix64 x1, GGame.Math.Fix64 y1, GGame.Math.Fix64 x2, GGame.Math.Fix64 y2, GGame.Math.Fix64 x3, GGame.Math.Fix64 y3)
        {
            GGame.Math.Fix64 cross = (x2 - x1) * (y3 - y1) - (x3 - x1) * (y2 - y1);
            if (cross > 0)
            {
                Add(new Vector2(x1, y1));
                Add(new Vector2(x2, y2));
                Add(new Vector2(x3, y3));
            }
            else
            {
                Add(new Vector2(x1, y1));
                Add(new Vector2(x3, y3));
                Add(new Vector2(x2, y2));
            }
        }

        public bool IsInside(GGame.Math.Fix64 x, GGame.Math.Fix64 y)
        {
            Vector2 a = this[0];
            Vector2 b = this[1];
            Vector2 c = this[2];

            if (x < a.X && x < b.X && x < c.X) return false;
            if (x > a.X && x > b.X && x > c.X) return false;
            if (y < a.Y && y < b.Y && y < c.Y) return false;
            if (y > a.Y && y > b.Y && y > c.Y) return false;

            GGame.Math.Fix64 vx2 = x - a.X;
            GGame.Math.Fix64 vy2 = y - a.Y;
            GGame.Math.Fix64 vx1 = b.X - a.X;
            GGame.Math.Fix64 vy1 = b.Y - a.Y;
            GGame.Math.Fix64 vx0 = c.X - a.X;
            GGame.Math.Fix64 vy0 = c.Y - a.Y;

            GGame.Math.Fix64 dot00 = vx0 * vx0 + vy0 * vy0;
            GGame.Math.Fix64 dot01 = vx0 * vx1 + vy0 * vy1;
            GGame.Math.Fix64 dot02 = vx0 * vx2 + vy0 * vy2;
            GGame.Math.Fix64 dot11 = vx1 * vx1 + vy1 * vy1;
            GGame.Math.Fix64 dot12 = vx1 * vx2 + vy1 * vy2;
            GGame.Math.Fix64 invDenom = 1.0f / (dot00 * dot11 - dot01 * dot01);
            GGame.Math.Fix64 u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            GGame.Math.Fix64 v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            return ((u > 0) && (v > 0) && (u + v < 1));
        }
    }
}