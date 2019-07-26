using System.Collections.Generic;

namespace VelcroPhysics.Extensions.PhysicsLogics.Explosion
{
    /// <summary>
    /// This is a comparer used for
    /// detecting angle difference between rays
    /// </summary>
    internal class RayDataComparer : IComparer<GGame.Math.Fix64>
    {
        #region IComparer<GGame.Math.Fix64> Members

        int IComparer<GGame.Math.Fix64>.Compare(GGame.Math.Fix64 a, GGame.Math.Fix64 b)
        {
            GGame.Math.Fix64 diff = (a - b);
            if (diff > 0)
                return 1;
            if (diff < 0)
                return -1;
            return 0;
        }

        #endregion
    }
}