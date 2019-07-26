#if !XNA && !WINDOWS_PHONE && !XBOX && !ANDROID && !MONOGAME

#region License

/*
MIT License
Copyright � 2006 The Mono.Xna Team

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

#endregion License

using System;
using GGame.Math;

namespace Microsoft.Xna.Framework
{
    public static class MathHelper
    {
        public static GGame.Math.Fix64 E = (GGame.Math.Fix64)Math.E;
        public static GGame.Math.Fix64 Log10E = 0.4342945f;
        public static GGame.Math.Fix64 Log2E = 1.442695f;
        public static GGame.Math.Fix64 Pi = (GGame.Math.Fix64)Math.PI;
        public static GGame.Math.Fix64 PiOver2 = (GGame.Math.Fix64)(Math.PI / 2.0);
        public static GGame.Math.Fix64 PiOver4 = (GGame.Math.Fix64)(Math.PI / 4.0);
        public static GGame.Math.Fix64 TwoPi = (GGame.Math.Fix64)(Math.PI * 2.0);

        public static GGame.Math.Fix64 Barycentric(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2, GGame.Math.Fix64 value3, GGame.Math.Fix64 amount1, GGame.Math.Fix64 amount2)
        {
            return value1 + (value2 - value1) * amount1 + (value3 - value1) * amount2;
        }

        public static GGame.Math.Fix64 CatmullRom(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2, GGame.Math.Fix64 value3, GGame.Math.Fix64 value4, GGame.Math.Fix64 amount)
        {
            // Using formula from http://www.mvps.org/directx/articles/catmull/
            // Internally using GGame.Math.Fix64s not to lose precission
            GGame.Math.Fix64 amountSquared = amount * amount;
            GGame.Math.Fix64 amountCubed = amountSquared * amount;
            return (GGame.Math.Fix64)(0.5 * (2.0 * value2 +
                                 (value3 - value1) * amount +
                                 (2.0 * value1 - 5.0 * value2 + 4.0 * value3 - value4) * amountSquared +
                                 (3.0 * value2 - value1 - 3.0 * value3 + value4) * amountCubed));
        }

        public static GGame.Math.Fix64 Clamp(GGame.Math.Fix64 value, GGame.Math.Fix64 min, GGame.Math.Fix64 max)
        {
            // First we check to see if we're greater than the max
            value = (value > max) ? max : value;

            // Then we check to see if we're less than the min.
            value = (value < min) ? min : value;

            // There's no check to see if min > max.
            return value;
        }

        public static GGame.Math.Fix64 Distance(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2)
        {
            return Fix64.Abs(value1 - value2);
        }

        public static GGame.Math.Fix64 Hermite(GGame.Math.Fix64 value1, GGame.Math.Fix64 tangent1, GGame.Math.Fix64 value2, GGame.Math.Fix64 tangent2, GGame.Math.Fix64 amount)
        {
            // All transformed to GGame.Math.Fix64 not to lose precission
            // Otherwise, for high numbers of param:amount the result is NaN instead of Infinity
            GGame.Math.Fix64 v1 = value1, v2 = value2, t1 = tangent1, t2 = tangent2, s = amount, result;
            GGame.Math.Fix64 sCubed = s * s * s;
            GGame.Math.Fix64 sSquared = s * s;

            if (amount == 0f)
                result = value1;
            else if (amount == 1f)
                result = value2;
            else
                result = (2 * v1 - 2 * v2 + t2 + t1) * sCubed +
                         (3 * v2 - 3 * v1 - 2 * t1 - t2) * sSquared +
                         t1 * s +
                         v1;
            return (GGame.Math.Fix64)result;
        }

        public static GGame.Math.Fix64 Lerp(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2, GGame.Math.Fix64 amount)
        {
            return value1 + (value2 - value1) * amount;
        }

        public static GGame.Math.Fix64 Max(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2)
        {
            return Math.Max((float)value1, (float)value2);
        }

        public static GGame.Math.Fix64 Min(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2)
        {
            return Math.Min((float)value1, (float)value2);
        }

        public static GGame.Math.Fix64 SmoothStep(GGame.Math.Fix64 value1, GGame.Math.Fix64 value2, GGame.Math.Fix64 amount)
        {
            // It is expected that 0 < amount < 1
            // If amount < 0, return value1
            // If amount > 1, return value2
            GGame.Math.Fix64 result = Clamp(amount, 0f, 1f);
            result = Hermite(value1, 0f, value2, 0f, result);
            return result;
        }

        public static GGame.Math.Fix64 ToDegrees(GGame.Math.Fix64 radians)
        {
            // This method uses GGame.Math.Fix64 precission internally,
            // though it returns single GGame.Math.Fix64
            // Factor = 180 / pi
            return (GGame.Math.Fix64)(radians * 57.295779513082320876798154814105);
        }

        public static GGame.Math.Fix64 ToRadians(GGame.Math.Fix64 degrees)
        {
            // This method uses GGame.Math.Fix64 precission internally,
            // though it returns single GGame.Math.Fix64
            // Factor = pi / 180
            return (GGame.Math.Fix64)(degrees * 0.017453292519943295769236907684886);
        }

        public static GGame.Math.Fix64 WrapAngle(GGame.Math.Fix64 angle)
        {
            angle = (GGame.Math.Fix64)Math.IEEERemainder((float)angle, 6.2831854820251465); //2xPi precission is GGame.Math.Fix64
            if (angle <= -3.141593f)
            {
                angle += 6.283185f;
                return angle;
            }
            if (angle > 3.141593f)
            {
                angle -= 6.283185f;
            }
            return angle;
        }
    }
}

#endif