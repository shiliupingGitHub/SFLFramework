using System;
using System.Collections.Generic;
using GGame.Math;
using Microsoft.Xna.Framework;
using VelcroPhysics.Collision.Shapes;
using VelcroPhysics.Dynamics;
using VelcroPhysics.Shared;
using VelcroPhysics.Templates;
using VelcroPhysics.Tools.Triangulation.TriangulationBase;
using VelcroPhysics.Utilities;

namespace VelcroPhysics.Factories
{
    public static class BodyFactory
    {
        public static Body CreateBody(World world, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            BodyTemplate template = new BodyTemplate();
            template.Position = position;
            template.Angle = rotation;
            template.Type = bodyType;
            template.UserData = userData;

            return world.CreateBody(template);
        }

        public static Body CreateEdge(World world, Vector2 start, Vector2 end, object userData = null)
        {
            Body body = CreateBody(world);
            body.UserData = userData;

            FixtureFactory.AttachEdge(start, end, body);
            return body;
        }

        public static Body CreateChainShape(World world, Vertices vertices, Vector2 position = new Vector2(), object userData = null)
        {
            Body body = CreateBody(world, position);
            body.UserData = userData;

            FixtureFactory.AttachChainShape(vertices, body);
            return body;
        }

        public static Body CreateLoopShape(World world, Vertices vertices, Vector2 position = new Vector2(), object userData = null)
        {
            Body body = CreateBody(world, position);
            body.UserData = userData;

            FixtureFactory.AttachLoopShape(vertices, body);
            return body;
        }

        public static Body CreateRectangle(World world, GGame.Math.Fix64 width, GGame.Math.Fix64 height, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be more than 0 meters");

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be more than 0 meters");

            Body body = CreateBody(world, position, rotation, bodyType, userData);

            Vertices rectangleVertices = PolygonUtils.CreateRectangle(width / 2, height / 2);
            FixtureFactory.AttachPolygon(rectangleVertices, density, body);

            return body;
        }

        public static Body CreateCircle(World world, GGame.Math.Fix64 radius, GGame.Math.Fix64 density, Vector2 position = new Vector2(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Body body = CreateBody(world, position, 0, bodyType, userData);
            FixtureFactory.AttachCircle(radius, density, body);
            return body;
        }

        public static Body CreateEllipse(World world, GGame.Math.Fix64 xRadius, GGame.Math.Fix64 yRadius, int edges, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Body body = CreateBody(world, position, rotation, bodyType, userData);
            FixtureFactory.AttachEllipse(xRadius, yRadius, edges, density, body);
            return body;
        }

        public static Body CreatePolygon(World world, Vertices vertices, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Body body = CreateBody(world, position, rotation, bodyType, userData);
            FixtureFactory.AttachPolygon(vertices, density, body);
            return body;
        }

        public static Body CreateCompoundPolygon(World world, List<Vertices> list, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            //We create a single body
            Body body = CreateBody(world, position, rotation, bodyType, userData);
            FixtureFactory.AttachCompoundPolygon(list, density, body);
            return body;
        }

        public static Body CreateGear(World world, GGame.Math.Fix64 radius, int numberOfTeeth, GGame.Math.Fix64 tipPercentage, GGame.Math.Fix64 toothHeight, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Vertices gearPolygon = PolygonUtils.CreateGear(radius, numberOfTeeth, tipPercentage, toothHeight);

            //Gears can in some cases be convex
            if (!gearPolygon.IsConvex())
            {
                //Decompose the gear:
                List<Vertices> list = Triangulate.ConvexPartition(gearPolygon, TriangulationAlgorithm.Earclip, true, 0.001f);

                return CreateCompoundPolygon(world, list, density, position, rotation, bodyType, userData);
            }

            return CreatePolygon(world, gearPolygon, density, position, rotation, bodyType, userData);
        }

        public static Body CreateCapsule(World world, GGame.Math.Fix64 height, GGame.Math.Fix64 topRadius, int topEdges, GGame.Math.Fix64 bottomRadius, int bottomEdges, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Vertices verts = PolygonUtils.CreateCapsule(height, topRadius, topEdges, bottomRadius, bottomEdges);

            //There are too many vertices in the capsule. We decompose it.
            if (verts.Count >= Settings.MaxPolygonVertices)
            {
                List<Vertices> vertList = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Earclip, true, 0.001f);
                return CreateCompoundPolygon(world, vertList, density, position, rotation, bodyType, userData);
            }

            return CreatePolygon(world, verts, density, position, rotation, bodyType, userData);
        }

        public static Body CreateCapsule(World world, GGame.Math.Fix64 height, GGame.Math.Fix64 endRadius, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            //Create the middle rectangle
            Vertices rectangle = PolygonUtils.CreateRectangle(endRadius, height / 2);

            List<Vertices> list = new List<Vertices>();
            list.Add(rectangle);

            Body body = CreateCompoundPolygon(world, list, density, position, rotation, bodyType, userData);
            FixtureFactory.AttachCircle(endRadius, density, body, new Vector2(0, height / 2));
            FixtureFactory.AttachCircle(endRadius, density, body, new Vector2(0, -(height / 2)));

            //Create the two circles
            //CircleShape topCircle = new CircleShape(endRadius, density);
            //topCircle.Position = new Vector2(0, height / 2);
            //body.CreateFixture(topCircle);

            //CircleShape bottomCircle = new CircleShape(endRadius, density);
            //bottomCircle.Position = new Vector2(0, -(height / 2));
            //body.CreateFixture(bottomCircle);
            return body;
        }

        public static Body CreateRoundedRectangle(World world, GGame.Math.Fix64 width, GGame.Math.Fix64 height, GGame.Math.Fix64 xRadius, GGame.Math.Fix64 yRadius, int segments, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Vertices verts = PolygonUtils.CreateRoundedRectangle(width, height, xRadius, yRadius, segments);

            //There are too many vertices in the capsule. We decompose it.
            if (verts.Count >= Settings.MaxPolygonVertices)
            {
                List<Vertices> vertList = Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Earclip, true, 0.001f);
                return CreateCompoundPolygon(world, vertList, density, position, rotation, bodyType, userData);
            }

            return CreatePolygon(world, verts, density, position, rotation, bodyType, userData);
        }

        public static Body CreateLineArc(World world, GGame.Math.Fix64 radians, int sides, GGame.Math.Fix64 radius, bool closed = false, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Body body = CreateBody(world, position, rotation, bodyType, userData);
            FixtureFactory.AttachLineArc(radians, sides, radius, closed, body);
            return body;
        }

        public static Body CreateSolidArc(World world, GGame.Math.Fix64 density, GGame.Math.Fix64 radians, int sides, GGame.Math.Fix64 radius, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64(), BodyType bodyType = BodyType.Static, object userData = null)
        {
            Body body = CreateBody(world, position, rotation, bodyType, userData);
            FixtureFactory.AttachSolidArc(density, radians, sides, radius, body);

            return body;
        }

        public static BreakableBody CreateBreakableBody(World world, Vertices vertices, GGame.Math.Fix64 density, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64())
        {
            //TODO: Implement a Voronoi diagram algorithm to split up the vertices
            List<Vertices> triangles = Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Earclip, true, 0.001f);

            BreakableBody breakableBody = new BreakableBody(world, triangles, density, position, rotation);
            breakableBody.MainBody.Position = position;
            world.AddBreakableBody(breakableBody);
            return breakableBody;
        }

        public static BreakableBody CreateBreakableBody(World world, IEnumerable<Shape> shapes, Vector2 position = new Vector2(), GGame.Math.Fix64 rotation = new Fix64())
        {
            BreakableBody breakableBody = new BreakableBody(world, shapes, position, rotation);
            breakableBody.MainBody.Position = position;
            world.AddBreakableBody(breakableBody);
            return breakableBody;
        }

        public static Body CreateFromTemplate(World world, BodyTemplate bodyTemplate)
        {
            return world.CreateBody(bodyTemplate);
        }
    }
}