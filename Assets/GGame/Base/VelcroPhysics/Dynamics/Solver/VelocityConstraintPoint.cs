using Microsoft.Xna.Framework;

namespace VelcroPhysics.Dynamics.Solver
{
    public sealed class VelocityConstraintPoint
    {
        public GGame.Math.Fix64 NormalImpulse;
        public GGame.Math.Fix64 NormalMass;
        public Vector2 rA;
        public Vector2 rB;
        public GGame.Math.Fix64 TangentImpulse;
        public GGame.Math.Fix64 TangentMass;
        public GGame.Math.Fix64 VelocityBias;
    }
}