
using System;
using System.ComponentModel;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    ///  xy is a foundation type for points in the 2D-Geometry
    ///  I think the name is nice and short.
    ///  He has two fields x and y which represent the x-Coordinate and the y-coordinate of the point.
    ///  </summary>
    [Serializable]
    [TypeConverter(typeof(xyEditor))]

    public struct xy
    {
        /// <summary>
        /// Multiply the point with a <see cref="Matrix"/> by lifting the z-value with z = 0;
        /// The result omits the z-value.
        /// </summary>
        /// <param name="m">Matrix of the multiplication</param>
        /// <param name="a">Point to be transformed</param>
        /// <returns>The transformed point</returns>
        public static xy operator *(Matrix3x3 m, xy a)
        {
            return a.mul(m);
        }
        ///// <summary>
        ///// Multiply the point with a <see cref="Matrix"/> by lifting the z-value with z = 0;
        ///// The result omit the z-value.
        ///// </summary>
        ///// <param name="m"></param>
        ///// <returns>The transformed point</returns>
        //public xy mul(Matrix3x3 m)
        //{
        //    return mul(m);
        //}
        /// <summary>
        /// Multiply the point with a matrix and return the result
        /// This matrix multiplication is often used in transformations.
        /// </summary>
        /// <param name="a">Transformations matrix, which will be multiplied</param>
        /// <returns>
        /// Transformed point
        /// </returns>
        /// <remarks>
        /// The Matrix a must be a 4 X 4 matrix. So the 3D-point will be "lifted" to a 4D-Point by setting the fourth coordinate to 1.
        /// After the well-known matrix multiplication the coordinates are divided by the result in the fourth coordinate.
        /// In this way, it is for example possible, to transform a point by a matrix, which represents a perspectively projection.
        /// </remarks>
        public xy mul(Matrix3x3 a)
        {
            xy p = new xy(0, 0);
            double r;
           
            {
                p.x = a.a00 * x + a.a01 * y + a.a02;
                p.y = a.a10 * x + a.a11 * y + a.a12 ;
                r = a.a20 * x + a.a21 * y + a.a22;
               
                if (!Utils.Equals(r, 0))
                {
                    p.x = p.x / r;
                    p.y = p.y / r;
                   
                }
            }
            return p;
        }
        /// <summary>
        /// Returns a vector, which has the length "Le"
        /// </summary>
        /// <param name="Le">The desired length</param>
        /// <returns></returns>
        public xy Trim(double Le)
        {
            return normalize() * Le;
        }
        /// <summary>
        /// the same as x
        /// </summary>
        public double X { get { return x; } set { x = value; } }
        /// <summary>
        /// the same as y
        /// </summary>
        public double Y { get { return y; } set { y = value; } }
        /// <summary>
        /// Lifts a xy-value to a xyz-value by setting z = 0;
        /// <seealso cref="xyz.toXY"/>
        /// </summary>
        public xyz toXYZ()
        {
            return new xyz(x, y, 0);
        }
        /// <summary>
        /// The (0,0)-vector
        /// </summary>
        public static xy Null = new xy(0, 0);
        /// <summary>
        /// Is true, if the vector is Null.
        /// </summary>
        /// <returns></returns>
        public bool isZero()
        {
            return (Utils.Equals(dist(new xy(0, 0)), 0));
        }
        /// <summary>
        /// Overrides the method GetHashCode
        /// </summary>
        /// <returns>HashCode</returns>
        public override int GetHashCode()
        {
            return (int)(x * 1000) * 1000 + (int)(y * 1000);
        }

        /// <summary>
        /// Calculates the cross product with a value;
        /// </summary>
        /// <param name="value">Value of the vector with which the cross product is calculated</param>
        /// <returns>Cross product</returns>
        public double cross(xy value)
        {
            return (x * value.y - y * value.x);
        }
        /// <summary>
        /// Operator for the cross product
        /// </summary>
        /// <param name="a">First parameter </param>/// 
        /// <param name="b">Second parameter</param>
        /// <returns></returns>
        public static double operator &(xy a, xy b)
        {
            return a.cross(b);
        }

        /// <summary>
        /// Multiply the vector by a scalar.
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="b">scalar</param>
        /// <returns>scalar multiplied vector</returns>
        public static xy operator *(xy a, double b)
        { return new xy(a.x * b, a.y * b); }
        /// <summary>
        /// Calculates the dotproduct of the vectors a and b.
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns>dotproduct</returns>


        public static double operator *(xy a, xy b)
        { return a.x * b.x + a.y * b.y; }
        /// <summary>
        /// Adds two vectors
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns></returns>
        public static xy operator +(xy a, xy b)
        { return new xy(a.x + b.x, a.y + b.y); }
        /// <summary>
        /// Subtracts two vectors
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns>Difference vector</returns>
        public static xy operator -(xy a, xy b)
        { return new xy(a.x - b.x, a.y - b.y); }
        /// <summary>
        /// Subtract a value and retrieve the difference vector
        /// </summary>
        /// <param name="value">Value which will be subtracted</param>
        /// <returns>Diference vector</returns>

        public xy sub(xy value)
        {
            return this - value;
        }
        /// <summary>
        /// Returns a vector of the length 1 with the same direction.
        /// </summary>
        /// <returns>Normalized vector</returns>
        public xy normalize()
        {
            double L = length();
            if (Utils.Equals(L, 0))
                return new xy(0, 0);

            return new xy(x / L, y / L);
        }
        /// <summary>
        /// Returns a normal vector which is rotated in a counterclockwise sense.
        /// </summary>
        /// <returns></returns>
        public xy normal()
        {
            return new xy(y, -x);
        }

        /// <summary>                                 
        /// Retrieves the length of the vector
        /// </summary>
        /// <returns>
        /// Returns the length of the vector
        /// </returns>
        public double length()
        {
            return System.Math.Sqrt(x * x + y * y);
        }
        /// <summary>
        /// Retrieves the distance to the point p
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>
        /// Retrieves the distance to the point p
        /// </returns>
        public double dist(xy p)
        {
            return sub(p).length();
        }

        /// <summary>
        /// x-coordinate
        /// </summary>

        public double x;
        /// <summary>
        /// y-coordinate
        /// </summary>
        public double y;
        /// <summary>
        /// Constructor with value x and y, which are the coordinates of the point
        /// </summary>
        /// <param name="x">x-coodinate</param>
        /// <param name="y">y-coodinate</param>
        public xy(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// gets the parameter lam and mue, so that Center+ a*lam + b*mue gives <b>this</b>
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>xy vaule consisting of lam and mue.</returns>
        public xy getAffin(xy Center, xy a, xy b)
        {
            if ((a.x * b.y - b.x * a.y) == 0) return new xy(1e10, 1e10);
            double Mue = ((x - Center.x) * (-a.y) + (y - Center.y) * (a.x)) / (a.x * b.y - b.x * a.y);
            double Lam = ((x - Center.x) * (-b.y) + (y - Center.y) * (b.x)) / (-a.x * b.y + b.x * a.y);
            return new xy(Lam, Mue);
        }
        /// <summary>
        /// Converts the xy-value to a string by using <see cref="Utils.Delimiter"/> and <see cref="Utils.DoubleFormat"/>
        /// </summary>
        /// <returns>The converted string</returns>
        public override string ToString()
        {

            return x.ToString(Utils.DoubleFormat) + Utils.Delimiter + y.ToString(Utils.DoubleFormat);

        }
        /// <summary>
        /// Checks parallelism to a vector.
        /// </summary>
        /// <param name="value">A vector</param>
        /// <returns>True, if this is parallel to value</returns>
        public bool Parallel(xy value)
        {
            return (Utils.Equals(x * value.y, y * value.x));
        }
        /// <summary>
        /// Gets a <see cref="xy"/>-value from a string by using <see cref="Utils.Delimiter"/>
        /// </summary>
        /// <param name="s">A string</param>
        /// <returns><see cref="xy"/>-value</returns>
        public static xy FromString(string s)
        {
            char[] d = { Utils.Delimiter };
            string[] n = s.Split(d, 2);
            if (n.Length >= 2)
                return new xy(Utils.StrToFloat(n[0]), Utils.StrToFloat(n[1]));
            if (n.Length >= 1)
                return new xy(Utils.StrToFloat(n[0]), 0);
            return new xy(0, 0);

        }
        /// <summary>
        /// In order to that it overrides the Equalsmethod and returns true if the differences of the x and y-values are smaller than <see cref="Utils.epsilon"/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is xy)
                return Utils.Equals(this, (xy)obj);

            return base.Equals(obj);
        }
    }
}
