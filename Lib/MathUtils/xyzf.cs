
using System;

namespace Drawing3d
{
    /// <summary>
    /// a point, which holds x,y and z coordinates as float.
    /// We call such a point "Float Point"
    /// See <see cref="xyz"/>
    /// </summary>
    [Serializable]
    public partial struct xyzf
    {

        /// <summary>
        /// x coordinate as float 
        /// </summary>
        public float x;
        /// <summary>
        /// y coordinate as float 
        /// </summary>
        public float y;
        /// <summary>
        /// x coordinate as float 
        /// </summary>
        public float z;
        /// <summary>
        /// Overrides <see cref="Equals(object)"/>
        /// </summary>
        /// <param name="obj">an other point</param>
        /// <returns>If obj is a xyzf type and is equal then it returns true else false.</returns>
        public override bool Equals(object obj)
        {
            return (((xyzf)obj).dist(this) < 0.005);

        }
        /// <summary>
        /// overrides GetHashCode.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Constructor for xyz, who needs initial values for x, y and z
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <param name="z">z-coordinate</param>
        /// 
        public xyzf(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// Convert a float point in a double point
        /// </summary>
        /// <returns></returns>
        public xyz Toxyz()
        {
            return new xyz(x, y, z);
        }
        /// <summary>
        /// calulates the distance to the point P.
        /// </summary>
        /// <param name="P">Point, whose distance will be calculated.</param>
        /// <returns>the distance.</returns>
        public double Dist(xyzf P)
        {
            return (P.x - x) * (P.x - x) + (P.y - y) * (P.y - y) + (P.z - z) * (P.z - z);
        }
        /// <summary>
        /// Converts a <see cref="xyzArray"/> to an array of float points.
        /// </summary>
        /// <param name="src">xyzarray, which will be convrted.</param>
        /// <returns></returns>
        public static xyzf[] ToArray(xyzArray src)
        {

            xyzf[] Result = new xyzf[src.Count];

            for (int i = 0; i < src.Count; i++)
            {
                Result[i] = new xyzf((float)src[i].x, (float)src[i].y, (float)src[i].z);
            }
            return Result;
        }
        /// <summary>
        /// Convert a xyzArray to an array of float points.
        /// </summary>
        /// <param name="src">Array of float points</param>
        /// <returns></returns>
        public static xyzf[] ToArray(xyz[] src)
        {

            xyzf[] Result = new xyzf[src.Length];

            for (int i = 0; i < src.Length; i++)
            {
                Result[i] = new xyzf((float)src[i].x, (float)src[i].y, (float)src[i].z);
            }
            return Result;
        }
        /// <summary>
        /// Calculates the dot product of three float points
        /// </summary>
        /// <param name="A">first point</param>
        /// <param name="B">second point</param>
        /// <param name="C">third point</param>
        /// <returns></returns>
        public static float dot(xyzf A, xyzf B, xyzf C)
        {
            return A.x * B.y * C.z + B.x * C.y * A.z + C.x * A.y * B.z - A.x * B.z * C.y - B.x * C.z * A.y - C.x * A.z * B.y;
        }




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
        /// Gets a <see cref="xyz"/>-value from a string by using <see cref="Utils.Delimiter"/>
        /// </summary>
        /// <param name="s">A string</param>
        /// <returns><see cref="xyz"/>-value</returns>
        public static xyzf FromString(string s)
        {


            char[] d = { Utils.Delimiter };
            string[] n = s.Split(d, 3);
            if (n.Length >= 3)
                return new xyzf((float)Utils.StrToFloat(n[0]), (float)Utils.StrToFloat(n[1]), (float)Utils.StrToFloat(n[2]));
            if (n.Length >= 2)
                return new xyzf((float)Utils.StrToFloat(n[0]), (float)Utils.StrToFloat(n[1]), 0);
            if (n.Length >= 1)
                return new xyzf((float)Utils.StrToFloat(n[0]), 0, 0);
            return new xyzf(0f, 0f, 0f);

        }
        /// <summary>
        /// the same as <see cref="x"/>
        /// </summary>
        public float X { get { return x; } set { x = value; } }
        /// <summary>
        /// the same as <see cref="y"/>
        /// </summary>
        public float Y { get { return y; } set { y = value; } }
        /// <summary>
        /// the same as <see cref="z"/>
        /// </summary>
        public float Z { get { return z; } set { z = value; } }

        /// <summary>
        /// Overrides the operator for controling the equality of two vectors by using epsilon as tolerance
        /// 
        /// </summary>
        /// <param name="a">first vector for check</param>
        /// <param name="b">second vector for check</param>
        /// <returns>true, if they are equals else false</returns>
        public static xyzf operator +(xyzf a, xyzf b)
        { return a.add(b); }
        /// <summary>
        /// Overrides the - operator and retrieves the usual differencevector
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns></returns>

        public static xyzf operator -(xyzf a, xyzf b)
        { return a.sub(b); }
        /// <summary>
        /// Returns the dotproduct
        /// </summary>
        /// <param name="a">first vector</param>
        /// <param name="b">second vector</param>
        /// <returns>a.x * b.x + a.y * b.y + a.z * b.z </returns>
        public static double operator *(xyzf a, xyzf b)
        { return a.Scalarproduct(b); }
        /// <summary>
        /// Calculates the cross product of "a" and "b". 
        /// It obeys the screw-drive-rule, 
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Vector product</returns>
        public static xyzf operator &(xyzf a, xyzf b)
        { return a.cross(b); }



        /// <summary>
        /// Multiply the vector by a scalar
        /// </summary>
        /// <param name="a">vector</param>
        /// <param name="b">scalar</param>
        /// <returns>scalar multiplied vector</returns>
        public static xyzf operator *(xyzf a, float b)
        { return a.mul(b); }
        /// <summary>
        /// Adds the vector p and retrieves the result
        /// </summary>
        /// <param name="p">A vector, which becomes added</param>
        /// <returns>The sumvector of this and p </returns>
        public xyzf add(xyzf p)
        {
            return new xyzf(x + p.x, y + p.y, z + p.z);
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
        public xyzf sub(xyzf p)
        {
            return new xyzf(x - p.x, y - p.y, z - p.z);
        }
        /// <summary>
        /// Multiply the vector this by lam and returns this vector
        /// </summary>
        /// <param name="lam">with this value the vector will be multiplied</param>
        /// <returns>returns the multiplyed vector</returns>
        /// <remarks>
        /// The vector this doesn`t change
        /// </remarks>

        public xyzf mul(float lam)
        {
            return new xyzf(x * lam, y * lam, z * lam);
        }



        /// <summary>
        /// Calculates the scalar product with p and returns this value
        /// </summary>
        /// <param name="p">With this vector the dotarproduct will be calculated</param>
        /// <returns>
        /// the dotarproduct with p
        /// </returns>
        /// <remarks>
        /// The vector this doesn`t change
        /// </remarks>

        public double Scalarproduct(xyzf p)
        {
            return x * p.x + y * p.y + z * p.z;
        }
        /// <summary>
        /// Retrieves the length of the vector
        /// </summary>
        /// <returns>
        /// Returns the length of the vector
        /// </returns>
        public float length()
        {
            return (float)System.Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Normalizes the vector to the lenght 1 and returns this as resultvector.
        /// </summary>
        public xyzf normalized()
        {
            if (length() < double.Epsilon)
                return this;
            return mul(1f / length());
        }

        /// <summary>
        /// Makes a vector with length Size and returns this
        /// </summary>
        /// <param name="Size"></param>
        /// <returns>
        /// returns a vector with length Size which has the same direction like this
        /// </returns>
        public xyzf skip(float Size)
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

        public xyzf cross(xyzf p)
        {
            return new xyzf(y * p.z - z * p.y, z * p.x - x * p.z, x * p.y - y * p.x);
        }
        /// <summary>
        /// Retrieves the distance to point p
        /// </summary>
        /// <param name="p">A point</param>
        /// <returns>
        /// Retrieves the distance to point p
        /// </returns>
        public double dist(xyzf p)
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
        public xyzf mul(Matrix a)
        {
            xyzf p = new xyzf(0, 0, 0);
            double r;
            if ((a.Cols == 4) && (a.Rows == 4))
            {
                p.x = (float)(a.a00 * x + a.a01 * y + a.a02 * z + a.a03);
                p.y = (float)(a.a10 * x + a.a11 * y + a.a12 * z + a.a13);
                p.z = (float)(a.a20 * x + a.a21 * y + a.a22 * z + a.a23);
                r = (float)(a.a30 * x + a.a31 * y + a.a32 * z + a.a33);
                if (!Utils.Equals(r, 0))
                {
                    p.x = (float)(p.x / r);
                    p.y = (float)(p.y / r);
                    p.z = (float)(p.z / r);
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
