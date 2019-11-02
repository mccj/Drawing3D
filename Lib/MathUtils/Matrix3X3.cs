using System;
using System.ComponentModel;
using System.Collections;

namespace Drawing3d
{
    /// <summary>
    /// Matrix is the fundamental class for all 2d-transformations. 
    /// So a Matrix has 3 Rows and 3 Cols.
    /// </summary>
    [DefaultValue(new double[] { 1, 0, 0,
                                 0, 1, 0,
                                 0, 0, 1 })]
    [Serializable]
    public struct Matrix3x3
    {


        /// <summary>
        /// Converts a string to a 3 x 3- matrix, where valid delimiter are ; , and ' '.
        /// For example:
        /// <code>
        /// 1    2.4   3 
        /// 2.3  4.4   2 
        /// 1    2.4   3 
        ///  
        /// </code>
        /// </summary>
        /// <param name="value">String which describes the matrix</param>
        /// See <see cref="ToString"/>
        /// <returns>A matrix</returns>
        public static Matrix3x3 FromString(string value)
        {

            Matrix3x3 result = new Matrix3x3(1);

            string[] s = value.Split(new char[4] { ';', '[', ']', ' ' });

            int k = 0;
            ArrayList a = new ArrayList();
            for (k = 0; k < s.Length; k++)
                if (s[k].Trim() != "") a.Add(s[k]);
            if (a.Count != 9) return result;

            result.a00 = (float)Utils.StrToFloat((string)a[0]);
            result.a01 = (float)Utils.StrToFloat((string)a[1]);
            result.a02 = (float)Utils.StrToFloat((string)a[2]);


            result.a10 = (float)Utils.StrToFloat((string)a[3]);
            result.a11 = (float)Utils.StrToFloat((string)a[4]);
            result.a12 = (float)Utils.StrToFloat((string)a[5]);

            result.a20 = (float)Utils.StrToFloat((string)a[6]);
            result.a21 = (float)Utils.StrToFloat((string)a[7]);
            result.a22 = (float)Utils.StrToFloat((string)a[8]);


            return result;
        }


        /// <summary>
        /// Gets the i-th row
        /// </summary>
        /// <param name="i">Index of the row.</param>
        /// <returns>The i-th row</returns>
        public double[] GetRow(int i)
        {

            if (i == 0) return new double[] { a00, a01, a02 };
            if (i == 1) return new double[] { a10, a11, a12 };
            if (i == 2) return new double[] { a20, a21, a22 };

            return new double[] { 0, 0, 0 };

        }
        /// <summary>
        /// Gives the cols of the matrix
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double[] GetCol(int i)
        {

            if (i == 0) return new double[] { a00, a10, a20 };
            if (i == 1) return new double[] { a01, a11, a21 };
            if (i == 2) return new double[] { a02, a12, a22 };

            return new double[] { 0, 0, 0 };

        }
        /// <summary>
        /// Converts the matrix to an array of doubles
        /// </summary>
        /// <returns>double array which contains the elements of the matrix</returns>
        public double[] toArray()
        {
            return new double[] { a00, a10, a20, a01, a11, a21, a02, a12, a22 };
        }


        /// <summary>
        /// gets s matrix from the values of the array a.
        /// e.g:a00, a10, a20, a01, a11, a21,  a02, a12, a22
        /// </summary>
        /// <param name="a">the array</param>
        public static Matrix3x3 fromArray(double[] a)
        {

            Matrix3x3 Result = Matrix3x3.identity;

            Result.a00 = (float)a[0];
            Result.a10 = (float)a[1];
            Result.a20 = (float)a[2];

            Result.a01 = (float)a[3];
            Result.a11 = (float)a[4];
            Result.a21 = (float)a[5];

            Result.a02 = (float)a[6];
            Result.a12 = (float)a[7];
            Result.a22 = (float)a[8];

            return Result;

        }
        /// <summary>
        /// Multiply the 2D-point with the matrix but omit the third component.
        /// This multiplication gives the zero point by a multiplication of a zero point.
        /// You can use it to transform vectors.
        /// </summary>
        /// <param name="a">a point, which will be multiplied</param>
        /// <returns>
        /// Transformed point
        /// </returns>
        public xy multaffin(xy a)
        {
            xy p = new xy(0, 0);


            {
                p.x = a00 * a.x + a01 * a.y;
                p.y = a10 * a.x + a11 * a.y;
               


            }
            return p;
        }
        /// <summary>
        /// Multiply the 2D-point <see cref="xyf"/> with the matrix but omit the third component.
        /// This multiplication gives the zero point by a multiplication of a zero point.
        /// You can use it to transform vectors.
        /// </summary>
        /// <param name="a">a <see cref="xyf"/>point, which will be multiplied</param>
        /// <returns>
        /// Transformed point
        /// </returns>
        public xyf multaffin(xyf a)
        {
            xyf p = new xyf(0,0);


            {
                p.x = a00 * a.x + a01 * a.y;
                p.y = a10 * a.x + a11 * a.y;
            


            }
            return p;
        }
        /// <summary>
        /// Multiplicates with a Matrix M
        /// </summary>
        /// <param name="M">Matrix, which will be multiplicated</param>
        /// <returns></returns>
        public Matrix3x3 mul(Matrix3x3 M)
        {

            Matrix3x3 Result;

            Result.a00 = a00 * M.a00 + a01 * M.a10 + a02 * M.a20;
            Result.a01 = a00 * M.a01 + a01 * M.a11 + a02 * M.a21;
            Result.a02 = a00 * M.a02 + a01 * M.a12 + a02 * M.a22;


            Result.a10 = a10 * M.a00 + a11 * M.a10 + a12 * M.a20;
            Result.a11 = a10 * M.a01 + a11 * M.a11 + a12 * M.a21;
            Result.a12 = a10 * M.a02 + a11 * M.a12 + a12 * M.a22;


            Result.a20 = a20 * M.a00 + a21 * M.a10 + a22 * M.a20;
            Result.a21 = a20 * M.a01 + a21 * M.a11 + a22 * M.a21;
            Result.a22 = a20 * M.a02 + a21 * M.a12 + a22 * M.a22;

            return Result;

        }
        /// <summary>
        /// Calculates the difference to the Matrix M
        /// </summary>
        /// <param name="M">The matrix to which the difference is calculated</param>
        /// <returns>Difference of the matrices</returns>
        public Matrix3x3 sub(Matrix M)
        {

            Matrix3x3 Result; ;

            Result.a00 = a00 - M.a00;
            Result.a01 = a01 - M.a01;
            Result.a02 = a02 - M.a02;


            Result.a10 = a10 - M.a10;
            Result.a11 = a11 - M.a11;
            Result.a12 = a12 - M.a12;
            Result.a20 = a20 - M.a20;
            Result.a21 = a21 - M.a21;
            Result.a22 = 1;
            return Result;

        }
        /// <summary>
        /// Gets the determinant of the matrix
        /// </summary>
        /// <returns>The determinant of a the matrix</returns>
        public double Determinant()
        {

            return

             a00 * a11 * a22 + a01 * a12 * a20 + a02 * a10 * a21 - a00 * a12 * a21 - a01 * a10 * a22 - a02 * a11 * a20;

        }
        /// <summary>
        /// Inverts the Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix3x3 invert()
        {
            Matrix3x3 M = Matrix3x3.identity;

            float d = (float)Determinant();
            if (d == 0) return M;
            M.a00 = (a11 * a22 - a12 * a21) / d;
            M.a10 = -(+a10 * a22 - a12 * a20) / d;
            M.a20 = (+a10 * a21 - a11 * a20) / d;


            M.a01 = -(+a22 * a01 - a21 * a02) / d;
            M.a11 = (a22 * a00 - a20 * a02) / d;
            M.a21 = -(+a21 * a00 - a20 * a01) / d;

            M.a02 = (a01 * a12 - a02 * a11) / d;
            M.a12 = -(+a00 * a12 - a02 * a10) / d;
            M.a22 = (a00 * a11 - a01 * a10) / d;
            return M;

        }
        /// <summary>
        /// A[0,0]
        /// </summary>
        public float
            a00;
        /// <summary>
        /// A[1,0]
        /// </summary>
        public float
            a10;

        /// <summary>
        /// A[2,0]
        /// </summary>
        public float
            a20;



        /// <summary>
        /// A[0,1]
        /// </summary>
        public float
            a01;


        /// <summary>
        /// A[1,1]
        /// </summary>
        public float
            a11;

        /// <summary>
        /// A[2,1]
        /// </summary>
        public float
            a21;





        /// <summary>
        /// A[0,2]
        /// </summary>
        public float
            a02;


        /// <summary>
        /// A[1,2]
        /// </summary>
        public float
            a12;

        /// <summary>
        /// A[2,2]
        /// </summary>
        public float
            a22;
        /// <summary>
        /// Converts a matrix in a string by using "[" and "]" for every row and ";" inside the row as delimiter
        /// 
        /// </summary>
        /// <example>
        /// [[3.4;1.2;4.3];[2.8;6.4;3.6];[2.4;6.8;5.7]]
        /// <see cref="FromString(string)"/>
        /// </example>
        /// <returns></returns>
        public override string ToString()
        {
            string s = Utils.DoubleFormat;
          
            string Result =
                Utils.FloatToStr(a00) + " " + Utils.FloatToStr(a01) + " " + Utils.FloatToStr(a02) + ";"
               + Utils.FloatToStr(a10) + " " + Utils.FloatToStr(a11) + " " + Utils.FloatToStr(a12) + ";"
               + Utils.FloatToStr(a20) + " " + Utils.FloatToStr(a21) + " " + Utils.FloatToStr(a22);

          
            return Result;
        }

        /// <summary>
        /// This operator calculates the usual matrix multiplication a * b
        /// The colcount of a must be equal the rowcount of b.
        /// </summary>
        /// <param name="a"></param>
        /// a is the first matrix
        /// <param name="b"></param>
        /// b is the second matrix
        /// <returns></returns>
        public static Matrix3x3 operator *(Matrix3x3 a, Matrix3x3 b)
        { return a.mul(b); }
        //public static Matrix operator -(Matrix a, Matrix b)
        //{ return a.sub(b); }
        /// <summary>
        /// overrides GetHashCode()
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Converts the matrix to an array of float
        /// </summary>
        /// <returns>float array which contains the elements of the matrix</returns>

        public float[] ToFloat()
        {

            float[] F = new float[9];


            F[0 + 0 * 3] = (float)a00;
            F[1 + 0 * 3] = (float)a10;
            F[2 + 0 * 3] = (float)a20;


            F[0 + 1 * 3] = (float)a01;
            F[1 + 1 * 3] = (float)a11;
            F[2 + 1 * 3] = (float)a21;


            F[0 + 2 * 3] = (float)a02;
            F[1 + 2 * 3] = (float)a12;
            F[2 + 2 * 3] = (float)(a22);


            return F;
        }
     

        


        /// <summary>
        /// This operator returns the multiplication of a two-dimensional floatvector with the matrix3x3.
         /// </summary>
        /// <param name="a">3X3 Matrix</param>
        /// <param name="b">2D-floatvector</param>
        /// <returns>2D-floatvector</returns>


        public static xyf operator *(Matrix3x3 a, xyf b)
        {

            float x = b.x;
            float y = b.y;

            xyf p = new xyf(0, 0);
            float r;
         
            {
                p.x = a.a00 * x + a.a01 * y + a.a02;
                p.y = a.a10 * x + a.a11 * y + a.a12;
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
        /// Multiply the matrix by the inverse matrix of b;
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix3x3 operator /(Matrix3x3 a, Matrix3x3 b)
        {
            return
                a.mul(b.invert());
        }
        /// <summary>
        ///  This function checks the equality between two values <see cref="xyz"/>
        ///  This is to avoid errors at the computational accuracy with doubles.
        ///  If their distance of the coordinates is less than <see cref="Utils.epsilon"/> equality is done
        ///  </summary>
        ///  <param name="obj">Matrx value</param>
        ///  <returns> returns true, if they are nearly equal</returns>

        public override bool Equals(object obj)
        {
            if (obj is Matrix3x3)
            {
                Matrix3x3 a = (Matrix3x3)obj;

                if (!Utils.Equals(a.a00, a00)) return false;
                if (!Utils.Equals(a.a01, a01)) return false;
                if (!Utils.Equals(a.a02, a02)) return false;


                if (!Utils.Equals(a.a10, a10)) return false;
                if (!Utils.Equals(a.a11, a11)) return false;
                if (!Utils.Equals(a.a12, a12)) return false;


                if (!Utils.Equals(a.a20, a20)) return false;
                if (!Utils.Equals(a.a21, a21)) return false;
                if (!Utils.Equals(a.a22, a22)) return false;



                return true;

            }

            return base.Equals(obj);
        }

        //public static double[,] ID = { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        /// <summary>
        /// Gets the unitmatrix.
        /// </summary>
        public static Matrix3x3 identity
        {
            get { return new Matrix3x3(1); }
        }

        /// <summary>
        /// Minimum for calculating a scalematrix (see <see cref="Matrix3x3.Scale(xy)"/>)
        /// </summary>
        static double MinScaleFactor = 0.0000001;
        /// <summary>
        /// Retrieves a scalematrix. Its scaling values are given by pt.x, pt.y and pt.z
        /// Fixpoint is the origin (0/0/0)
        /// </summary>
        /// <param name="pt">Scalingvalues</param>
        /// <returns>Scalematrix</returns>

        static public Matrix3x3 Scale(xy pt)
        {
            Matrix3x3 a = new Matrix3x3(1);
            if ((System.Math.Abs(pt.x) < MinScaleFactor))
                pt.x = MinScaleFactor;
            if ((System.Math.Abs(pt.y) < MinScaleFactor))
                pt.y = MinScaleFactor;

            {
                a.a00 = (float)pt.x;
                a.a11 = (float)pt.y;
                a.a22 = 1;

            }
            return a;
        }



        /// <summary>
        /// returns a rotationmatrix with center (0,0)
        /// </summary>
        /// <param name="Angle">Rotationangle ain rad</param>
        /// <returns>Rotationmatrix</returns>
        static public Matrix3x3 Rotation(double Angle)
        {

            Matrix3x3 result = new Matrix3x3(1);
            double sin_a = System.Math.Sin(Angle);
            double cos_a = System.Math.Cos(Angle);
            result.a00 = (float)cos_a;
            result.a10 = (float)(sin_a);
            result.a20 = 0;

            result.a01 = (float)(-sin_a);
            result.a11 = (float)(cos_a);

            result.a22 = (float)(1);
            return result;
        }
        /// <summary>
        /// returns a rotationmatrix with a center and a rotationangle.
        /// </summary>
        /// <param name="Center">Direction of the rotation</param>
        /// <param name="Angle">Rotationangle ain rad</param>
        /// <returns>Rotationmatrix</returns>
        static public Matrix3x3 Rotation(xy Center,double Angle)
        {
           return Translation(Center)*Rotation(Angle)*Translation(Center * (-1));
          
        }
        /// <summary>
        /// Returns a translation matrix. The values are given by the translationvector pt.
        /// </summary>
        /// <param name="pt">Holds the values for the translation</param>
        /// <returns>Translationmatrix</returns>
        static public Matrix3x3 Translation(xy pt)
        {
            Matrix3x3 a = new Matrix3x3(1);
            a.a02 = (float)pt.x;
            a.a12 = (float)pt.y;
            a.a22 = (float)1;
          return a;
        }
        /// <summary>
        /// Returns a scaling matrix. She's scaling the x, y Coordinates by the parametervalues.
        /// </summary>
        /// <param name="x">scalefactor x</param>
        /// <param name="y">scalefactor y</param>

        /// <returns></returns>
        static public Matrix3x3 Scale(double x, double y)
        {
            return Matrix3x3.Scale(new xy(x, y));


        }
        /// <summary>
        /// Returns a translationmatrix for a translation by the vector(x,y)
        /// </summary>
        /// <param name="x">x-Coord of the translationvector</param>
        /// <param name="y">y-Coord of the translationvector</param>

        /// <returns></returns>
        static public Matrix3x3 Translation(double x, double y)
        {
            return Matrix3x3.Translation(new xy(x, y));
        }

        private double Determinant2x2(double[,] M)
        {

            return M[0, 0] * M[1, 1] - M[1, 2] * M[2, 1];
        }


        /// <summary>
        /// Returns a mirror-transformation with the axis goven by A and B.
        /// </summary>
        /// <param name="A">First point of the axis</param>
        /// <param name="B">Second point of the axis</param>
        /// <returns>Mirrortransformation</returns>
        static public Matrix3x3 Mirror(xy A, xy B)
        {
           xy Normal = (A - B).normal().normalize();
            Matrix3x3 a = new Matrix3x3(1);
            a.a00 = (float)(1 - 2 * Normal.x * Normal.x);
            a.a10 = (float)(-2 * Normal.x * Normal.y);


            a.a01 = (float)(-2 * Normal.y * Normal.x);
            a.a11 = (float)(1 - 2 * Normal.y * Normal.y);
            Matrix3x3 U = Matrix3x3.Translation(A * (-1));
            Matrix3x3 V = Matrix3x3.Translation(A);
            return V * a * U;

        
    }

        /// <summary>
        /// Gives a transposed matrix.
        /// </summary>
        /// <returns>transposed matrix</returns>
        public Matrix3x3 transpose()
        {
            Matrix3x3 result = new Matrix3x3(1);
            result.a00 = a00;
            result.a01 = a10;
            result.a02 = a20;

            result.a10 = a01;
            result.a11 = a11;
            result.a12 = a21;

            result.a20 = a02;
            result.a21 = a12;
            result.a22 = a22;


            return result;
        }



        /// <summary>
        /// Constructor for a matrix, who initializes a 3 X 3 matrix by a by a Matrix with diagonal elements = id.
        /// Attention: the paramless constructor new Matrix() returns a zero matrix.
        /// </summary>
        /// <param name="id">Diagonal elements</param>

        public Matrix3x3(int id)
        {
            a00 = id; a10 = 0; a20 = 0;
            a01 = 0; a11 = id; a21 = 0;
            a02 = 0; a12 = 0; a22 = id;


        }



      

      
    }
}
