using System;
using System.Collections.Generic;
using GGame.Math;

namespace VelcroPhysics.Tools.Triangulation.Delaunay.Util
{
    internal class PointGenerator
    {
        private static readonly SRandom RNG = new SRandom(1234);

        public static List<TriangulationPoint> UniformDistribution(int n, GGame.Math.Fix64 scale)
        {
            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n; i++)
            {
                points.Add(new TriangulationPoint(scale * (0.5 - RNG.Next()), scale * (0.5 - RNG.Next())));
            }
            return points;
        }

        public static List<TriangulationPoint> UniformGrid(int n, GGame.Math.Fix64 scale)
        {
            GGame.Math.Fix64 x = 0;
            GGame.Math.Fix64 size = scale / n;
            GGame.Math.Fix64 halfScale = 0.5 * scale;

            List<TriangulationPoint> points = new List<TriangulationPoint>();
            for (int i = 0; i < n + 1; i++)
            {
                x = halfScale - i * size;
                for (int j = 0; j < n + 1; j++)
                {
                    points.Add(new TriangulationPoint(x, halfScale - j * size));
                }
            }
            return points;
        }
    }
}