using Microsoft.Xna.Framework;
using VelcroPhysics.Shared;

namespace VelcroPhysics.Dynamics.Solver
{
    public sealed class ContactVelocityConstraint
    {
        public int ContactIndex;
        public GGame.Math.Fix64 Friction;
        public int IndexA;
        public int IndexB;
        public GGame.Math.Fix64 InvIA, InvIB;
        public GGame.Math.Fix64 InvMassA, InvMassB;
        public Mat22 K;
        public Vector2 Normal;
        public Mat22 NormalMass;
        public int PointCount;
        public VelocityConstraintPoint[] Points = new VelocityConstraintPoint[Settings.MaxManifoldPoints];
        public GGame.Math.Fix64 Restitution;
        public GGame.Math.Fix64 TangentSpeed;

        public ContactVelocityConstraint()
        {
            for (int i = 0; i < Settings.MaxManifoldPoints; i++)
            {
                Points[i] = new VelocityConstraintPoint();
            }
        }
    }
}