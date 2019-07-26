/*
* Velcro Physics:
* Copyright (c) 2017 Ian Qvist
*/

using Microsoft.Xna.Framework;

namespace VelcroPhysics.Utilities
{
    /// <summary>
    /// Convert units between display and simulation units.
    /// </summary>
    public static class ConvertUnits
    {
        private static GGame.Math.Fix64 _displayUnitsToSimUnitsRatio = 100f;
        private static GGame.Math.Fix64 _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

        public static void SetDisplayUnitToSimUnitRatio(GGame.Math.Fix64 displayUnitsPerSimUnit)
        {
            _displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            _simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        public static GGame.Math.Fix64 ToDisplayUnits(GGame.Math.Fix64 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static GGame.Math.Fix64 ToDisplayUnits(int simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToDisplayUnits(Vector2 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static void ToDisplayUnits(ref Vector2 simUnits, out Vector2 displayUnits)
        {
            Vector2.Multiply(ref simUnits, _displayUnitsToSimUnitsRatio, out displayUnits);
        }

        public static Vector3 ToDisplayUnits(Vector3 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToDisplayUnits(GGame.Math.Fix64 x, GGame.Math.Fix64 y)
        {
            return new Vector2(x, y) * _displayUnitsToSimUnitsRatio;
        }

        public static void ToDisplayUnits(GGame.Math.Fix64 x, GGame.Math.Fix64 y, out Vector2 displayUnits)
        {
            displayUnits = Vector2.Zero;
            displayUnits.X = x * _displayUnitsToSimUnitsRatio;
            displayUnits.Y = y * _displayUnitsToSimUnitsRatio;
        }

        public static GGame.Math.Fix64 ToSimUnits(GGame.Math.Fix64 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }
        

        public static GGame.Math.Fix64 ToSimUnits(int displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static Vector2 ToSimUnits(Vector2 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static Vector3 ToSimUnits(Vector3 displayUnits)
        {
            return displayUnits * _simUnitsToDisplayUnitsRatio;
        }

        public static void ToSimUnits(ref Vector2 displayUnits, out Vector2 simUnits)
        {
            Vector2.Multiply(ref displayUnits, _simUnitsToDisplayUnitsRatio, out simUnits);
        }

        public static Vector2 ToSimUnits(GGame.Math.Fix64 x, GGame.Math.Fix64 y)
        {
            return new Vector2(x, y) * _simUnitsToDisplayUnitsRatio;
        }
        

        public static void ToSimUnits(GGame.Math.Fix64 x, GGame.Math.Fix64 y, out Vector2 simUnits)
        {
            simUnits = Vector2.Zero;
            simUnits.X = x * _simUnitsToDisplayUnitsRatio;
            simUnits.Y = y * _simUnitsToDisplayUnitsRatio;
        }
    }
}