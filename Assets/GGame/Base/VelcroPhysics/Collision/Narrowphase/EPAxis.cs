namespace VelcroPhysics.Collision.Narrowphase
{
    /// <summary>
    /// This structure is used to keep track of the best separating axis.
    /// </summary>
    public struct EPAxis
    {
        public int Index;
        public GGame.Math.Fix64 Separation;
        public EPAxisType Type;
    }
}