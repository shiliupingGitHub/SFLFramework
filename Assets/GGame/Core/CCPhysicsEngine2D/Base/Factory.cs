using System.Collections.Generic;
using GGame.Core.Physics;

namespace GGame.Core.Physics
{
    public static class Factory
    {
        public static Body CreateRectangleBody(double x, double y, double width, double height, bool isStatic = false)
        {
            var path = new List<Point> { new Point(0, 0), new Point(width, 0), new Point(width, height), new Point(0,height) };
            var body = new Body {Static = isStatic};
            body.Init(path);
            body.Position = new Point(x, y);
            return body;
        }
    }
}
