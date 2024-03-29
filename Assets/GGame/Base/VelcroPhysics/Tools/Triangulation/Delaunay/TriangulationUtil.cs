﻿/* Poly2Tri
 * Copyright (c) 2009-2010, Poly2Tri Contributors
 * http://code.google.com/p/poly2tri/
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * * Neither the name of Poly2Tri nor the names of its contributors may be
 *   used to endorse or promote products derived from this software without specific
 *   prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace VelcroPhysics.Tools.Triangulation.Delaunay
{
    /**
     * @author Thomas Åhlén, thahlen@gmail.com
     */

    internal class TriangulationUtil
    {
        public static GGame.Math.Fix64 EPSILON = 1e-12f;

        /// <summary>
        /// Requirements:
        /// 1. a,b and c form a triangle.
        /// 2. a and d is know to be on opposite side of bc
        /// <code>
        ///                a
        ///                +
        ///               / \
        ///              /   \
        ///            b/     \c
        ///            +-------+ 
        ///           /    B    \  
        ///          /           \ 
        /// </code>
        /// Facts:
        /// d has to be in area B to have a chance to be inside the circle formed by a,b and c
        /// d is outside B if orient2d(a,b,d) or orient2d(c,a,d) is CW
        /// This preknowledge gives us a way to optimize the incircle test
        /// </summary>
        /// <param name="pa">triangle point, opposite d</param>
        /// <param name="pb">triangle point</param>
        /// <param name="pc">triangle point</param>
        /// <param name="pd">point opposite a</param>
        /// <returns>true if d is inside circle, false if on circle edge</returns>
        public static bool SmartIncircle(TriangulationPoint pa, TriangulationPoint pb, TriangulationPoint pc,
                                         TriangulationPoint pd)
        {
            GGame.Math.Fix64 pdx = pd.X;
            GGame.Math.Fix64 pdy = pd.Y;
            GGame.Math.Fix64 adx = pa.X - pdx;
            GGame.Math.Fix64 ady = pa.Y - pdy;
            GGame.Math.Fix64 bdx = pb.X - pdx;
            GGame.Math.Fix64 bdy = pb.Y - pdy;

            GGame.Math.Fix64 adxbdy = adx * bdy;
            GGame.Math.Fix64 bdxady = bdx * ady;
            GGame.Math.Fix64 oabd = adxbdy - bdxady;

            //        oabd = orient2d(pa,pb,pd);
            if (oabd <= 0)
                return false;

            GGame.Math.Fix64 cdx = pc.X - pdx;
            GGame.Math.Fix64 cdy = pc.Y - pdy;

            GGame.Math.Fix64 cdxady = cdx * ady;
            GGame.Math.Fix64 adxcdy = adx * cdy;
            GGame.Math.Fix64 ocad = cdxady - adxcdy;

            //      ocad = orient2d(pc,pa,pd);
            if (ocad <= 0)
                return false;

            GGame.Math.Fix64 bdxcdy = bdx * cdy;
            GGame.Math.Fix64 cdxbdy = cdx * bdy;

            GGame.Math.Fix64 alift = adx * adx + ady * ady;
            GGame.Math.Fix64 blift = bdx * bdx + bdy * bdy;
            GGame.Math.Fix64 clift = cdx * cdx + cdy * cdy;

            GGame.Math.Fix64 det = alift * (bdxcdy - cdxbdy) + blift * ocad + clift * oabd;

            return det > 0;
        }
        /*
        public static bool InScanArea(TriangulationPoint pa, TriangulationPoint pb, TriangulationPoint pc,
                                      TriangulationPoint pd)
        {
            GGame.Math.Fix64 pdx = pd.X;
            GGame.Math.Fix64 pdy = pd.Y;
            GGame.Math.Fix64 adx = pa.X - pdx;
            GGame.Math.Fix64 ady = pa.Y - pdy;
            GGame.Math.Fix64 bdx = pb.X - pdx;
            GGame.Math.Fix64 bdy = pb.Y - pdy;

            GGame.Math.Fix64 adxbdy = adx*bdy;
            GGame.Math.Fix64 bdxady = bdx*ady;
            GGame.Math.Fix64 oabd = adxbdy - bdxady;
            //        oabd = orient2d(pa,pb,pd);
            if (oabd <= 0)
            {
                return false;
            }

            GGame.Math.Fix64 cdx = pc.X - pdx;
            GGame.Math.Fix64 cdy = pc.Y - pdy;

            GGame.Math.Fix64 cdxady = cdx*ady;
            GGame.Math.Fix64 adxcdy = adx*cdy;
            GGame.Math.Fix64 ocad = cdxady - adxcdy;
            //      ocad = orient2d(pc,pa,pd);
            if (ocad <= 0)
            {
                return false;
            }
            return true;
        }
        */

        public static bool InScanArea(TriangulationPoint pa, TriangulationPoint pb, TriangulationPoint pc, TriangulationPoint pd)
        {
            GGame.Math.Fix64 oadb = (pa.X - pb.X) * (pd.Y - pb.Y) - (pd.X - pb.X) * (pa.Y - pb.Y);
            if (oadb >= -EPSILON)
            {
                return false;
            }

            GGame.Math.Fix64 oadc = (pa.X - pc.X) * (pd.Y - pc.Y) - (pd.X - pc.X) * (pa.Y - pc.Y);
            if (oadc <= EPSILON)
            {
                return false;
            }
            return true;
        }

        /// Forumla to calculate signed area
        /// Positive if CCW
        /// Negative if CW
        /// 0 if collinear
        /// A[P1,P2,P3]  =  (x1*y2 - y1*x2) + (x2*y3 - y2*x3) + (x3*y1 - y3*x1)
        /// =  (x1-x3)*(y2-y3) - (y1-y3)*(x2-x3)
        public static Orientation Orient2d(TriangulationPoint pa, TriangulationPoint pb, TriangulationPoint pc)
        {
            GGame.Math.Fix64 detleft = (pa.X - pc.X) * (pb.Y - pc.Y);
            GGame.Math.Fix64 detright = (pa.Y - pc.Y) * (pb.X - pc.X);
            GGame.Math.Fix64 val = detleft - detright;
            if (val > -EPSILON && val < EPSILON)
            {
                return Orientation.Collinear;
            }
            if (val > 0)
            {
                return Orientation.CCW;
            }
            return Orientation.CW;
        }
    }
}