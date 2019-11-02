
using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;
using System.Collections.Generic;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    /// An essential geometrical object is a Plane. We represent a plane allways by a point and a normal vector,
    /// which is normalized.
    /// 
    /// </summary>
    [Serializable]
    public class Plane : ITransform
    {
        /// <summary>
        /// An operator, which transform a plane
        /// </summary>
        /// <param name="M">Matrix, which holds the transformation</param>
        /// <param name="P">the plane, which will be transformed</param>
        /// <returns></returns>
        public static Plane operator *(Matrix M, Plane P)
        {

            return new Plane(M * P.P, M * P.NormalUnit - M * new xyz(0, 0, 0));

        }
        /// <summary>
        /// P is a point which belongs to the plane
        /// </summary>
        public xyz P;
        /// <summary>
        /// NormalUnit is a vector, which is normal to the plane and has the length 1
        /// </summary>
        public xyz NormalUnit;
        /// <summary>
        /// The constructor of a plane takes the Point P and a NormalUnit
        /// </summary>
        /// <param name="P"></param>
        /// <param name="NormalUnit"></param>
        public Plane(xyz P, xyz NormalUnit)
        {
            this.P = P;
            this.NormalUnit = NormalUnit;

        }
        /// <summary>
        /// The plane will be initialized by three points, which are contained in the plane.
        /// </summary>
        /// <param name="A">First point</param>
        /// <param name="B">Second point</param>
        /// <param name="C">Third point</param>
        public Plane(xyz A, xyz B, xyz C)
        {
            P = A;
            NormalUnit = (B - A) & (C - A);
            NormalUnit = NormalUnit.normalized();

        }
        /// <summary>
        /// The plane will be initialized by three float points, which are contained in the plane.
        /// </summary>
        /// <param name="A">First point</param>
        /// <param name="B">Second point</param>
        /// <param name="C">Third point</param>
        public Plane(xyzf A, xyzf B, xyzf C)
        {
            P = A.Toxyz();
            xyz U = new xyz(B.x - A.X, B.y - A.y, B.z - A.z);
            xyz V = new xyz(C.x - A.X, C.y - A.y, C.z - A.z);
            NormalUnit = U & V;
            NormalUnit = NormalUnit.normalized();

        }
        /// <summary>
        /// Calculate the cross point with a line.
        /// </summary>
        /// <param name="aLine">A line which intersects the plane</param>
        /// <param name="Lam">Is a parameter, which determines the crosspoint</param>
        /// <param name="pt">Is the cross point</param>
        /// <returns>returns true, if there is a crosspoint.</returns>
        /// <example><code>
        /// //the following snippet shows the meaning of lam
        /// Plane p = new Plane( new xyz(0, 0, 0), new xyz(1, 5, 5));
        /// LineType l = new LineType( new xyz (3,-1,3), new xyz(2, 1, 5));
        /// if (p.Cross(l, out Lam, out pt))
        ///		{
        ///		// you can also get p through the following
        ///		pt = l.P + l.Direction*lam;
        ///		}
        /// </code></example>


        public bool Cross(LineType aLine, out double Lam, out xyz pt)
        {
            pt = aLine.P;
            Lam = -1;
         
            xyz Direction = aLine.Direction.normalized();
            double En = Direction.Scalarproduct(NormalUnit);
       
            if (System.Math.Abs(En) < Utils.epsilon) 
                return false;
            xyz OP = aLine.P;
            OP = OP.sub(P);
            
            Lam = -OP.Scalarproduct(NormalUnit) / En;
            pt = pt.add(Direction.mul(Lam));
            Lam /= (aLine.P - aLine.Q).length();

            return true;
        }

        /// <summary>
        /// Crosses the plane with an other plane and returns the crossline.
        /// </summary>
        /// <param name="aPlane">The other plane</param>
        /// <returns></returns>
        public LineType Cross(Plane aPlane)
        {
            LineType Result = new LineType();
            xyz Dir = NormalUnit & aPlane.NormalUnit;
            if (Dir.isZero()) return Result;
            xyz s = Dir & NormalUnit;
            double Lam = (aPlane.P - P) * aPlane.NormalUnit / (s * aPlane.NormalUnit);
            Result.P = P + s * Lam;
            Result.Direction = Dir;
            return Result;








        }

        #region ITransform Member
        /// <summary>
        /// Transforms the plane with a transformation matrix
        /// </summary>
        /// <param name="T"></param>
        public void Transform(Matrix T)
        {
            P = T * P;
            double l = NormalUnit.length();
            NormalUnit = T * NormalUnit - T * new xyz(0, 0, 0);
            l = NormalUnit.length();
        }

        #endregion
    }
}
