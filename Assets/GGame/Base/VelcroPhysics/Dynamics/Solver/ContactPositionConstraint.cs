using Microsoft.Xna.Framework;
using VelcroPhysics.Collision.Narrowphase;

namespace VelcroPhysics.Dynamics.Solver
{
    public sealed class ContactPositionConstraint
    {
        public int IndexA;
        public int IndexB;
        public GGame.Math.Fix64 InvIA, InvIB;
        public GGame.Math.Fix64 InvMassA, InvMassB;
        public Vector2 LocalCenterA, LocalCenterB;
        public Vector2 LocalNormal;
        public Vector2 LocalPoint;
        public Vector2[] LocalPoints = new Vector2[Settings.MaxManifoldPoints];
        public int PointCount;
        public GGame.Math.Fix64 RadiusA, RadiusB;
        public ManifoldType Type;
    }
}