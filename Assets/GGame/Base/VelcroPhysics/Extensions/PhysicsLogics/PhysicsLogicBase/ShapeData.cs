using VelcroPhysics.Dynamics;

namespace VelcroPhysics.Extensions.PhysicsLogics.PhysicsLogicBase
{
    public struct ShapeData
    {
        public Body Body;
        public GGame.Math.Fix64 Max;
        public GGame.Math.Fix64 Min; // absolute angles
    }
}