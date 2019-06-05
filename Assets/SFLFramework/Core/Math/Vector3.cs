using System;

namespace GGame
{
    public struct Vector3
    {
        public Fix64 X;
        public Fix64 Y;
        public Fix64 Z;

        public Vector3(Fix64 x, Fix64 y, Fix64 z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 Zero
        {
            get { return new Vector3(Fix64.Zero, Fix64.Zero, Fix64.Zero); }
        }
        
        public static Vector3 One
        {
            get { return new Vector3(Fix64.One, Fix64.One, Fix64.One); }
        }
        
        public static Vector3 operator *(Vector3 x, Fix64 y)
        {
            return new Vector3(x.X * y, x.Y * y, x.Z * y);
        }
        
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            Vector3 ret;
            ret.X = v1.X + v2.X;
            ret.Y = v1.Y + v2.Y;
            ret.Z = v1.Z + v2.Z;

            return ret;
        }
    }
}