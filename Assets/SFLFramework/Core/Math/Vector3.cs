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
        
        public static Vector3 operator *(Vector3 x, Vector3 y)
        {
            return new Vector3(x.X * y.X, x.Y * y.Y, x.Z * y.Z);
        }
        
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            Vector3 ret;
            ret.X = v1.X + v2.X;
            ret.Y = v1.Y + v2.Y;
            ret.Z = v1.Z + v2.Z;

            return ret;
        }
        
        public static Vector3 Lerp(Vector3 value1, Vector3 value2, float amount)
        {
            Vector3 vector3;
            vector3.X = value1.X + (value2.X - value1.X) * amount;
            vector3.Y = value1.Y + (value2.Y - value1.Y) * amount;
            vector3.Z = value1.Z + (value2.Z - value1.Z) * amount;
            return vector3;
        }
    }
}