using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;


using System.Collections.Generic;

namespace Drawing3d
{
    /// <summary>
    /// An internal structure, which holds two floating points
    /// </summary>
    internal struct EdgeCoordsf
    {
        /// <summary>
        /// Constructor with two Points
        /// </summary>
        /// <param name="A">first point</param>
        /// <param name="B">second point</param>
        public EdgeCoordsf(xyzf A, xyzf B)
        {
            this.A = new xyz(Math.Round(A.x, 6), Math.Round(A.y, 6), Math.Round(A.z, 6)).toXYZF();
            this.B = new xyz(Math.Round(B.x, 6), Math.Round(B.y, 6), Math.Round(B.z, 6)).toXYZF();

        }
        /// <summary>
        /// first Point
        /// </summary>
        public xyzf A;
        /// <summary>
        /// second Point
        /// </summary>
        public xyzf B;
        /// <summary>
        /// overrides equals, which returns true, if obj== this.
        /// </summary>
        /// <param name="obj">an other object</param>
        /// <returns>returns true, if obj== this</returns>
        public override bool Equals(object obj)
        {
            return (Equals(((EdgeCoordsf)obj).A, A) && Equals(((EdgeCoordsf)obj).B, B));
        }
        /// <summary>
        /// overrides GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
