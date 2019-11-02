using System;
using System.ComponentModel;
using System.Collections;
namespace Drawing3d
{


    /// <summary>
    /// Matrix is the fundamental class for all transformations.
    /// So a Matrix has a 4 Rows  and 4 Cols.
    /// </summary>
    [DefaultValue(new double[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 })]
    [Serializable]
    public struct Matrix
    {


        /// <summary>
        /// Converts a string to a 4 x 4- matrix, where valid delimiter are ; , and ' '.
        /// For example:
        /// <code>
        /// 1    2.4   3  -5;
        /// 2.3  4.4   2   0;
        /// 1    2.4   3  -1;
        /// 1    8.1  3.2 -9;
        ///  
        /// </code>
        /// </summary>
        /// <param name="value">String which describes the matrix</param>
        /// See <see cref="ToString"/>
        /// <returns>A matrix</returns>
        public static Matrix FromString(string value)
        {

            Matrix result = new Matrix(1);

            string[] s = value.Split(new char[4] { ';', '[', ']', ' ' });

            int k = 0;
            ArrayList a = new ArrayList();
            for (k = 0; k < s.Length; k++)
                if (s[k].Trim() != "") a.Add(s[k]);
            if (a.Count != 16) return result;

            result.a00 = (float)Utils.StrToFloat((string)a[0]);
            result.a01 = (float)Utils.StrToFloat((string)a[1]);
            result.a02 = (float)Utils.StrToFloat((string)a[2]);
            result.a03 = (float)Utils.StrToFloat((string)a[3]);

            result.a10 = (float)Utils.StrToFloat((string)a[4]);
            result.a11 = (float)Utils.StrToFloat((string)a[5]);
            result.a12 = (float)Utils.StrToFloat((string)a[6]);
            result.a13 = (float)Utils.StrToFloat((string)a[7]);
            result.a20 = (float)Utils.StrToFloat((string)a[8]);
            result.a21 = (float)Utils.StrToFloat((string)a[9]);
            result.a22 = (float)Utils.StrToFloat((string)a[10]);
            result.a23 = (float)Utils.StrToFloat((string)a[11]);
            result.a30 = (float)Utils.StrToFloat((string)a[12]);
            result.a31 = (float)Utils.StrToFloat((string)a[13]);
            result.a32 = (float)Utils.StrToFloat((string)a[14]);
            result.a33 = (float)Utils.StrToFloat((string)a[15]);
            return result;
        }
        ///// <summary>
        ///// Multiplication operateor og two matrices
        ///// </summary>
        ///// <param name="M"></param>
        ///// <param name="P"></param>
        ///// <returns></returns>
       
        /// <summary>
        /// BIAS - Matrix
        /// </summary>
        /// <returns></returns>
        public static Matrix Bias()
        {
            Matrix Result = new Matrix();
            Result.a00 = 0.5f; Result.a10 = 0; Result.a20 = 0; Result.a30 = 0;
            Result.a01 = 0; Result.a11 = 0.5f; Result.a21 = 0; Result.a31 = 0;
            Result.a02 = 0; Result.a12 = 0; Result.a22 = 0.5f; Result.a32 = 0;
            Result.a03 = 0.5f; Result.a13 = 0.5f; Result.a23 = 0.5f; Result.a33 = 1;
            return Result;

        }
        /// <summary>
        /// Calculate an inverted Biasmatrix
        /// </summary>
        public static Matrix BiasInvert = Bias().invert();
        /// <summary>
        /// Returns the rotationaxis of a orthogonal matrix and the rotation angle.
        /// </summary>
        /// <param name="Angle">Angle of the rotation of the matrix</param>
        /// <returns></returns>
        public LineType GetRotationAxis(ref double Angle)
        {
         
            Matrix M = this;
            xyz P1 = new xyz(1, 0, 0);
            xyz P2 = M * P1;
            xyz P3 = M * P2;
            LineType Result;
            if (P2.Equals(P1))
            {
                P1 = new xyz(0, 1, 0);
                P2 = M * P1;
                P3 = M * P2;
                if (P2.Equals(P1))
                {
                    Result = new LineType(P1, new xyz(1, -1, 0));
                    Angle = Utils.Angle(new xyz(0, 0, 1), M * (new xyz(0, 0, 1)), new xyz(1, 1, 0));
                    if (System.Math.Abs(Angle) < 0.000001)
                    {
                        Result = new LineType(xyz.Null, new xyz(0, 0, 1));
                    }
                    return Result;
                }
            }

            Plane Plane1 = new Plane(((P1 + P2) * 0.5), P2 - P1);
            Plane Plane2 = new Plane(((P2 + P3) * 0.5), P3 - P2);

           
          
            return Plane1.Cross(Plane2);
        }
        /// <summary>
        /// Gets the i-th row
        /// </summary>
        /// <param name="i">Index of the row.</param>
        /// <returns>The i-th row</returns>
        public double[] GetRow(int i)
        {

            if (i == 0) return new double[] { a00, a01, a02, a03 };
            if (i == 1) return new double[] { a10, a11, a12, a13 };
            if (i == 2) return new double[] { a20, a21, a22, a23 };
            if (i == 3) return new double[] { a30, a31, a32, a33 };
            return new double[] { 0, 0, 0, 0 };

        }
        /// <summary>
        /// Gives the cols of the matrix
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public double[] GetCol(int i)
        {

            if (i == 0) return new double[] { a00, a10, a20, a30 };
            if (i == 1) return new double[] { a01, a11, a21, a31 };
            if (i == 2) return new double[] { a02, a12, a22, a32 };
            if (i == 3) return new double[] { a03, a13, a23, a33 };
            return new double[] { 0, 0, 0, 0 };

        }
        /// <summary>
        /// Converts the matrix to an array of doubles
        /// </summary>
        /// <returns>double array which contains the elements of the matrix</returns>
        public double[] toArray()
        {
            return new double[] { a00, a10, a20, a30, a01, a11, a21, a31, a02, a12, a22, a32, a03, a13, a23, a33 };
        }


        /// <summary>
        /// gets s matrix from the values of the array a.
        /// e.g:a00, a10, a20, a30, a01, a11, a21, a31, a02, a12, a22, a32, a03, a13, a23, a33
        /// </summary>
        /// <param name="a">the array</param>
        public Matrix fromArray(double[] a)
        {
            Matrix Result = Matrix.identity;
            Result.a00 = (float)a[0];
            Result.a10 = (float)a[1];
            Result.a20 = (float)a[2];
            Result.a30 = (float)a[3];
            Result.a01 = (float)a[4];
            Result.a11 = (float)a[5];
            Result.a21 = (float)a[6];
            Result.a31 = (float)a[7];
            Result.a02 = (float)a[8];
            Result.a12 = (float)a[9];
            Result.a22 = (float)a[10];
            Result.a32 = (float)a[11];
            Result.a03 = (float)a[12];
            Result.a13 = (float)a[13];
            Result.a23 = (float)a[14];
            Result.a33 = (float)a[15];
            return Result;
        }
        /// <summary>
        /// Multiply the point of type <see cref="xyz"/> with the matrix but omit the fordth component.
        /// This multiplication gives the zero point by a multiplication of a zero point.
        /// It can be used to transform vectors.
        /// </summary>
        /// <param name="a">a point, which will be multiplied</param>
        /// <returns>
        /// Transformed vector.
        /// </returns>
        public xyz multaffin(xyz a)
        {
            xyz p = new xyz(0, 0, 0);
           
           
            {
                p.x = a00 * a.x + a01 * a.y + a02 * a.z ;
                p.y = a10 * a.x + a11 * a.y + a12 * a.z ;
                p.z = a20 * a.x + a21 * a.y + a22 * a.z ;
              
               
            }
            return p;
        }
        /// <summary>
        /// Multiply the point of type <see cref="xyzf"/> with the matrix but omit the fordth component.
        /// This multiplication gives the zero point by a multiplication of a zero point.
        /// It can be used to transform vectors.
        /// </summary>
        /// <param name="a">a point, which will be multiplied</param>
        /// <returns>
        /// Transformed vector.
        /// </returns>
        public xyzf multaffin(xyzf a)
        {
            xyzf p = new xyzf(0, 0, 0);
            {
                p.x = a00 * a.x + a01 * a.y + a02 * a.z;
                p.y = a10 * a.x + a11 * a.y + a12 * a.z;
                p.z = a20 * a.x + a21 * a.y + a22 * a.z;


            }
            return p;
        }
        /// <summary>
        /// Multiplicates with a Matrix M
        /// </summary>
        /// <param name="M">Matrix, which will be multiplicated</param>
        /// <returns></returns>
        public Matrix mul(Matrix M)
        {

            Matrix Result; ;

            Result.a00 = a00 * M.a00 + a01 * M.a10 + a02 * M.a20 + a03 * M.a30;
            Result.a01 = a00 * M.a01 + a01 * M.a11 + a02 * M.a21 + a03 * M.a31;
            Result.a02 = a00 * M.a02 + a01 * M.a12 + a02 * M.a22 + a03 * M.a32;
            Result.a03 = a00 * M.a03 + a01 * M.a13 + a02 * M.a23 + a03 * M.a33;

            Result.a10 = a10 * M.a00 + a11 * M.a10 + a12 * M.a20 + a13 * M.a30;
            Result.a11 = a10 * M.a01 + a11 * M.a11 + a12 * M.a21 + a13 * M.a31;
            Result.a12 = a10 * M.a02 + a11 * M.a12 + a12 * M.a22 + a13 * M.a32;
            Result.a13 = a10 * M.a03 + a11 * M.a13 + a12 * M.a23 + a13 * M.a33;

            Result.a20 = a20 * M.a00 + a21 * M.a10 + a22 * M.a20 + a23 * M.a30;
            Result.a21 = a20 * M.a01 + a21 * M.a11 + a22 * M.a21 + a23 * M.a31;
            Result.a22 = a20 * M.a02 + a21 * M.a12 + a22 * M.a22 + a23 * M.a32;
            Result.a23 = a20 * M.a03 + a21 * M.a13 + a22 * M.a23 + a23 * M.a33;

            Result.a30 = a30 * M.a00 + a31 * M.a10 + a32 * M.a20 + a33 * M.a30;
            Result.a31 = a30 * M.a01 + a31 * M.a11 + a32 * M.a21 + a33 * M.a31;
            Result.a32 = a30 * M.a02 + a31 * M.a12 + a32 * M.a22 + a33 * M.a32;
            Result.a33 = a30 * M.a03 + a31 * M.a13 + a32 * M.a23 + a33 * M.a33;

            return Result;

        }
        /// <summary>
        /// Calculates the difference to the Matrix M
        /// </summary>
        /// <param name="M">The matrix to which the difference is calculated</param>
        /// <returns>Difference of the matrices</returns>
        public Matrix sub(Matrix M)
        {

            Matrix Result; ;

            Result.a00 = a00 - M.a00;
            Result.a01 = a01 - M.a01;
            Result.a02 = a02 - M.a02;
            Result.a03 = a03 - M.a03;

            Result.a10 = a10 - M.a10;
            Result.a11 = a11 - M.a11;
            Result.a12 = a12 - M.a12;
            Result.a13 = a13 - M.a13;



            Result.a20 = a20 - M.a20;
            Result.a21 = a21 - M.a21;
            Result.a22 = a22 - M.a22;
            Result.a23 = a23 - M.a23;


            Result.a30 = a30 - M.a30;
            Result.a31 = a31 - M.a31;
            Result.a32 = a32 - M.a32;
            Result.a33 = 1;




            return Result;

        }
        /// <summary>
        /// Gets the determinant of the matrix
        /// </summary>
        /// <returns>The determinant of a the matrix</returns>
        public double Determinant()
        {

            return a00 * (a11 * a22 * a33 + a12 * a23 * a31 + a13 * a21 * a32 - a11 * a23 * a32 - a12 * a21 * a33 - a13 * a22 * a31)
             - a01 * (a12 * a23 * a30 + a13 * a20 * a32 + a10 * a22 * a33 - a12 * a20 * a33 - a13 * a22 * a30 - a10 * a23 * a32)
             + a02 * (a13 * a20 * a31 + a10 * a21 * a33 + a11 * a23 * a30 - a13 * a21 * a30 - a10 * a22 * a31 - a11 * a20 * a33)
             - a03 * (a10 * a21 * a32 + a11 * a22 * a30 + a12 * a20 * a31 - a10 * a22 * a31 - a11 * a23 * a32 - a12 * a21 * a30);

        }
        /// <summary>
        /// Inverts the Matrix
        /// </summary>
        /// <returns></returns>
        public Matrix invert()
        {
            Matrix M = Matrix.identity;

            float d = (float)Determinant();
            if (d == 0) return M;
            M.a00 = (a11 * a22 * a33 + a12 * a23 * a31 + a13 * a21 * a32 - a11 * a23 * a32 - a12 * a21 * a33 - a13 * a22 * a31) / d;
            M.a10 = -(a12 * a23 * a30 + a13 * a20 * a32 + a10 * a22 * a33 - a12 * a20 * a33 - a13 * a22 * a30 - a10 * a23 * a32) / d;
            M.a20 = (a13 * a20 * a31 + a10 * a21 * a33 + a11 * a23 * a30 - a13 * a21 * a30 - a10 * a23 * a31 - a11 * a20 * a33) / d;
            M.a30 = -(a10 * a21 * a32 + a11 * a22 * a30 + a12 * a20 * a31 - a10 * a22 * a31 - a11 * a20 * a32 - a12 * a21 * a30) / d;


            M.a01 = -(a21 * a32 * a03 + a22 * a33 * a01 + a23 * a31 * a02 - a21 * a33 * a02 - a22 * a31 * a03 - a23 * a32 * a01) / d;
            M.a11 = (a22 * a33 * a00 + a23 * a30 * a02 + a20 * a32 * a03 - a22 * a30 * a03 - a23 * a32 * a00 - a20 * a33 * a02) / d;
            M.a21 = -(a23 * a30 * a01 + a20 * a31 * a03 + a21 * a33 * a00 - a23 * a31 * a00 - a20 * a33 * a01 - a21 * a30 * a03) / d;
            M.a31 = (a20 * a31 * a02 + a21 * a32 * a00 + a22 * a30 * a01 - a20 * a32 * a01 - a21 * a30 * a02 - a22 * a31 * a00) / d;

            M.a02 = (a31 * a02 * a13 + a32 * a03 * a11 + a33 * a01 * a12 - a31 * a03 * a12 - a32 * a01 * a13 - a33 * a02 * a11) / d;
            M.a12 = -(a32 * a03 * a10 + a33 * a00 * a12 + a30 * a02 * a13 - a32 * a00 * a13 - a33 * a02 * a10 - a30 * a03 * a12) / d;
            M.a22 = (a33 * a00 * a11 + a30 * a01 * a13 + a31 * a03 * a10 - a33 * a01 * a10 - a30 * a03 * a11 - a31 * a00 * a13) / d;
            M.a32 = -(a30 * a01 * a12 + a31 * a02 * a10 + a32 * a00 * a11 - a30 * a02 * a11 - a31 * a00 * a12 - a32 * a01 * a10) / d;

            M.a03 = -(a01 * a12 * a23 + a02 * a13 * a21 + a03 * a11 * a22 - a01 * a13 * a22 - a02 * a11 * a23 - a03 * a12 * a21) / d;
            M.a13 = (a02 * a13 * a20 + a03 * a10 * a22 + a00 * a12 * a23 - a02 * a10 * a23 - a03 * a12 * a20 - a00 * a13 * a22) / d;
            M.a23 = -(a03 * a10 * a21 + a00 * a11 * a23 + a01 * a13 * a20 - a03 * a11 * a20 - a00 * a13 * a21 - a01 * a10 * a23) / d;
            M.a33 = (a00 * a11 * a22 + a01 * a12 * a20 + a02 * a10 * a21 - a00 * a12 * a21 - a01 * a10 * a22 - a02 * a11 * a20) / d;

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
        /// A[3,0]
        /// </summary>
        public float
            a30;
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
        /// A[3,1]
        /// </summary>
        public float
            a31;



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
        /// A[3,2]
        /// </summary>
        public float
            a32;

        /// <summary>
        /// A[0,3]
        /// </summary>
        public float
            a03;


        /// <summary>
        /// A[1,3]
        /// </summary>
        public float
            a13;

        /// <summary>
        /// A[2,3]
        /// </summary>
        public float
            a23;

        /// <summary>
        /// A[3,1]
        /// </summary>
        public float
            a33;





        /// <summary>
        /// Converts a matrix in a string by using "[" and "]" for every row and ";" inside the row as delimiter
        /// 
        /// </summary>
        /// <example>
        /// [[3.4;1.2;4.3;5.7];[2.8;6.4;3.6;5.3];[2.4;6.8;5.7;3.2];[1.3;4.6;7.8;9.2]]
        /// <see cref="FromString(string)"/>
        /// </example>
        /// <returns></returns>
        public override string ToString()
        {
            string s = Utils.DoubleFormat;
            Utils.DoubleFormat = "0.######";
            string Result =
                Utils.FloatToStr(a00) + " " + Utils.FloatToStr(a01) + " " + Utils.FloatToStr(a02) + " " + Utils.FloatToStr(a03) + ";"
               + " " + Utils.FloatToStr(a10) + ";" + Utils.FloatToStr(a11) + " " + Utils.FloatToStr(a12) + " " + Utils.FloatToStr(a13) + " "
               + " " + Utils.FloatToStr(a20) + ";" + Utils.FloatToStr(a21) + " " + Utils.FloatToStr(a22) + " " + Utils.FloatToStr(a23) + " "
               + " " + Utils.FloatToStr(a30) + ";" + Utils.FloatToStr(a31) + " " + Utils.FloatToStr(a32) + " " + Utils.FloatToStr(a33);
            Utils.DoubleFormat = s;
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
        public static Matrix operator *(Matrix a, Matrix b)
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
           
            float[] F = new float[16];

          
            F[0 + 0 * 4] = (float)a00;
            F[1 + 0 * 4] = (float)a10;
            F[2 + 0 * 4] = (float)a20;
            F[3 + 0 * 4] = (float)a30;

            F[0 + 1 * 4] = (float)a01;
            F[1 + 1 * 4] = (float)a11;
            F[2 + 1 * 4] = (float)a21;
            F[3 + 1 * 4] = (float)a31;

            F[0 + 2 * 4] = (float)a02;
            F[1 + 2 * 4] = (float)a12;
            F[2 + 2 * 4] = (float)(a22);
            if (F[10] == float.NaN)
            {
            }

            F[3 + 2 * 4] = (float)a32;

            F[0 + 3 * 4] = (float)a03;
            F[1 + 3 * 4] = (float)a13;
            F[2 + 3 * 4] = (float)a23;
            F[3 + 3 * 4] = (float)a33;
           
            return F;
        }
        /// <summary>
        /// This operator returns the multiplication of a three-dimensional vector with the matrix.
        /// The Matrix a has to be a 4 X 4 matrix. So the 3D-point will be "lifted" to a 4D-Point by setting the fourth coordinate to 1.
        /// After the well known matrix multiplication the coordinates are divided by the result in the fourth coordinate.
        /// In this way, it is for example possible to transform a point by a matrix, which represents a perspectively projection.
        /// <seealso cref="xyz.mul(double)"/>
        /// </summary>
        /// <param name="a">4X4 Matrix</param>
        /// <param name="b">3D-Vector</param>
        /// <returns>3D-Vector</returns>

        public static xyz operator *(Matrix a, xyz b)
        { return b.mul(a); }


        /// <summary>
        /// This operator returns the multiplication of a three-dimensional floatvector with the matrix.
        /// The Matrix a has to be a 4 X 4 matrix. So the 3D-point will be "lifted" to a 4D-Point by setting the fourth coordinate to 1.
        /// After the well known matrix multiplication the coordinates are divided by the result in the fourth coordinate.
        /// In this way, it is for example possible to transform a point by a matrix, which represents a perspectively projection.
        /// <seealso cref="xyz.mul(double)"/>
        /// </summary>
        /// <param name="a">4X4 Matrix</param>
        /// <param name="b">3D-floatvector</param>
        /// <returns>3D-floatvector</returns>


        public static xyzf operator *(Matrix a, xyzf b)
        {

            float x = b.x;
            float y = b.y;
            float z = b.z;
            xyzf p = new xyzf(0, 0, 0);
            float r;
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
        /// Multiply the matrix by the inverse matrix of b;
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Matrix operator /(Matrix a, Matrix b)
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


            if (obj is Matrix)
            {
                Base B = this.toBase();
                Base A = ((Matrix)obj).toBase();
                return B.Equals(A);
            }

            else

                if (obj is Double[])
            {
                double[] a = (double[])obj;
                if (a.Length != 16) return false;
                if (!Utils.Equals(a[0], a00)) return false;
                if (!Utils.Equals(a[1], a01)) return false;
                if (!Utils.Equals(a[2], a02)) return false;
                if (!Utils.Equals(a[3], a03)) return false;

                if (!Utils.Equals(a[4], a10)) return false;
                if (!Utils.Equals(a[5], a11)) return false;
                if (!Utils.Equals(a[6], a12)) return false;
                if (!Utils.Equals(a[7], a13)) return false;

                if (!Utils.Equals(a[8], a20)) return false;
                if (!Utils.Equals(a[9], a21)) return false;
                if (!Utils.Equals(a[10], a22)) return false;
                if (!Utils.Equals(a[11], a23)) return false;

                if (!Utils.Equals(a[12], a30)) return false;
                if (!Utils.Equals(a[13], a31)) return false;
                if (!Utils.Equals(a[14], a32)) return false;
                if (!Utils.Equals(a[15], a33)) return false;
                return true;

            }

            return base.Equals(obj);
        }

        //public static double[,] ID = { { 1, 0, 0, 0 }, { 0, 1, 0, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 1 } };
        /// <summary>
        /// Gets the unitmatrix.
        /// </summary>
        public static Matrix identity
        {
            get { return new Matrix(1); }
        }
       
       
        /// <summary>
        /// Will be called from <see cref="FromEuler(xyz)"/>
        /// <seealso cref="FromEuler"/>
        /// </summary>
        /// <param name="M">Orthogonal Matrix</param>
        /// <returns>Euler angles</returns>
        public static xyz toEuler(Matrix M)
        {
            double x1 = System.Math.Atan2(-M.a01, M.a11);
            double y1 = System.Math.Atan2(M.a21, System.Math.Sqrt(M.a20 * M.a20 + M.a22 * M.a22));
            double z1 = System.Math.Atan2(-M.a20, M.a22);
            return new xyz(x1, y1, z1);

        }
     /// <summary>
     /// Calculates a Matrix with the Euler angles are stored in E
     /// </summary>
     /// <param name="E">Euler angles</param>
     /// <returns>Return matrix</returns>
        public static Matrix FromEuler(xyz E)
        {

            Matrix X = Matrix.identity;
            Matrix Y = Matrix.identity;
            Matrix Z = Matrix.identity;

            X.a00 = (float)System.Math.Cos(E.x);// rot um z
            X.a01 = -(float)System.Math.Sin(E.x);
            X.a10 = (float)System.Math.Sin(E.x);
            X.a11 = (float)System.Math.Cos(E.x);

            Y.a11 = (float)System.Math.Cos(E.y);  // rot um x
            Y.a12 = -(float)System.Math.Sin(E.y);
            Y.a21 = (float)System.Math.Sin(E.y);
            Y.a22 = (float)System.Math.Cos(E.y);


            Z.a00 = (float)System.Math.Cos(E.z); // rot um y
            Z.a02 = (float)System.Math.Sin(E.z);
            Z.a20 = -(float)System.Math.Sin(E.z);
            Z.a22 = (float)System.Math.Cos(E.z);



            return (X * Y) * Z;


        }

       
      
        /// <summary>
        /// Gets the transformation matrix for a "look at" situation. The camera is at CameraPosition
        /// and looks to CameraTarget. Additionally a normalvector CameraUpVector has to be given, because the 
        /// Camera can turn up or down...
        /// </summary>
        /// <param name="CameraPosition">Position of the camera</param>
        /// <param name="CameraTarget">The camera looks to that point</param>
        /// <param name="CameraUpVector">Normalvector of the camera</param>
        /// <example> Matrix M = LookAt(new xyz(0,10,2), new xyz(0,0,2), new xyz(0,0,1));</example>
        /// <returns></returns>
        public static Matrix LookAt(xyz CameraPosition, xyz CameraTarget, xyz CameraUpVector)
        {
            Base B = Base.UnitBase;
            B = Base.LookAt(CameraPosition, CameraTarget, CameraUpVector);
            return B.ToMatrix().invert();

        }
        /// <summary>
        /// Returns a perspective matrix.
        /// </summary>
        /// <param name="FovY">Field of view in degree</param>
        /// <param name="AspectRatio">The ratio between x and y </param>
        /// <param name="zNear">Nearclipping</param>
        /// <param name="zFar">Farclipping</param>
        /// <returns></returns>
        public static Matrix Perspective(double FovY, double AspectRatio, double zNear, double zFar)
        {
            FovY = FovY * System.Math.PI / 360f;
            double bottom = -System.Math.Tan(FovY) * zNear;

            double top = System.Math.Tan(FovY) * zNear;
            double near = zNear;
            double far = zFar;
            double left = bottom * AspectRatio;
            double right = top * AspectRatio;
            return Frustum(left, right, bottom, top, near, far);
        }
        /// <summary>
        /// Returns the ortogonal projection, where the point( right, top, far) is mapped to
        /// (1, 1, 1) and  the point( left, bottom, near) to (-1, -1, -1)
        /// Imagine a cube, which is genereted by it's edgepoints ( right, top, far) and
        /// ( left, bottom, near)
        /// Remark: The viewing cube is clipping the scene
        /// </summary>
        /// <param name="Left">Leftcoordinate of the viewing cube</param>
        /// <param name="Right">Rightcoordinate of the viewing cube</param>
        /// <param name="Bottom">Bottomcoordinate of the viewing cube</param>
        /// <param name="Top">Topcoordinate of the viewing cube</param>
        /// <param name="Near">Nearcoordinate of the viewing cube</param>
        /// <param name="Far">Farcoordinate of the viewing cube</param>
        /// <returns>Orthogonal projectionmatrix</returns>
        /// <seealso cref="Frustum"/>
        static public Matrix Orthogonal(double Left, double Right, double Bottom, double Top, double Near, double Far)
        {
            Matrix a = new Matrix(1);
            a.a00 = (float)(2 / (Right - Left));
            a.a11 = (float)(2 / (Top - Bottom));
            a.a22 = (float)(-2 / (Far - Near));
            a.a03 = (float)(-(Right + Left) / (Right - Left));
            a.a13 = (float)(-(Top + Bottom) / (Top - Bottom));
            a.a23 = (float)(-(Far + Near) / (Far - Near));

            return a;
        }
        /// <summary>
        /// Returns the perspective projection. The Frustum, which is given by (right, top, near)
        /// (left, top, near), ( right, bottom, near) and (left, bottom, near) as surface area
        /// and (right, top, far), (left, top, far), ( right, bottom, far) and (left, bottom, far)
        /// as cover area.
        /// This frustum will be transformed into a cube with (1, 1, 1) and (-1, -1, -1)
        /// </summary>
        /// Remark: The frustum is clipping scene<param name="Left"></param>
        /// <param name="Right">Right</param>
        /// <param name="Bottom">Bottom</param>
        /// <param name="Top">Top</param>
        /// <param name="Near">Near</param>
        /// <param name="Far">Farr</param>
        /// <returns>Perspective projectionmatrix</returns>
        /// <seealso cref="Orthogonal"/>
        static public Matrix Frustum(double Left, double Right, double Bottom, double Top, double Near, double Far)
        {
            Matrix a = new Matrix(1);
            a.a00 = (float)(2 * Near / (Right - Left));
            a.a01 = 0;
            a.a02 = (float)((Right + Left) / (Right - Left));
            a.a03 = 0;

            a.a11 = (float)(2 * Near / (Top - Bottom));
            a.a12 = (float)((Top + Bottom) / (Top - Bottom));


            a.a22 = (float)(-(Far + Near) / (Far - Near));
            a.a23 = (float)(-2 * Far * Near / (Far - Near));
            a.a32 = -1;
            a.a33 = 0;

            return a;
        }
        /// <summary>
        /// Minimum for calculating a scalematrix (see <see cref="Scale(xyz)"/>)
        /// </summary>
        static double MinScaleFactor = 0.0000001;
        /// <summary>
        /// Retrieves a scalematrix. Its scaling values are given by pt.x, pt.y and pt.z
        /// Fixpoint is the origin (0/0/0)
        /// </summary>
        /// <param name="pt">Scalingvalues</param>
        /// <returns>Scalematrix</returns>

        static public Matrix Scale(xyz pt)
        {
            Matrix a = new Matrix(1);
            if ((System.Math.Abs(pt.x) < MinScaleFactor))
                pt.x = MinScaleFactor;
            if ((System.Math.Abs(pt.y) < MinScaleFactor))
                pt.y = MinScaleFactor;
            if ((System.Math.Abs(pt.z) < MinScaleFactor))
                pt.z = MinScaleFactor;
            {
                a.a00 = (float)pt.x;
                a.a11 = (float)pt.y;
                a.a22 = (float)pt.z;
                a.a33 = 1;
            }
            return a;
        }
        /// <summary>
        /// Retrieves a scalematrix. Its scaling values are given by pt.x, pt.y and pt.z
        /// Fixpoint is the origin (0/0/0)
        /// </summary>
        /// <param name="pt">Scalingvalues</param>
        /// <returns>Scalematrix</returns>

        static public Matrix scale(xyz pt)
        {
            Matrix a = new Matrix(1);
            if ((System.Math.Abs(pt.x) < MinScaleFactor))
                pt.x = MinScaleFactor;
            if ((System.Math.Abs(pt.y) < MinScaleFactor))
                pt.y = MinScaleFactor;
            if ((System.Math.Abs(pt.z) < MinScaleFactor))
                pt.z = MinScaleFactor;
            {
                a.a00 = (float)pt.x;
                a.a11 = (float)pt.y;
                a.a22 = (float)pt.z;
                a.a33 = 1;
            }
            return a;
        }
        /// <summary>
        /// Overloads the scalemethod with an arbitrary fixpoint origin.
        /// </summary>
        /// <param name="Origin">Fixpoint</param>
        /// <param name="pt">Scalefactors</param>
        /// <returns>Resultmatrix</returns>
        static public Matrix scale(xyz Origin, xyz pt)
        {
            Matrix Tinv = Matrix.Translation(Origin * (-1));
            Matrix T = Matrix.Translation(Origin);
            return T * (Scale(pt) * Tinv);
        }
        /// <summary>
        /// Overloads the scalemethod with an arbitrary fixpoint origin and a destination point dest.
        /// </summary>
        /// <param name="Origin">Fixpoint</param>
        /// <param name="dest">Destinationpoint</param>
        /// <param name="source">Sourcepoint</param>
        /// <returns>Resultmatrix</returns>

        static public Matrix scale(xyz Origin, xyz source, xyz dest)
        {
            xyz s = source;
            xyz d = dest;
            if (System.Math.Abs(s.x) < MinScaleFactor)
            { s.x = 1; d.x = 1; }
            if (System.Math.Abs(s.y) < MinScaleFactor)
            { s.y = 1; d.y = 1; }
            if (System.Math.Abs(s.z) < MinScaleFactor)
            { s.z = 1; d.z = 1; }
            return scale(Origin, new xyz(d.x / s.x, d.y / s.y, d.z / s.z));
        }


        /// <summary>
        /// returns a rotationmatrix , whose rotation-axis is given by the direction. Angle is the rotationangle.
        /// </summary>
        /// <param name="direction">Direction of the rotation</param>
        /// <param name="Angle">Rotationangle ain rad</param>
        /// <returns>Rotationmatrix</returns>
        static public Matrix Rotation(xyz direction, double Angle)
        {

            Matrix result = new Matrix(1);
            double sin_a = System.Math.Sin(Angle);
            double cos_a = System.Math.Cos(Angle);
            result.a00 = (float)(direction.x * direction.x + (1 - direction.x * direction.x) * cos_a);
            result.a10 = (float)(direction.x * direction.y * (1 - cos_a) + direction.z * sin_a);
            result.a20 = (float)(direction.x * direction.z * (1 - cos_a) - direction.y * sin_a);

            result.a01 = (float)(direction.x * direction.y * (1 - cos_a) - direction.z * sin_a);
            result.a11 = (float)(direction.y * direction.y + (1 - direction.y * direction.y) * cos_a);
            result.a21 = (float)(direction.y * direction.z * (1 - cos_a) + direction.x * sin_a);

            result.a02 = (float)(direction.x * direction.z * (1 - cos_a) + direction.y * sin_a);
            result.a12 = (float)(direction.y * direction.z * (1 - cos_a) - direction.x * sin_a);
            result.a22 = (float)(direction.z * direction.z + (1 - direction.z * direction.z) * cos_a);
            return result;
        }
        private static Matrix Rotation(xyz Point, xyz direction, double Angle)
        {
            direction = direction.normalized();
            Matrix result = new Matrix();
            double sin_a = System.Math.Sin(Angle);
            double cos_a = System.Math.Cos(Angle);

            result.a00 = (float)(direction.x * direction.x + (1 - direction.x * direction.x) * cos_a);
            result.a10 = (float)(direction.x * direction.y * (1 - cos_a) + direction.z * sin_a);
            result.a20 = (float)(direction.x * direction.z * (1 - cos_a) - direction.y * sin_a);

            result.a01 = (float)(direction.x * direction.y * (1 - cos_a) - direction.z * sin_a);
            result.a11 = (float)(direction.y * direction.y + (1 - direction.y * direction.y) * cos_a);
            result.a21 = (float)(direction.y * direction.z * (1 - cos_a) + direction.x * sin_a);

            result.a02 = (float)(direction.x * direction.z * (1 - cos_a) + direction.y * sin_a);
            result.a12 = (float)(direction.y * direction.z * (1 - cos_a) - direction.x * sin_a);
            result.a22 = (float)(direction.z * direction.z + (1 - direction.z * direction.z) * cos_a);

            result.a03 = (float)(Point.x - (result.a00 * Point.x + result.a01 * Point.y + result.a02 * Point.z));
            result.a13 = (float)(Point.y - (result.a10 * Point.x + result.a11 * Point.y + result.a12 * Point.z));
            result.a23 = (float)(Point.z - (result.a20 * Point.x + result.a21 * Point.y + result.a22 * Point.z));


            result.a33 = 1;
            return result;
        }
        /// <summary>
        /// Returns a rotationmatrix with the linetype L and the rotationangle Angle
        /// Of course L doesn`t have to contain the origin
        /// </summary>
        /// <param name="L">Rotationaxis</param>
        /// <param name="Angle">Rotationangle rad</param>
        /// <returns>Rotationmatrix</returns>
        static public Matrix Rotation(LineType L, double Angle)
        {
          

            return Matrix.Rotation(L.P, L.Direction, Angle);
           
        }
        /// <summary>
        /// Returns a translation matrix. The values are given by the translationvector pt.
        /// </summary>
        /// <param name="pt">Holds the values for the translation</param>
        /// <returns>Translationmatrix</returns>
        static public Matrix Translation(xyz pt)
        {
            Matrix a = new Matrix(1);
            a.a03 = (float)pt.x;
            a.a13 = (float)pt.y;
            a.a23 = (float)pt.z;
            a.a33 = 1;

            return a;
        }
        /// <summary>
        /// Returns a scaling matrix. She's scaling the x, y, z Coordinates by the paraetervalues.
        /// </summary>
        /// <param name="x">scalefactor x</param>
        /// <param name="y">scalefactor y</param>
        /// <param name="z">scalefactor z</param>
        /// <returns></returns>
        static public Matrix Scale(double x, double y, double z)
        {
            return Matrix.Scale(new xyz(x, y, z));


        }
        /// <summary>
        /// Returns a translationmatrix for a translation by the vector(x,y,z)
        /// </summary>
        /// <param name="x">x-Coord of the translationvector</param>
        /// <param name="y">y-Coord of the translationvector</param>
        /// <param name="z">z-Coord of the translationvector</param>
        /// <returns></returns>
        static public Matrix Translation(double x, double y, double z)
        {
            return Matrix.Translation(new xyz(x, y, z));
        }

        private double Determinant3x3(double[,] M)
        {

            return M[0, 0] * M[1, 1] * M[2, 2] + M[0, 1] * M[1, 2] * M[2, 0] + M[0, 2] * M[1, 0] * M[2, 1] -
                (M[0, 0] * M[1, 2] * M[2, 1] + M[0, 1] * M[1, 0] * M[2, 2] + M[0, 2] * M[1, 1] * M[2, 0]);
        }

       
        /// <summary>
        /// Returns a mirror-transformation, whose mirrorplane contains the origin 0/0/0 and has the normal vector Normal
        /// </summary>
        /// <param name="Normal">Normal to the mirrorplane</param>
        /// <returns>Mirrortransformation</returns>
        static public Matrix Mirror(xyz Normal)
        {

            Matrix a = new Matrix(1);

            a.a00 = (float)(1 - 2 * Normal.x * Normal.x);
            a.a10 = (float)(-2 * Normal.x * Normal.y);
            a.a20 = (float)(-2 * Normal.x * Normal.z);

            a.a01 = (float)(-2 * Normal.y * Normal.x);
            a.a11 = (float)(1 - 2 * Normal.y * Normal.y);
            a.a21 = (float)(-2 * Normal.y * Normal.z);

            a.a02 = (float)(-2 * Normal.z * Normal.x);
            a.a12 = (float)(-2 * Normal.z * Normal.y);
            a.a22 = (float)(1 - 2 * Normal.z * Normal.z);
            return a;
        }
        /// <summary>
        /// Calculates a mirror transformation to a mirror plane.
        /// Remark: the normalvector of the plane have to be normalized.
        /// </summary>
        /// <param name="MirrorPlane">mirrorplane</param>
        /// <returns>Transformation</returns>
        static public Matrix Mirror(Plane MirrorPlane)
        {
            Matrix Tinv = Matrix.Translation(MirrorPlane.P.mul(-1));
            Matrix T = Matrix.Translation(MirrorPlane.P);
            return T * (Mirror(MirrorPlane.NormalUnit) * Tinv);
        }
        /// <summary>
        /// Gives a transposed matrix.
        /// </summary>
        /// <returns>transposed matrix</returns>
        public Matrix transpose()
        {
            Matrix result = new Matrix(1);
            result.a00 = a00;
            result.a01 = a10;
            result.a02 = a20;
            result.a03 = a30;
            result.a10 = a01;
            result.a11 = a11;
            result.a12 = a21;
            result.a13 = a31;
            result.a20 = a02;
            result.a21 = a12;
            result.a22 = a22;
            result.a23 = a32;
            result.a30 = a03;
            result.a31 = a13;
            result.a32 = a23;
            result.a33 = a33;

            return result;
        }


      
        /// <summary>
        /// Constructor for a matrix, who initializes a 4 X 4 matrix by a by a Matrix with diagonal elements = id.
        /// Attention: the paramless constructor new Matrix() returns a zero matrix.
        /// </summary>
        /// <param name="id">Diagonal elements</param>

        public Matrix(int id)
        {
            a00 = id; a10 = 0; a20 = 0; a30 = 0;
            a01 = 0; a11 = id; a21 = 0; a31 = 0;
            a02 = 0; a12 = 0; a22 = id; a32 = 0;
            a03 = 0; a13 = 0; a23 = 0; a33 = id;

        }
     
       
        
        /// <summary>
        /// Retrieves the count of columns of the matrix
        /// </summary>
        public int Cols
        {
            get
            {
                return 4;
            }
        }
        /// <summary>
        /// retrieves the Rowcount of the matrix
        /// </summary>
        public int Rows
        {
            get
            {
                return 4;
            }
        }
        /// <summary>
        /// converts a <see cref="Matrix"/> to a <see cref="Matrix3x3"/>
        /// by omittng the third row and the third col.
        /// </summary>
        /// <returns></returns>
        public Matrix3x3 toMatrix3x3()
        {

            Matrix3x3 Result = Matrix3x3.identity;
            Result.a00 = a00;
            Result.a10 = a10;
            Result.a20 = a20;
            Result.a01 = a01;
            Result.a11 = a11;
            Result.a21 = a21;
            Result.a02 = a02;
            Result.a12 = a12;
            Result.a22 = a22;
            return Result;
        }
        /// <summary>
        /// Converts a Matrix to a base, by taken the columnvectors as Basevectors.
        /// </summary>
        /// <returns></returns>
        public Base toBase()
        {
            Base a = new Base();
            a.BaseX.x = a00;
            a.BaseX.y = a10;
            a.BaseX.z = a20;
            a.BaseY.x = a01;
            a.BaseY.y = a11;
            a.BaseY.z = a21;
            a.BaseZ.x = a02;
            a.BaseZ.y = a12;
            a.BaseZ.z = a22;
            a.BaseO.x = a03;
            a.BaseO.y = a13;
            a.BaseO.z = a23;
            return a;
        }
       
    }
    /// <summary>
    /// If an object implements the interface ITransform it will be transformated by
    /// calling this interface
    /// </summary>
    public interface ITransform
    {
        /// <summary>
        /// This method is called by a transformation
        /// </summary>
        /// <param name="T"></param>
        void Transform(Matrix T);
    }
    /// <summary>
    /// If an object implements the interface ITransform for 2 dimensions. it will be transformated by
    /// calling this interface
    /// </summary>
    public interface ITransform2d
    {
        /// <summary>
        /// This method is called by a transformation
        /// </summary>
        /// <param name="T"></param>
        void Transform(Matrix3x3 T);
    }
}

