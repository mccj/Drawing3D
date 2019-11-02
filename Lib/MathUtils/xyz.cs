
using System;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.


namespace Drawing3d
{
    /// <summary>
    ///  xyz is a foundation type for points in the 3D-world
    ///  I think the name is nice, short and handy.
    ///  He has three fields x, y and z, which represent the x-Coordinate, the y-coordinate and the z-coordinate respectively
    /// </summary>


    [Serializable]

    public struct xyz
    {
        /// <summary>
        /// Converts to <see cref="xyzf"/>
        /// </summary>
        /// <returns></returns>
        public xyzf toXYZF()
        {
            return new xyzf((float)x, (float)y, (float)z);
        }
        /// <summary>
        ///  if <b>this</b> is in the plane,spanned by C and the direction a, and b it will return
        ///  parameters Lam and Mue, so that <b>this</b> = C+a*Lam +b.Mue.
        /// </summary>
        /// <param name="C"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public xy getAffin(xyz C,xyz a, xyz b)
        {
            if ((a.x * b.y - b.x * a.y) != 0) return new xy(x, y).getAffin(new xy(C.x, C.y), new xy(a.x, a.y), new xy(b.x, b.y));
            if ((a.x * b.z - b.x * a.z) != 0) return new xy(x, z).getAffin(new xy(C.x, C.z), new xy(a.x, a.z), new xy(b.x, b.z));
            if ((a.y * b.z - b.y * a.z) != 0) return new xy(y, z).getAffin(new xy(C.y, C.z), new xy(a.y, a.z), new xy(b.y, b.z));
            return new xy(0, 0);
        }
        /// <summary>
        /// Calculates the dot product
        /// </summary>
        /// <param name="A">first point</param>
        /// <param name="B">second point</param>
        /// <param name="C">third point</param>
        /// <returns></returns>
        public static double dot(xyz A, xyz B, xyz C)
        {
            return A.x * B.y * C.z + B.x * C.y * A.z + C.x * A.y * B.z - A.x * B.z * C.y - B.x * C.z * A.y - C.x * A.z * B.y;
        }
        /// <summary>
        /// Constructor for xyz, who needs initial values for x, y and z
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        /// 

        public xyz(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;


        }
        /// <summary>
        /// Loads an xyz point from a reader
        /// </summary>
        /// <param name="S">binary reader</param>
        /// <returns></returns>
        public static xyz Load(System.IO.BinaryReader S)
        {
            return new xyz(S.ReadDouble(), S.ReadDouble(), S.ReadDouble());

        }
        /// <summary>
        /// Saves a xyz point to a writer
        /// </summary>
        /// <param name="W">Binary writer</param>
        /// <param name="Value">the point, which will be saved</param>
        public static void Save(System.IO.BinaryWriter W, xyz Value)
        {
            W.Write(Value.x);
            W.Write(Value.y);
            W.Write(Value.z);

        }
        /// <summary>
        /// overrides getHashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (int)(100 * x) * 10000 + (int)(100 * x) * 100 + (int)(100 * x);
        }
        /// <summary>
        ///  Gets the vecto (0,0,0)
        /// </summary>
        public static xyz Null = new xyz(0, 0, 0);
        /// <summary>
        /// Overrides the method ToString and returns a converted string, which uses the constant 
        /// <see cref="Utils.DoubleFormat">Utils.DoubleFormat</see>. The three numbers are separated by Utils. Delimiter
        /// </summary>
        /// <returns>
        /// Returns the coordinates converted to a string
        /// </returns>
        public override string ToString()
        {
            //  System.Windows.Forms.Application.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";
            return Utils.FloatToStr(x) + Utils.Delimiter + Utils.FloatToStr(y) + Utils.Delimiter + Utils.FloatToStr(z);
        }
        /// <summary>
        /// calculates the barycentric coordinates, which wil be returned.
        /// The point A*result.x +B*result.y+C*result.z is the normalprojection of P to
        /// the plane spanned by A,B and C. If A,B and C are in a line 0,0,0 wil be returned.
        /// </summary>
        /// <param name="A">Edge point of the triangle</param>
        /// <param name="B">Edge point of the triangle</param>
        /// <param name="C">Edge point of the triangle</param>
        /// <param name="P">Base point in the ABC plane </param>
        /// <returns></returns>
       public static xyz BaryCentric(xyz A, xyz B, xyz C, xyz P)
        {
            xyz F = (B - A) & (C - A);
            if (F.length() < 0.000000001) return new xyz(0, 0, 0);
            xyz N = F.normalized();
            double TrABC = F * N;
            double TrCAP = ((C - P) & (A - P)) * N;
            double TrABP = ((A - P) & (B - P)) * N;
            double TrBCP = ((B - P) & (C - P)) * N;
            return new xyz(TrBCP / TrABC, TrCAP / TrABC, TrABP / TrABC);
        }
        /// <summary>
        /// Gets a <see cref="xyz"/>-value from a string by using <see cref="Utils.Delimiter"/>
        /// </summary>
        /// <param name="s">A string</param>
        /// <returns><see cref="xyz"/>-value</returns>
        public static xyz FromString(string s)
        {

            // System.Windows.Forms.Application.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",";
            char[] d = { Utils.Delimiter };
            string[] n = s.Split(d, 3);
            double f = (Utils.StrToFloat(n[0]));








            if (n.Length >= 3)
                return new xyz(Utils.StrToFloat(n[0]), Utils.StrToFloat(n[1]), Utils.StrToFloat(n[2]));
            if (n.Length >= 2)
                return new xyz(Utils.StrToFloat(n[0]), Utils.StrToFloat(n[1]), 0);
            if (n.Length >= 1)
                return new xyz(Utils.StrToFloat(n[0]), 0, 0);
            return new xyz(0, 0, 0);

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
        /// the same as z
        /// </summary>
        public double Z { get { return z; } set { z = value; } }
        /// <summary>
        /// x-coordinate of the Point
        /// </summary>
        public double x;
        /// <summary>
        /// y-coordinate of the Point
        /// </summary>

        public double y;
        /// <summary>
        /// z-coordinate of the Point
        /// </summary>

        public double z;
        /// <summary>
        /// Checks, when two points are equal. This is the case, if their coordinates are equal in the sense of
        /// Utils.Equals.
        /// </summary>
        /// <param name="value">Point of type xyz to check the equality</param>
        /// <returns>True, if they are equals, otherwise false</returns>
        public override bool Equals(object value)
        {
            if (value is xyz)
            {
                xyz Q = (xyz)value;
                return ((Utils.Equals(this.x, Q.x)) && (Utils.Equals(this.y, Q.y)) && (Utils.Equals(this.z, Q.z)));
            }
            if (value is double[])
            {
                double[] Q = (double[])value;
                if (Q.Length < 3) return false;
                return ((Utils.Equals(this.x, Q[0])) && (Utils.Equals(this.y, Q[1])) && (Utils.Equals(this.z, Q[2])));
            }
            return false;
        }
        /// <summary>
        /// Overrides the operator for controling the equality of two vectors by using epsilon as tolerance
        /// 
        /// </summary>
        /// <param name="P">first vector for check</param>
        /// <param name="Q">second vector for check</param>
        /// <returns>true, if they are equals else false</returns>
        public static bool operator ==(xyz P, xyz Q)
        {
            return P.Equals(Q);
        }
        /// <summary>
        /// Overrides the operator != which indicates inequality
        /// </summary>
        /// <param name="P">first vector to check</param>
        /// <param name="Q">second vector to check</param>
        /// <returns>true, if they are not equals else false</returns>
        public static bool operator !=(xyz P, xyz Q)
        {
            return !P.Equals(Q);
        }
        /// <summary>
        /// Overrides the + operator and retrieves the usual sumvector
        /// </summary>
        /// <param name="a">first summand</param>
        /// <param name="b">second summand</param>
        /// <returns>Added vector of a and b</returns>

        public static xyz operator +(xyz a, xyz b)
        { return a.add(b); }
        /// <summary>
        /// Overrides the - operator and retrieves the usual differencevector
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns></returns>

        public static xyz operator -(xyz a, xyz b)
        { return a.sub(b); }
        /// <summary>
        /// Returns the dotproduct
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns>a.x * b.x + a.y * b.y + a.z * b.z </returns>
        public static double operator *(xyz a, xyz b)
        { return a.Scalarproduct(b); }
        /// <summary>
        /// Calculates the cross product of "a" and "b". 
        /// It obeys the screw-drive-rule, 
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Vector product</returns>
        public static xyz operator &(xyz a, xyz b)
        { return a.cross(b); }



        /// <summary>
        /// Multiply the vector by a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="b">scalar</param>
        /// <returns>scalar multiplied vector</returns>
        public static xyz operator *(xyz a, double b)
        { return a.mul(b); }
        /// <summary>
        /// Adds the vector p and retrieves the result
        /// </summary>
        /// <param name="p">A vector, which becomes added</param>
        /// <returns>The sumvector of this and p </returns>
        public xyz add(xyz p)
        {
            return new xyz(x + p.x, y + p.y, z + p.z);
        }
        /// <summary>
        /// Subtract the vector p and retrieves the result
        /// </summary>
        /// <param name="p">
        /// A vector, how becomes subtructed
        /// </param>
        /// <returns>
        /// the differencevector of this and p 
        /// </returns>
        public xyz sub(xyz p)
        {
            return new xyz(x - p.x, y - p.y, z - p.z);
        }
        /// <summary>
        /// Multiply the vector this by lam and returns this vector
        /// </summary>
        /// <param name="lam">with this value the vector will be multiplied</param>
        /// <returns>returns the multiplyed vector</returns>
        /// <remarks>
        /// The vector this doesn`t change
        /// </remarks>

        public xyz mul(double lam)
        {
            return new xyz(x * lam, y * lam, z * lam);
        }
        /// <summary>
        /// Rounds the coordinates to the destinated number of "Decimals"/> to
        /// </summary>
        /// <param name="Decimals">Number of decimals</param>
        /// <returns>the rounded point</returns>
        public xyz Round(int Decimals)
        {
            return new xyz(System.Math.Round(x, Decimals), System.Math.Round(y, Decimals), System.Math.Round(z, Decimals));
        }
        /// <summary>
        /// Gets true if the vector is (0,0,0).
        /// </summary>
        /// <returns></returns>
        public bool isZero()
        {
            return (Utils.Equals(dist(new xyz(0, 0, 0)), 0));
        }
        /// <summary>
        /// Calculates the dotarproduct with p and returns this value
        /// </summary>
        /// <param name="p">With this vector the dotarproduct will be calculated</param>
        /// <returns>
        /// the dotarproduct with p
        /// </returns>
        /// <remarks>
        /// The vector this doesn`t change
        /// </remarks>

        public double Scalarproduct(xyz p)
        {
            return x * p.x + y * p.y + z * p.z;
        }
        /// <summary>
        /// Retrieves the length of the vector
        /// </summary>
        /// <returns>
        /// Returns the length of the vector
        /// </returns>
        public double length()
        {

            return System.Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Normalizes the vector to the lenght 1 and returns this as resultvector.
        /// </summary>
        public xyz normalized()
        {
            double L = length();
            if (L < double.Epsilon) return this;
            return mul(1 / L);
        }

        /// <summary>
        /// Makes a vector with length Size and returns this
        /// </summary>
        /// <param name="Size"></param>
        /// <returns>
        /// returns a vector with length Size which has the same direction like this
        /// </returns>
        public xyz skip(double Size)
        {
            return normalized().mul(Size);
        }
        /// <summary>
        /// Calculates the vector product with p and returns this vector
        /// </summary>
        /// <param name="p">with this vector the vector product will be calculated</param>
        /// <returns>
        /// vector product with p
        /// </returns>

        public xyz cross(xyz p)
        {
            return new xyz(y * p.z - z * p.y, z * p.x - x * p.z, x * p.y - y * p.x);
        }
        /// <summary>
        /// Retrieves the distance to point p
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>
        /// Retrieves the distance to point p
        /// </returns>
        public double dist(xyz p)
        {
            return sub(p).length();
        }
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
        public xyz mul(Matrix a)
        {
            xyz p = new xyz(0, 0, 0);
            double r;
            if ((a.Cols == 4) && (a.Rows == 4))
            {
                p.x = a.a00 * x + a.a01 * y + a.a02 * z + a.a03;
                p.y = a.a10 * x + a.a11 * y + a.a12 * z + a.a13;
                p.z = a.a20 * x + a.a21 * y + a.a22 * z + a.a23;
                r = a.a30 * x + a.a31 * y + a.a32 * z + a.a33;
                if (!Utils.Equals(r, 0))
                {
                    p.x = p.x / r;
                    p.y = p.y / r;
                    p.z = p.z / r;
                }
            }
            return p;
        }


        /// <summary>
        /// The Method omits the third coordinate.
        /// <seealso cref="xy.toXYZ"/>
        ///
        /// </summary>

        public xy toXY()
        {
            return new xy(x, y);

        }
    }

}
