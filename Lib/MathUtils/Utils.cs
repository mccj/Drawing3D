using System.Drawing;
using System;
using System.Globalization;
using System.Collections;

//Copyright (C) 2016 Wolfgang Nagl

// This program is free software; you can redistribute it and/or modify  it under the terms of the GNU General Public License as published by  the Free Software Foundation; either version 2 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU  General Public License for more details.
#if LONGDEF
using IndexType = System.Int32;
#else
using IndexType = System.UInt16;
#endif
namespace Drawing3d
{
    /// <summary>
    /// contains only static members
    /// </summary>
    [Serializable]
    public class Utils
    {
        /// <summary>
        /// Is used from <see cref="Utils.FunSurfaceNurbs"/>
        /// </summary>
        /// <param name="Knots">The knots of the spline</param>
        /// <param name="CtrlPos">the controlpoints</param>
        /// <param name="Degree">the degree</param>
        /// <param name="Periodicity">if the periodicity >100000 (z.B. 2PI), the parameter is taken inside the periodicity </param>
        /// <param name="t">the pameter</param>
        /// <returns></returns>
        public static double[] GetSplineCoeffs(double[] Knots, ref int CtrlPos, int Degree, double Periodicity, double t)
        {

            {
                if ((t > Knots[Knots.Length - 1] + 0.00001) && (Periodicity < 100000))
                {
                    t -= Periodicity;
                    while ((t > Knots[Knots.Length - 1] + 0.00001) && (t > Knots[0]))
                        t -= Periodicity;
                }
                if ((t < Knots[0] - 0.00001) && (Periodicity < 100000))
                {
                    t += Periodicity;
                    while ((t < Knots[0] - 0.000001) && (t < Knots[Knots.Length - 1] + 0.0000001))
                        t += Periodicity;
                }
                if ((t - 0.0001 < Knots[0]) && (t + 0.0001 > Knots[0]))
                    t = Knots[0] + 0.0000000000000001;
                if ((t + 0.0001 > Knots[Knots.Length - 1]) && (t - 0.0001 < Knots[Knots.Length - 1]))

                    t = Knots[Knots.Length - 1] - 0.000000000000001;

                if (t < Knots[0]) t = Knots[0] + 0.00000001;
                if (t > Knots[Knots.Length - 1]) t = Knots[Knots.Length - 1] - 0.00001;

            }
            int first = 0;
            while ((first < Knots.Length) && (t >= Knots[first]))
                first++;
            if (Knots.Length - first < Degree)
                first -= 1;
            CtrlPos = first - Degree - 1;
            if (CtrlPos < 0)
                CtrlPos = 0;

            if (CtrlPos + Degree + 2 >= Knots.Length)
            {
                CtrlPos -= 1;
            }

            double[] Result = _GetSplineBase(Degree + 1, 2 * (Degree + 1), CtrlPos, Knots, t);

            return Result;
        }

        private static double[] _GetSplineBase(int Order, int Count, int Offset, double[] Knots, double t)  // calculate the blending value
        {


            int ct = Count - Order;
            double[] result = new double[ct];
            if (Order == 1)			// base case for the recursion
            {
                for (int i = 0; i < ct; i++)
                    if ((Knots[i + Offset] <= t) && (t < Knots[i + 1 + Offset]))
                        result[i] = 1;
                    else
                        result[i] = 0;

            }
            else
            {
                double[] Base = _GetSplineBase(Order - 1, Count, Offset, Knots, t);
                for (int i = 0; i < ct; i++)
                {
                    if ((Knots[i + Order - 1 + Offset] == Knots[i + Offset]) && (Knots[i + Order + Offset] == Knots[i + 1 + Offset]))  // check for divide by zero
                        result[i] = 0;
                    else
                        if (Knots[i + Order - 1 + Offset] == Knots[i + Offset]) // if a term's denominator is zero,use just the other
                        result[i] = (Knots[i + Order + Offset] - t) / (Knots[i + Order + Offset] - Knots[i + 1 + Offset]) * Base[i + 1];
                    else
                            if (Knots[i + Order + Offset] == Knots[i + 1 + Offset])
                        result[i] = (t - Knots[i + Offset]) / (Knots[i + Order - 1 + Offset] - Knots[i + Offset]) * Base[i];
                    else
                        result[i] =
                            result[i] = (Knots[i + Order + Offset] - t) / (Knots[i + Order + Offset] - Knots[i + 1 + Offset]) * Base[i + 1] +
                             (t - Knots[i + Offset]) / (Knots[i + Order - 1 + Offset] - Knots[i + Offset]) * Base[i];
                }

            }

            return result;
        }
        /// <summary>
        /// Calculates the Union of two rectangles
        /// </summary>
        /// <param name="A">first rectangle</param>
        /// <param name="B">second rectangle</param>
        /// <returns></returns>
        public static System.Drawing.RectangleF Union(System.Drawing.RectangleF A, System.Drawing.RectangleF B)
        { 
            if ((A.Width <= 0) || (A.Height <= 0)) return B;
            if ((B.Width <= 0) || (B.Height <= 0)) return A;
            System.Drawing.RectangleF Result = new System.Drawing.RectangleF();
            double minx = Math.Min(A.Location.X, B.Location.X);
            double miny = Math.Min(A.Location.Y, B.Location.Y);
            double maxx = Math.Max(A.Location.X + A.Size.Width, B.Location.X + B.Size.Width);
            double maxy = Math.Max(A.Location.Y + A.Size.Height, B.Location.Y + B.Size.Height);
            Result.Location = new System.Drawing.PointF((float)minx, (float)miny);
            Result.Size = new System.Drawing.SizeF((float)(maxx - minx), (float)(maxy - miny));

            return Result;
        }
        /// <summary>
        /// Calculates a rectangle belonging to two points
        /// </summary>
        /// <param name="A">first point</param>
        /// <param name="B">second point</param>
        /// <returns></returns>
        public static System.Drawing.RectangleF ToRectangle(xy A, xy B)
        {
            double minx = Math.Min(A.x, B.x);
            double miny = Math.Min(A.y, B.y);
            double maxx = Math.Max(A.x, B.x);
            double maxy = Math.Max(A.y, B.y);
            System.Drawing.RectangleF Result = new System.Drawing.RectangleF();
            Result.Location = new System.Drawing.PointF((float)minx, (float)miny);
            Result.Size = new System.Drawing.SizeF((float)(maxx - minx), (float)(maxy - miny));

            return Result;

        }
        /// <summary>
        /// 
        /// Combines the sorted arrays a and b by a logical "and".
        /// The first member of a (b) toggle the state to true, the second to false and so on.
        /// The result toggle to true only when both a and b have the state true. If one have the state
        /// false then the result toggles to false.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static double[] CombineAnd(double[] a, double[] b)
        {
            ArrayList result = new ArrayList();
            int ia = 0;
            int ib = 0;
            bool aon = false;
            bool bon = false;
            bool ron = false;


            while ((ia < a.Length) && (ib < b.Length))
            {
                if (a[ia] < b[ib])
                {
                    aon = !aon;
                    if (aon && bon && !ron)
                    {
                        result.Add(a[ia]);
                        ron = true;
                    }
                    else
                    {
                        if ((!aon || !bon) && ron)
                        {
                            result.Add(a[ia]);
                            ron = false;
                        }
                    }
                    ia++;
                }
                else
                {
                    bon = !bon;
                    if (aon && bon && !ron)
                    {
                        result.Add(b[ib]);
                        ron = true;
                    }
                    else
                        if ((!aon || !bon) && ron)
                    {
                        result.Add(b[ib]);
                        ron = false;
                    }
                    ib++;
                }
            }

            for (int i = ia; i < a.Length; i++)
            {
                aon = !aon;
                if ((bon) && (aon) && (!ron)) { result.Add(a[i]); ron = true; }
                if ((!aon) && (ron)) { result.Add(a[i]); ron = false; }

            }
            for (int i = ib; i < b.Length; i++)
            {
                bon = !bon;
                if ((bon) && (aon) && (!ron)) { result.Add(b[i]); ron = true; }
                if ((!bon) && (ron)) { result.Add(b[i]); ron = false; }

            }
            double[] r = new double[result.Count];
            for (int i = 0; i < result.Count; i++) r[i] = (double)result[i];
            return r;
        }
        /// <summary>
        /// Calculates the xored value of the Color C with ColorBack.
        /// </summary>
        /// <param name="C"></param>
        /// <param name="ColorBack"></param>
        /// <returns></returns>
        public static Color ColorXor(Color C, Color ColorBack)
        {
            return Color.FromArgb(C.R ^ ColorBack.R, C.G ^ ColorBack.G, C.B ^ ColorBack.B);


        }
        /// <summary>
        ///  represents the format in which a double value is converted to a string by <see cref="FloatToStr"/>
        ///  This format is used by the overridden ToString-method of xyz, xy ..
        ///		 
        ///  
        /// </summary>
        /// 
        public static RectangleF RectNomalize(RectangleF R)
        {
            if (R.Width < 0)
            {
                R.X = R.X + R.Width;
                R.Width = -R.Width;
            }
            if (R.Height < 0)
            {
                R.Y = R.Y + R.Height;
                R.Height = -R.Height;
            }
            return R;

        }
        /// <summary>
        /// This format is taken for all conversions from double to string.
        /// The default is "0.######".
        /// </summary>
        public static string DoubleFormat = "0.######";
        /// <summary>
        ///  Separates the coordiantes of a xyz-point, when it is converted to a string
        ///  Defaultvalue is "/"
        /// </summary>

        public static char Delimiter = '/';

        /// <summary>
        /// Converts a floating value to a string by using <see cref="DoubleFormat"/>
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string FloatToStr(double d)
        {
           
            string s = d.ToString("0.######");
       
             s=s.Replace('.', DecimalSeparator);
            return s.Replace(',', DecimalSeparator);

        }


     
        /// <summary>
        /// Convert a string to float. Decpoint and deccomma are valid separators!
        /// By using this conversion you avoid a lot of troubles.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double StrToFloat(string s)
        {
            s.Replace( DecimalSeparator,'.');
            double c = 0;
            try
            {
                c=  double.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {

                return c;
            }

            return c;
           
        }
      
        /// <summary>
        ///  Returns the binomialcoefficient, with N over K.
        ///  z.b.: N_over_K(6, 2) is 15;
        ///  </summary>
        ///  <param name="N"> degree of the  binomial coefficient </param>
        ///  <param name="K"> index of the  binomial coefficient </param>
        ///  <returns>the binomial coefficient N over K </returns>
        /// 
        /// 
        public static double N_over_K(int N, int K)
        {
            if (K < 0) return 0;
            if (K > N) return 0;
            if ((K == N) || (K == 0)) return 1;
            double nenner = 1;
            double zaehler = 1;
            for (int i = 0; i < K; i++)
            {
                nenner = nenner * (i + 1);
                zaehler = zaehler * (N - i);
            }
            return zaehler / nenner;
        }
        /// <summary>
        /// just the same as System.Math.Sin(Alfa)
        /// </summary>
        /// <param name="Alfa">the angle</param>
        /// <returns>the sinus</returns>
        public static double Sin(double Alfa)
        {
            return System.Math.Sin(Alfa);

        }
        /// <summary>
        /// just the same as System.Math.Cos(Alfa)
        /// </summary>
        /// <param name="Alfa">the angle</param>
        /// <returns>the cosinus</returns>
        public static double Cos(double Alfa)
        {
            return System.Math.Cos(Alfa);

        }
        /// <summary>
        /// Calculates the radian of a degreemeasure
        /// </summary>
        /// <param name="value">measure in degree</param>
        /// <returns></returns>
        public static double DegToRad(double value)
        {
            return (value * System.Math.PI / 180);

        }
        /// <summary>
        ///  Check the point xy and retrieves a rectangle, which contains the rectangle rect and the
        ///  point xyz
        /// </summary>
        /// 
        ///	 <param name="xy"> a point for checking to ly in the rectangle </param>
        ///  <param name="rect"> index of the  binomial coefficient </param>
        ///  <returns>a rectangle which contains rect and xy</returns>
        public static Rectangle MaxRect(Point xy, Rectangle rect)
        {

            return GetMaxRectangle(rect, xy);

        }
        /// <summary>
        /// Oveloaded <see cref="MaxRect(Point,Rectangle)"/> 
        /// </summary>
        /// <param name="xy"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static RectangleF MaxRect(xy xy, RectangleF rect)
        {

            return GetMaxRectangle(rect, xy);

        }
        /// <summary>
        /// Calculates a minimum rectangle containing R1 and R2 
        /// </summary>
        /// <param name="R1">first rectangle</param>
        /// <param name="R2">second rectangle</param>
        /// <returns>minimum rectangle containing R1 and R2</returns>
        public static RectangleF GetMaxRectangle(RectangleF R1, RectangleF R2)
        {
            float left = System.Math.Min(R1.Left, R2.Left);
            float top = System.Math.Min(R1.Top, R2.Top);
            float right = System.Math.Max(R1.Right, R2.Right);
            float bottom = System.Math.Max(R1.Bottom, R2.Bottom);



            return new RectangleF(left, top, right - left, bottom - top);
        }

        /// <summary>
        /// Transforms a rectangle by a transformation.
        /// </summary>
        /// <param name="R">Rectangle, that will be transformed</param>
        /// <param name="T">Transformation</param>
        /// <returns>Transformed ractangle</returns>
        public static RectangleF TransformRectangle(RectangleF R, Matrix3x3 T)
        {
            xy A = new xy(R.X, R.Y);
            xy B = new xy(R.X + R.Width, R.Y + R.Height);
            A = T * A;
            B = T * B;
            return new RectangleF((float)A.x, (float)A.y, (float)(B.x - A.x), (float)(B.y - A.y));
        }
        /// <summary>
        /// Order the values of a rectangle such that left  &lt; right 
        /// and bottom &lt; top
        /// </summary>
        /// <param name="R">Rectangle</param>
        /// <returns>Ordered rectangle</returns>
        public static RectangleF OrderRect(RectangleF R)
        {
            float ax, ay, bx, by;
            ax = System.Math.Min(R.Left, R.Right);
            ay = System.Math.Min(R.Bottom, R.Top);

            bx = System.Math.Max(R.Left, R.Right);
            by = System.Math.Max(R.Bottom, R.Top);

            return new RectangleF(ax, ay, (bx - ax), by - ay);


        }

        /// <summary>
        /// Calculates the angle in radian between two vectors in a counterclockwise sense.
        /// 
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Angle in radian</returns>
        public static double Angle(xy a, xy b)
        {
            return Angle(new xyz(a.x, a.y, 0), new xyz(b.x, b.y, 0), new xyz(0, 0, 1));
        }
        /// <summary>
        /// Calculates the angle in radian between two vectors in a counterclockwise sense.
        /// 
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>Angle in radian</returns>

        public static double Angle(xyz a, xyz b)
        {

            return Angle(a, b, a & b);
        }

        /// <summary>
        /// Sets or gets the DecimalSeparator.
        /// </summary>
        //public static string DecimalSeparator =System.Windows.Forms.Application.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        static int MyProperty { get; set; }
        /// <summary>
        /// Defines the DecimalSeparator. It is per default ="." this means a
        /// decimal number has the string e.g. 4.321
        /// </summary>
        public static char DecimalSeparator = '.';
        /// <summary>
        /// Calculates the angle in radian between two vectors. The orientation of the angle is given by the normalvector.
        /// 
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <param name="normal">Normal vector, which indicates the anglefield</param>
        /// <returns>Angle in radian</returns>
        public static double Angle(xyz a, xyz b, xyz normal)
        {
            b = b.normalized();
            double x = a * b;
            xyz c = a & b;
            if (Utils.Equals(c.length(), 0))
            {

                if (Utils.Less((a * b), 0)) return System.Math.PI;
                else
                    return 0;
            }

            c = c.normalized();
            double y = a * (b & c);
            double result = System.Math.Atan2(y, x);
           
            if (c * normal < 0)
                result = 2 * System.Math.PI - result;

            return result;
        }
        /// <summary>
        ///  Calculates the Bernstein coefficient of degree and index id at the value t.
        ///  This is used for the calculation of Bezierfunction and Meshes
        /// </summary>
        /// <param name="t"> Parameter</param>
        /// <param name="deg"> degree if the Bernstein coefficient </param>
        /// <param name="id"> index of the Bernstein coefficient </param>
        /// <returns>value of the Bernstein coefficient</returns>
        /// <example>
        ///	if A, B, C, D are Points of type xy respectivly xyz then
        ///  b(t) = A * Bernstein(3, 0) + B * Bernstein(3, 1) + C*Bernstein(3, 2) + D * Bernstein(3, 3)
        ///  results the interpolated Beziervalue of degree 3.
        ///  "*" means a multiplication between a Point ( xy or xyz ) and a double value. 
        /// </example>
        public static double Bernstein(int deg, int id, double t)
        {
            if (id < 0) return 0;
            double P1;
            if (Utils.Equals(1 - t, 0) && Utils.Equals(deg - id, 0))
                P1 = 1;
            else
                if (Utils.Equals(1 - t, 0))
                P1 = 0;
            else
                P1 = System.Math.Pow(1 - t, deg - id);


            double P2 = System.Math.Pow(t, id);
            return P1 * P2 * N_over_K(deg, id);
        }
        /// <summary>
        ///  Calculate the derivation of the Bernstein coefficient of degree, index id at the value t.
        /// </summary>
        ///  <param name="deg"> degree if the Bernstein coefficient </param>
        ///  <param name="id"> index of the Bernstein coefficient </param>
        ///  <param name="t"> argument for which the Bernstein coefficient is calulated</param>
        ///  <returns>derivation of the Bernstein coefficient</returns>
        public static double BernsteinDerive(int deg, int id, double t)
        {

            return deg * (Bernstein(deg - 1, id - 1, t) - Bernstein(deg - 1, id, t));
        }
        /// <summary>
        ///  retrieves the value of Bezierfunction at t.
        ///  The degree is given by the count of "Points" in the array "Points".
        /// </summary>
        ///  <param name="Points"> control points of the Bezierfunction. The count determinates the degree </param>
        ///  <param name="t">is the value for which the calculation is done  </param>
        ///  <returns>vaule of the Bezierfunction </returns>
        ///   <example> 
        ///  xy[] Points = new xy[3];
        ///  xy[0] = new xy(0, 0);
        ///  xy[1] = new xy(1, 3);
        ///  xy[2] = new xy(4, 3);
        ///  xy[3] = new xy(6, 0);
        ///  xy p = funBezier ( Points , 0.4 );
        ///  </example>
        public static xy funBezier(xy[] Points, double t)
        {

            int N = Points.Length;
            if (N == 4)
            {
                double s = 1 - t;
                xy res;

                /* Note: a good optimizing compiler (such as gcc-3) reduces the
                   following to 16 multiplications, using common subexpression
                   elimination. */

                res.x = s * s * s * Points[0].x + 3 * (s * s * t) * Points[1].x + 3 * (t * t * s) * Points[2].x + t * t * t * Points[3].x;
                res.y = s * s * s * Points[0].y + 3 * (s * s * t) * Points[1].y + 3 * (t * t * s) * Points[2].y + t * t * t * Points[3].y;

                return res;


            }
            xy result = new xy(0, 0);
            for (int i = 0; i < N; i++)
                result = result + Points[i] * Bernstein(N - 1, i, t);
            return result;

        }
        /// <summary>
        /// Defines a structure for the calculations of spline. This structure contains the value and the derivation
        /// of the spline <seealso cref="SplineBaseEx"/>
        /// </summary>
        public struct SplineExValue
        {
            /// <summary>
            /// value of the spline
            /// </summary>
            public double Value;
            /// <summary>
            /// Derivation of the spline
            /// </summary>
            public double Derivation;
            /// <summary>
            /// The second derivation
            /// </summary>
            public double SecondDerivation1;
        }
        /// <summary>
        /// is called in the spline and nurbs calculation
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="PointCount">Count of controlpoints</param>
        /// <param name="Degree">the degree</param>
        /// <param name="t">parameter</param>
        /// <returns>Array of coefficients</returns>
        public static double[] UniformCoeffSpline(double[] Knots, int PointCount, int Degree, double t)
        {
            t = Knots[0] + t * (Knots[Knots.Length - 1] - Knots[0]) * 0.9999999;
            if (t == Knots[0])
                t = Knots[0] + 0.000001;
            return _SplineBase(Degree + 1, Knots, t);
        }
        /// <summary>
        /// clamps the t to the area of knots, if the difference to a knot is very small
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="PointCount">cont of control points</param>
        /// <param name="Degree">degree</param>
        /// <param name="Periodicity">periodicity</param>
        /// <param name="t">parameter</param>
        /// <returns>the clamped t</returns>
        public static double ClampToArea(double[] Knots, int PointCount, int Degree, double Periodicity, double t)
        {
            if ((t - 0.0001 < Knots[0]) && (t + 0.0001 > Knots[0]))
                t = Knots[0] + 0.00001;

            if ((t + 0.0001 > Knots[Knots.Length - 1]) && (t - 0.0001 < Knots[Knots.Length - 1]))

                t = Knots[Knots.Length - 1] - 0.000000000000001;
            if (Periodicity > 0)
            {
                if ((t > Knots[Knots.Length - 1] + 0.0000000000001) && (Periodicity < 100000))
                {
                    t -= Periodicity;
                    while ((t > Knots[Knots.Length - 1] + 0.000000001) && (t > Knots[0]))
                        t -= Periodicity;
                }

                if ((t < Knots[0] - 0.0000001) && (Periodicity < 100000))
                {
                    t += Periodicity;
                    while ((t < Knots[0] - 0.000001) && (t < Knots[Knots.Length - 1] + 0.0000001))
                        t += Periodicity;
                }
            }
            if (t < Knots[0]) t = Knots[0] + 0.00000001;
            if (t > Knots[Knots.Length - 1]) t = Knots[Knots.Length - 1] - 0.00001;
            return t;
        }
        /// <summary>
        /// Returns the spline coefficients for a given parameter t from the interval[0,1] a knotsarray, a count of controlpoins and the degree
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="PointCount">Count of controlpoints</param>
        /// <param name="Periodicity">Periodicity of the spline. If the spline has no periodicity take 100000</param>
        /// <param name="Degree">the Degree of the spline</param>
        /// <param name="t">parameter from the interval[0,1]</param>
        /// <returns></returns>
        public static double[] CoeffSpline(double[] Knots, int PointCount, int Degree, double Periodicity, double t)
        {
           if ((Periodicity < 100000) && (Periodicity > 0))

                while (t > Knots[Knots.Length - 1])
                    t -= Periodicity;
            if ((Periodicity < 100000) && (Periodicity > 0))
            {
                while (t < Knots[0])
                    t += Periodicity;
                if ((t < Knots[0] - Periodicity / 2) && (Periodicity < 100000))
                    t += Periodicity;
            }
            if ((t - 0.001 < Knots[Degree - 1]) && (t + 0.001 > Knots[Knots.Length - Degree]))
                t = Knots[Degree - 1] + 0.001;
            if ((t + 0.00001 > Knots[Knots.Length - 1]) && (t - 0.00001 < Knots[Knots.Length - Degree]))

                t = Knots[Knots.Length - Degree] - 0.0000000000001;

            if (t < Knots[Degree - 1])
                t = Knots[Degree - 1] + 0.00000000000001;
            if (t > Knots[Knots.Length - Degree])
                t = Knots[Knots.Length - Degree];


            return _SplineBase(Degree + 1, Knots, t);

        }

        /// <summary>
        /// Returns the spline coefficients and the derivated spline coefficients for a given parameter t from the interval[0,1] a knotsarray, a count of controlpoins and the degree
        /// <seealso cref="CoeffSpline"/>
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="PointCount">Count of controlpoints</param>
        /// <param name="Degree">the Degree of the spline</param>
        /// <param name="t">parameter from the interval[0,1]</param>
        /// <param name="Periodicity">periodicity</param>
        /// <returns></returns>
        public static SplineExValue[] CoeffSplineEx(double[] Knots, int PointCount, int Degree, double Periodicity, double t)
        {
            //  double a = ClampToArea(Knots, PointCount, Degree, Periodicity, t);
            if (Math.Abs(t) < 0.00001)
                t = Knots[0]+0.00001;
            if (Math.Abs(t) > 0.99999)
                t = Knots[Knots.Length-1]-0.0001;
            return SplineBaseEx(Degree + 1, Knots, t);

        }
 
        /// <summary>
        /// Returns an array of ControlPointCount + Degree + 1 knots, which are not uniform.
        /// Example:
        /// KNOTS={0,0,0,0,1,2,2,2,2} for ControlPointCount=5 and Degree =3.
        /// </summary>
        public static double[] DefaultKnots(int ControlPointCount, int Degree)
        {
            double[] Knots = new double[ControlPointCount + Degree + 1];


            int ct = Knots.Length;
            for (int j = 0; j < ct / 2; j++)
            {
                Knots[j] = 0;
                Knots[ct - 1 - j] = 1;
           }
            if ((ct - 1) / 2 * 2 == ct - 1)
                Knots[(ct - 1) / 2] = 0.5;

            return Knots;
        }
        /// <summary>
        /// Is the same as <see cref="StandardKnots(IndexType, IndexType, double, double)"/> with from=0 and to = 1.
        /// Calculate knots for use of <see cref="QSpline"/> and Nurbs
        /// The first Degree+1 and the last degree+1 ements of the result are set to 0 resp. 1. The other elements are equidistant.
        /// E.g. Utils.StandardKnots(6, 3) gets an array [0,0,0,0,0.33333333333333,0.6666666666666,1,1,1,1]
        /// </summary>
        /// <param name="ControlPointCount">Count of controlpoints (must be greather then the degree)</param>
        /// <param name="Degree">the degree</param>
        /// <returns>Knots</returns>
        public static double[] StandardKnots(int ControlPointCount, int Degree)
        {

            double[] Knots = new double[ControlPointCount + Degree + 1];
            for (int i = 0; i < Degree + 1; i++)
            {
                Knots[i] = 0;
            }
            int k = 1;
            for (int i = Degree + 1; i < ControlPointCount; i++)
            {
                Knots[i] = k * (float)1 / ((float)ControlPointCount - Degree);
                k++;
            }

            for (int i = ControlPointCount; i < Degree + ControlPointCount + 1; i++)
            {
                Knots[i] = 1;
            }
            return Knots;


        }
        /// <summary>
        /// Calculate knots for use of <see cref="QSpline"/> and Nurbs
        /// The first Degree+1 and the last degree+1 ements of the result are set to from resp. to. The other elements are equidistant.
        /// E.g. Utils.StandardKnots(6, 3,3,9) gets an array [3,3,3,3,5,7,9,9,9,9]
        /// </summary>
        /// <param name="ControlPointCount">Count of controlpoints (must be greather then the degree)</param>
        /// <param name="Degree">the degree</param>
        /// <param name="from">the smallest knot</param>
        /// <param name="to">the greatest knot</param>
        /// <returns>Knots</returns>
        public static double[] StandardKnots(int ControlPointCount, int Degree, double from, double to)
        {
            if (from > to)
            {
                double c = from;
                from = to;
                to = c;
                c = System.Math.Abs(from);
                from += c;
                to += c;
            }
            double[] Knots = new double[ControlPointCount + Degree + 1];
            for (int i = 0; i < Degree + 1; i++)
            {
                Knots[i] = from;
            }
            int k = 1;
            for (int i = Degree + 1; i < ControlPointCount; i++)
            {
                Knots[i] = from + k * (float)(to - from) / ((float)ControlPointCount - Degree);
                k++;
            }

            for (int i = ControlPointCount; i < Degree + ControlPointCount + 1; i++)
            {
                Knots[i] = to;
            }
            return Knots;
        }
        /// <summary>
        /// Returns an array of ControlPointCount + Degree + 1 knots, which are uniform.
        /// Example:
        /// KNOTS={0,0.125,0.25,0.375,0.5,0.625,0.75,0.875,1} for ControlPointCount=5 and Degree =3.
        /// 
        /// </summary>
        /// <param name="ControlPointCount">Count of the Controlpoints</param>
        /// <param name="Degree">Degree of the spline</param>
        /// <returns>Knotvector</returns>
        public static double[] UniformKnots(int ControlPointCount, int Degree)
        {
            double[] Knots = new double[ControlPointCount + Degree + 1];
            for (int j = 0; j < ControlPointCount + Degree + 1; j++)
                Knots[j] = (float)j / ((float)ControlPointCount + Degree);

            return Knots;

        }
        /// <summary>
        /// Calculates the 3-dimensional nurbs. It returns a point to a given parameter t from the interval[0,1], the knots,the weights, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="Weights">Weights</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns></returns>
        public static xyz funNurbs3d(double[] Knots, double[] Weights, xyz[] ControlPoints, int Degree, double t)
        {


            xyz Result = new xyz(0, 0, 0);

            double[] Coeffs = UniformCoeffSpline(Knots, ControlPoints.Length, Degree, t);
            double W = 0;
            for (int i = 0; i < ControlPoints.Length; i++)
                W = W + Coeffs[i] * Weights[i];
            if (Utils.Equals(W, 0))
            {
                for (int i = 0; i < ControlPoints.Length; i++)
                    Result = Result + ControlPoints[i] * (Coeffs[i]);
            }
            else
            {
                for (int i = 0; i < ControlPoints.Length; i++)
                    Result = Result + ControlPoints[i] * (Coeffs[i] * Weights[i] / W);
            }
            return Result;
        }
        /// <summary>
        /// Calculates the 2-dimensional nurbs. It returns a point to a given parameter t from the interval[0,1], the knots,the weights, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="Weights">Weights</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns></returns>
        public static xy funNurbs2d(double[] Knots, double[] Weights, xy[] ControlPoints, int Degree, double t)
        {


            xy Result = new xy(0, 0);
            double[] Coeffs = UniformCoeffSpline(Knots, ControlPoints.Length, Degree, t);



            int StartId = -1;
            int S2 = 0;
            for (int i = 0; i < Knots.Length - 1; i++)
            {
                if ((Knots[i] < t) && (t <= Knots[i + 1]))
                {
                    StartId = i;
                    S2 = i;
                    break;
                }
            }
            if (S2 + Degree > Knots.Length)
            {
                S2 = Knots.Length - Degree;
            }
            double[] K = new double[Degree];
            for (int i = 0; i < Degree; i++)
            {
                K[i] = Knots[i + S2];
            }
            if (StartId >= Degree) StartId -= (Degree);
            else
                StartId = 0;
            double W = 0;
            for (int i = 0; i < ControlPoints.Length; i++)
                W = W + Coeffs[i] * Weights[i];

            if (W == 0)
            {
                for (int i = 0; i < ControlPoints.Length; i++)
                    //Result = Result + ControlPoints[i] * (Coeffs[i] * Weights[i] / W);
                    Result = Result + ControlPoints[i] * Coeffs[i];
            }
            else
            {
                for (int i = 0; i < ControlPoints.Length; i++)
                    //Result = Result + ControlPoints[i] * (Coeffs[i] * Weights[i] / W);
                    Result = Result + ControlPoints[i] * (Coeffs[i] * Weights[i] / W);
            }
            return Result;
        }

        /// <summary>
        /// Calculates the 2-dimensional spline. It returns a point to a given parameter t from the interval[0,1], the knots, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="KT"><see cref="KnotType"/></param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns>2-dim point</returns>
        public static xy funBSpline2d(double[] Knots, xyArray ControlPoints, int Degree, double t, KnotType KT)
        {
            if (KT == KnotType.PiecewiseBezier)
                return funBezier(ControlPoints.data, t);

            xy Result = new xy(0, 0);
            double[] Coeffs = null;
            if ((KT == KnotType.Uniform) || (KT == KnotType.Unspecified) || (KT == KnotType.QuasiUniform))
                Coeffs = UniformCoeffSpline(Knots, ControlPoints.Count, Degree, t);
            else
                Coeffs = CoeffSpline(Knots, ControlPoints.Count, Degree, 1000, t);

            for (int i = 0; i < ControlPoints.Count; i++)
                Result = Result + ControlPoints[i] * Coeffs[i];
            return Result;
        }

        /// <summary>
        /// Calculates the derivation of 3-dimensional spline. It returns a vactor to a given parameter t from the interval[0,1], the knots, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <param name="KT"><see cref="KnotType"/></param>
        /// <returns>3-dim point</returns>
        public static xyz funBSpline3d(double[] Knots, xyz[] ControlPoints, int Degree, double t, KnotType KT)
        {

            if (t <= Knots[0]) t = Knots[0] + 0.0001 * (Knots[Knots.Length - 1] - Knots[0]);
            if (t >= Knots[Knots.Length - 1]) t = Knots[Knots.Length - 1] - 0.0001 * (Knots[Knots.Length - 1] - Knots[0]);
            xyz Result = new xyz(0, 0, 0);
            double[] Coeffs = null;
            if ((KT == KnotType.Uniform) || (KT == KnotType.QuasiUniform) || (KT == KnotType.Unspecified))
                Coeffs = UniformCoeffSpline(Knots, ControlPoints.Length, Degree, t);
            else
                Coeffs = CoeffSpline(Knots, ControlPoints.Length, Degree, 1000000, t);

            for (int i = 0; i < ControlPoints.Length; i++)
                Result = Result + ControlPoints[i] * Coeffs[i];
            if (Result == new xyz(0, 0, 0))
            {
            }
            return Result;
        }

        /// <summary>
        /// Calculates the 3-dimensional spline. It returns a point to a given parameter t from the interval[0,1], the knots, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns>3-dim point</returns>
        public static xyz funBSplineDerived3d(double[] Knots, xyz[] ControlPoints, int Degree, double t)
        {
            xyz Result = new xyz(0, 0, 0);
            SplineExValue[] Coeffs = CoeffSplineEx(Knots, ControlPoints.Length, Degree, 1000, t);

            for (int i = 0; i < ControlPoints.Length; i++)
                Result = Result + ControlPoints[i] * Coeffs[i].Derivation;
            return Result;
        }
        /// <summary>
        /// Calculates the 2-dimensional spline. It returns a point to a given parameter t from the interval[0,1], the knots, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns>3-dim point</returns>
        public static xy funBSplineDerived2d(double[] Knots, xy[] ControlPoints, int Degree, double t)
        {
            xy Result = new xy(0, 0);
            SplineExValue[] Coeffs = CoeffSplineEx(Knots, ControlPoints.Length, Degree, 1000, t);

            for (int i = 0; i < ControlPoints.Length; i++)
                Result = Result + ControlPoints[i] * Coeffs[i].Derivation;
            return Result;
        }
 
        /// <summary>
        /// Calculates the the derivation of a 2-dimensional nurbs. It returns a tangent vector to a given parameter t from the interval[0,1], the knots,the weights, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="Weights">Weights</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns></returns>
        public static xy funNurbsDerive2d(double[] Knots, double[] Weights, xy[] ControlPoints, int Degree, double t)
        {
            int k;
            t = Knots[Degree] + t * (Knots[ControlPoints.Length] - Knots[Degree]) * 0.99999;

            SplineExValue[] Coeffs = SplineBaseEx(Degree + 1, Knots, t);
            double WDerived = 0;
            double W = 0;
            for (k = 0; k < ControlPoints.Length; k++)
            {

                WDerived = WDerived + Weights[k] * Coeffs[k].Derivation;
                W = W + Weights[k] * Coeffs[k].Value;
            }


            xy Result = new xy();
            for (k = 0; k < ControlPoints.Length; k++)
            {
                xy T = (ControlPoints[k] * Weights[k] * ((W * Coeffs[k].Derivation - Coeffs[k].Value * WDerived) / (W * W)));
                Result = Result + T;

            }

            return Result;
        }

        /// <summary>
        /// Calculate a <see cref="Nurbs3d"/> point belonging to the parameter u and v.
        /// See <see cref="Surface"/>
        /// </summary>
        /// <param name="UKnots">UKnots</param>
        /// <param name="VKnots">VKnots</param>
        /// <param name="Weights">Weights</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="UDegree">UDegree</param>
        /// <param name="VDegree">VDegree</param>
        /// <param name="u">u parameter between 0 ant 1</param>
        /// <param name="v">v parameter between 0 ant 1</param>
        /// <returns>point on a nurbs3d</returns>
        public xyz FunSurfaceNurbs(double[] UKnots, double[] VKnots, double[,] Weights, xyz[,] ControlPoints, int UDegree, int VDegree, double u, double v)
        {

            xyz Result = new xyz(0, 0, 0);
            if ((UKnots == null) || (VKnots == null)) return Result;




            int UOffset = -1;
            int VOffset = -1;
            double[] UCoeff = Utils.GetSplineCoeffs(UKnots, ref UOffset, UDegree, 10000000, u);
            double[] VCoeff = Utils.GetSplineCoeffs(VKnots, ref VOffset, VDegree, 10000000, v);

            try
            {

                double W1 = 0;
                try
                {


                    int VCoeffIndex = 0;
                    int uCoeffIndex = 0;
                    for (int i = UOffset; i < UOffset + UDegree + 1; i++)
                    {
                        VCoeffIndex = 0;
                        for (int j = VOffset; j < VOffset + VDegree + 1; j++)
                        {
                            W1 = W1 + Weights[i, j] * UCoeff[uCoeffIndex] * VCoeff[VCoeffIndex];// *VCoeffDerived[j].Value;
                            VCoeffIndex++;
                        }
                        uCoeffIndex++;
                    }
                }
                catch (Exception)
                {


                }
                int uCoeffI = 0;
                int VCoeffI = 0;
              
                for (int i = UOffset; i < UOffset + UDegree + 1; i++)
                {
                    VCoeffI = 0;

                    for (int k = VOffset; k < VOffset + VDegree + 1; k++)
                    {
                        Result = Result + ControlPoints[i, k] * (UCoeff[uCoeffI] * VCoeff[VCoeffI]
                        * Weights[i, k] / W1);
                        VCoeffI++;
                    }
                    uCoeffI++;

                }
            }
            catch (Exception)
            {


            }
            return Result;
        }

        /// <summary>
        /// Calculates the the derivation of a 3-dimensional nurbs. It returns a tangent vector to a given parameter t from the interval[0,1], the knots,the weights, the degree and the controlpoints
        /// </summary>
        /// <param name="Knots">Knots</param>
        /// <param name="Weights">Weights</param>
        /// <param name="ControlPoints">ControlPoints</param>
        /// <param name="Degree">Degree</param>
        /// <param name="t">Parameter from the interval[0,1]</param>
        /// <returns></returns>
        public static xyz funNurbsDerive3d(double[] Knots, double[] Weights, xyz[] ControlPoints, int Degree, double t)
        {
            int k;
            t = Knots[Degree] + t * (Knots[ControlPoints.Length] - Knots[Degree]) * 0.999999999999;

            SplineExValue[] Coeffs = SplineBaseEx(Degree + 1, Knots, t);
            double WDerived = 0;
            double W = 0;
            for (k = 0; k < ControlPoints.Length; k++)
            {

                WDerived = WDerived + Weights[k] * Coeffs[k].Derivation;
                W = W + Weights[k] * Coeffs[k].Value;
            }


            xyz Result = new xyz();
            for (k = 0; k < ControlPoints.Length; k++)
            {
                xyz T = (ControlPoints[k] * Weights[k] * ((W * Coeffs[k].Derivation - Coeffs[k].Value * WDerived) / (W * W)));
                Result = Result + T;

            }

            return Result;
        }
        private static double[] _SplineBase(int Order, double[] Knots, double t)  // calculate the blending value
        {


            int ct = Knots.Length - Order;
            double[] result = new double[ct];
            try
            {

                if (Order == 1)			// base case for the recursion
                {
                    for (int i = 0; i < ct; i++)
                        if ((Knots[i] <= t) && (t < Knots[i + 1]))
                            result[i] = 1;
                        else
                            result[i] = 0;

                }
                else
                {
                    double[] Base = _SplineBase(Order - 1, Knots, t);
                    for (int i = 0; i < ct; i++)
                    {
                        if ((Knots[i + Order - 1] == Knots[i]) && (Knots[i + Order] == Knots[i + 1]))  // check for divide by zero
                            result[i] = 0;
                        else
                            if (Knots[i + Order - 1] == Knots[i]) // if a term's denominator is zero,use just the other
                            result[i] = (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * Base[i + 1];
                        else
                                if (Knots[i + Order] == Knots[i + 1])
                            result[i] = (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * Base[i];
                        else
                            result[i] =
                                result[i] = (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * Base[i + 1] +
                                 (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * Base[i];
                    }

                }
            }
            catch (Exception)
            {


            }
            return result;
        }
      
      
    
        /// <summary>
        /// calculate an array of <see cref="SplineExValue"/> to a given parameter t
        /// </summary>
        /// <param name="Order">degree</param>
        /// <param name="Knots">knots</param>
        /// <param name="t">parameter</param>
        /// <returns>array of <see cref="SplineExValue"/></returns>
        public static SplineExValue[] SplineBaseEx(int Order, double[] Knots, double t)  // calculate Value and Derivation
        {
            int ct = Knots.Length - Order;
            SplineExValue[] result = new SplineExValue[ct];
            if (Order == 1)			// base case for the recursion
            {
                for (int i = 0; i < ct; i++)
                    if ((Knots[i] <= t) && (t <= Knots[i + 1]))
                    {
                        result[i].Value = 1;
                        result[i].Derivation = 0;

                    }
                    else
                    {
                        result[i].Value = 0;
                        result[i].Derivation = 0;

                    }
            }
            else
            {
                SplineExValue[] Base = SplineBaseEx(Order - 1, Knots, t);
                for (int i = 0; i < ct; i++)
                {
                    if ((Knots[i + Order - 1] == Knots[i]) && (Knots[i + Order] == Knots[i + 1]))  // check for divide by zero
                    {
                        result[i].Derivation = 0;
                        result[i].Value = 0;

                    }
                    else
                        if (Knots[i + Order - 1] == Knots[i]) // if a term's denominator is zero,use just the other
                    {
                        SplineExValue C = Base[i + 1];
                        result[i].Value = (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * C.Value;
                        result[i].Derivation = (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * C.Derivation - C.Value / (Knots[i + Order] - Knots[i + 1]);
                    }

                    else
                            if (Knots[i + Order] == Knots[i + 1])
                    {
                        SplineExValue C = Base[i];
                        result[i].Value = (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * C.Value;
                        result[i].Derivation = (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * C.Derivation + C.Value / (Knots[i + Order - 1] - Knots[i]);

                    }
                    else
                    {


                        Utils.SplineExValue C1 = Base[i];
                        Utils.SplineExValue C2 = Base[i + 1];
                        result[i].Value = (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * C1.Value +
                            (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * C2.Value;




                        result[i].Derivation = (t - Knots[i]) / (Knots[i + Order - 1] - Knots[i]) * C1.Derivation +
                            (Knots[i + Order] - t) / (Knots[i + Order] - Knots[i + 1]) * C2.Derivation
                            + C1.Value / (Knots[i + Order - 1] - Knots[i])
                            - C2.Value / (Knots[i + Order] - Knots[i + 1])
                            ;
                    }
                }
            }
            return result;
        }
        private static SplineExValue _SplineBaseEx(int k, int Order, double[] Knots, double t)  // calculate Value and Derivation
        {
            SplineExValue Result = new SplineExValue();
            if (Order == 1)			// base case for the recursion
            {
                Result.Derivation = 0;
                if ((Knots[k] <= t) && (t <= Knots[k + 1]))
                    Result.Value = 1;
                else

                    Result.Value = 0;
            }
            else
            {
                if ((Knots[k + Order - 1] == Knots[k]) && (Knots[k + Order] == Knots[k + 1]))  // check for divide by zero
                {
                    Result.Derivation = 0;
                    Result.Value = 0;

                }
                else
                    if (Knots[k + Order - 1] == Knots[k]) // if a term's denominator is zero,use just the other
                {
                    SplineExValue C = _SplineBaseEx(k + 1, Order - 1, Knots, t);
                    Result.Value = (Knots[k + Order] - t) / (Knots[k + Order] - Knots[k + 1]) * C.Value;
                    Result.Derivation = (Knots[k + Order] - t) / (Knots[k + Order] - Knots[k + 1]) * C.Derivation - C.Value / (Knots[k + Order] - Knots[k + 1]);
                }

                else
                        if (Knots[k + Order] == Knots[k + 1])
                {
                    SplineExValue C = _SplineBaseEx(k, Order - 1, Knots, t);
                    Result.Value = (t - Knots[k]) / (Knots[k + Order - 1] - Knots[k]) * C.Value;
                    Result.Derivation = (t - Knots[k]) / (Knots[k + Order - 1] - Knots[k]) * C.Derivation + C.Value / (Knots[k + Order - 1] - Knots[k]);

                }
                else
                {
                    SplineExValue C1 = _SplineBaseEx(k, Order - 1, Knots, t);
                    SplineExValue C2 = _SplineBaseEx(k + 1, Order - 1, Knots, t);
                    Result.Value = (t - Knots[k]) / (Knots[k + Order - 1] - Knots[k]) * C1.Value +
                        (Knots[k + Order] - t) / (Knots[k + Order] - Knots[k + 1]) * C2.Value;
                    Result.Derivation = (t - Knots[k]) / (Knots[k + Order - 1] - Knots[k]) * C1.Derivation +
                        (Knots[k + Order] - t) / (Knots[k + Order] - Knots[k + 1]) * C2.Derivation
                        + C1.Value / (Knots[k + Order - 1] - Knots[k])
                        - C2.Value / (Knots[k + Order] - Knots[k + 1])
                        ;
                }
            }
            return Result;
        }

      
        /// <summary>
        /// Calculates a Bezier surface. It returns for to parameter u,v from [0,1] a 3D-Point. The array Points contains
        /// the Controlpoints.
        /// Hint : if the dimension of Points is greather than 4, a zero point will be returned.
        /// </summary>
        /// <param name="Points">Controlpoints</param>
        /// <param name="u">Parameter from [0,1]</param>
        /// <param name="v">Parameter from [0,1]</param>
        /// <returns>A 3D-Point on a Bezier surface</returns>
        public static xyz BezierSurfacePt(xyz[,] Points, double u, double v)
        {

            double s = 1 - v;
            double t = v;

            double m = 1 - u;
            double n = u;
            if ((Points.GetLength(0) == 3) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * s * m * m * m + Points[1, 0] * 2 * (s * t) * m * m * m + Points[2, 0] * (t * t) * m * m * m +
                   Points[0, 1] * s * s * 3 * (m * m * n) + Points[1, 1] * 2 * (s * t) * 3 * (m * m * n) + Points[2, 1] * (t * t) * 3 * (m * m * n) +
                   Points[0, 2] * s * s * 3 * (m * n * n) + Points[1, 2] * 2 * (s * t) * 3 * (m * n * n) + Points[2, 2] * (t * t) * 3 * (m * n * n) +
                   Points[0, 3] * s * s * n * n * n + Points[1, 3] * 2 * (s * t) * n * n * n + Points[2, 3] * (t * t) * n * n * n;



            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * s * s * m * m * m + Points[1, 0] * 3 * (s * s * t) * m * m * m + Points[2, 0] * 3 * (t * t * s) * m * m * m + Points[3, 0] * t * t * t * m * m * m +
                       Points[0, 1] * s * s * s * 3 * (m * m * n) + Points[1, 1] * 3 * (s * s * t) * 3 * (m * m * n) + Points[2, 1] * 3 * (t * t * s) * 3 * (m * m * n) + Points[3, 1] * t * t * t * 3 * (m * m * n) +
                       Points[0, 2] * s * s * s * 3 * (m * n * n) + Points[1, 2] * 3 * (s * s * t) * 3 * (m * n * n) + Points[2, 2] * 3 * (t * t * s) * 3 * (m * n * n) + Points[3, 2] * t * t * t * 3 * (m * n * n) +
                       Points[0, 3] * s * s * s * n * n * n + Points[1, 3] * 3 * (s * s * t) * n * n * n + Points[2, 3] * 3 * (t * t * s) * n * n * n + Points[3, 3] * t * t * t * n * n * n;
            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 3))
                return Points[0, 0] * s * s * s * m * m + Points[1, 0] * 3 * (s * s * t) * m * m + Points[2, 0] * 3 * (t * t * s) * m * m + Points[3, 0] * t * t * t * m * m +
                       Points[0, 1] * s * s * s * 2 * (m * n) + Points[1, 1] * 3 * (s * s * t) * 2 * (m * n) + Points[2, 1] * 3 * (t * t * s) * 2 * (m * n) + Points[3, 1] * t * t * t * 2 * (m * n) +
                       Points[0, 2] * s * s * s * (n * n) + Points[1, 2] * 3 * (s * s * t) * (n * n) + Points[2, 2] * 3 * (t * t * s) * (n * n) + Points[3, 2] * t * t * t * (n * n);

            return new xyz(0, 0, 0);

        }
        /// <summary>
        /// Calculates the partial v-derivation of a Bezier surface. It returns for to parameter u,v from [0,1] a vector. The array Points contains
        /// the Controlpoints.
        /// Hint : if the dimension of Points is greather than 4, a zero point will be returned.
        /// </summary>
        /// <param name="Points">Controlpoints</param>
        /// <param name="u">Parameter from [0,1]</param>
        /// <param name="v">Parameter from [0,1]</param>
        /// <returns>A 3D-Point on a Bezier surface</returns>
        public static xyz BezierSurfaceDeriveV(xyz[,] Points, double u, double v)
        {

            double s = 1 - u;
            double t = u;

            double m = 1 - v;
            double n = v;
            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 3))
                return Points[0, 0] * s * s * (-3) * m * m + Points[1, 0] * 3 * ((-2) * s * t + s * s) * m * m + Points[2, 0] * 3 * (2 * t * s - t * t) * m * m + Points[3, 0] * (3 * t * t) * m * m +
                       Points[0, 1] * s * s * (-3) * 2 * (m * n) + Points[1, 1] * 3 * ((-2) * s * t + s * s) * 2 * (m * n) + Points[2, 1] * 3 * (2 * t * s - t * t) * 2 * (m * n) + Points[3, 1] * (3 * t * t) * 2 * (m * n) +
                       Points[0, 2] * s * s * (-3) * (n * n) + Points[1, 2] * 3 * ((-2) * s * t + s * s) * (n * n) + Points[2, 2] * 3 * (2 * t * s - t * t) * (n * n) + Points[3, 2] * (3 * t * t) * (n * n);

            if ((Points.GetLength(0) == 3) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * (-2) * m * m * m + Points[1, 0] * 2 * ((-1) * t + s) * m * m * m + Points[2, 0] * (2 * t) * m * m * m +
                       Points[0, 1] * s * (-2) * 3 * (m * m * n) + Points[1, 1] * 2 * ((-1) * t + s) * 3 * (m * m * n) + Points[2, 1] * (2 * t) * 3 * (m * m * n) +
                       Points[0, 2] * s * (-2) * 3 * (m * n * n) + Points[1, 2] * 2 * ((-1) * t + s) * 3 * (m * n * n) + Points[2, 2] * (2 * t) * 3 * (m * n * n) +
                       Points[0, 3] * s * (-2) * n * n * n + Points[1, 3] * 2 * ((-1) * t + s) * n * n * n + Points[2, 3] * (2 * t) * n * n * n;


            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * s * (-3) * m * m * m + Points[1, 0] * 3 * ((-2) * s * t + s * s) * m * m * m + Points[2, 0] * 3 * (2 * t * s - t * t) * m * m * m + Points[3, 0] * (3 * t * t) * m * m * m +
                       Points[0, 1] * s * s * (-3) * 3 * (m * m * n) + Points[1, 1] * 3 * ((-2) * s * t + s * s) * 3 * (m * m * n) + Points[2, 1] * 3 * (2 * t * s - t * t) * 3 * (m * m * n) + Points[3, 1] * (3 * t * t) * 3 * (m * m * n) +
                       Points[0, 2] * s * s * (-3) * 3 * (m * n * n) + Points[1, 2] * 3 * ((-2) * s * t + s * s) * 3 * (m * n * n) + Points[2, 2] * 3 * (2 * t * s - t * t) * 3 * (m * n * n) + Points[3, 2] * (3 * t * t) * 3 * (m * n * n) +
                       Points[0, 3] * s * s * (-3) * n * n * n + Points[1, 3] * 3 * ((-2) * s * t + s * s) * n * n * n + Points[2, 3] * 3 * (2 * t * s - t * t) * n * n * n + Points[3, 3] * (3 * t * t) * n * n * n;

           
            return new xyz(0, 0, 0);
        }
        /// <summary>
        /// Calculates the partial u-derivation of a Bezier surface. It returns for to parameter u,v from [0,1] a vector. The array Points contains
        /// the Controlpoints.
        /// Hint : if the dimension of Points is greather than 4, a zero point will be returned.
        /// </summary>
        /// <param name="Points">Controlpoints</param>
        /// <param name="u">Parameter from [0,1]</param>
        /// <param name="v">Parameter from [0,1]</param>
        /// <returns>A 3D-Point on a Bezier surface</returns>
        public static xyz BezierSurfaceDeriveU(xyz[,] Points, double u, double v)
        {

            double s = 1 - v;
            double t = v;

            double m = 1 - u;
            double n = u;

            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 3))
                return Points[0, 0] * s * s * s * m * (-2) + Points[1, 0] * 3 * (s * s * t) * m * (-2) + Points[2, 0] * 3 * (t * t * s) * m * (-2) + Points[3, 0] * t * t * t * m * (-2) +
                  Points[0, 1] * s * s * s * 3 * ((-1) * n + m) + Points[1, 1] * 3 * (s * s * t) * 3 * ((-1) * n + m) + Points[2, 1] * 3 * (t * t * s) * 3 * ((-1) * n + m) + Points[3, 1] * t * t * t * 3 * ((-1) * n + m) +
                  Points[0, 2] * s * s * s * 3 * (2 * n) + Points[1, 2] * 3 * (s * s * t) * 3 * (2 * n) + Points[2, 2] * 3 * (t * t * s) * 3 * (2 * n) + Points[3, 2] * t * t * t * 3 * (2 * n);

            if ((Points.GetLength(0) == 3) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * s * m * m * (-3) + Points[1, 0] * 2 * (s * t) * m * m * (-3) + Points[2, 0] * (t * t) * m * m * (-3) +
                       Points[0, 1] * s * s * 3 * ((-2) * m * n + m * m) + Points[1, 1] * 2 * (s * t) * 3 * ((-2) * m * n + m * m) + Points[2, 1] * (t * t) * 3 * ((-2) * m * n + m * m) +
                       Points[0, 2] * s * s * 3 * (m * 2 * n - n * n) + Points[1, 2] * 2 * (s * t) * 3 * (m * 2 * n - n * n) + Points[2, 2] * (t * t) * 3 * (m * 2 * n - n * n) +
                       Points[0, 3] * s * s * 3 * n * n + Points[1, 3] * 2 * (s * t) * 3 * n * n + Points[2, 3] * (t * t) * 3 * n * n;

            if ((Points.GetLength(0) == 4) && (Points.GetLength(1) == 4))
                return Points[0, 0] * s * s * s * m * m * (-3) + Points[1, 0] * 3 * (s * s * t) * m * m * (-3) + Points[2, 0] * 3 * (t * t * s) * m * m * (-3) + Points[3, 0] * t * t * t * m * m * (-3) +
                       Points[0, 1] * s * s * s * 3 * ((-2) * m * n + m * m) + Points[1, 1] * 3 * (s * s * t) * 3 * ((-2) * m * n + m * m) + Points[2, 1] * 3 * (t * t * s) * 3 * ((-2) * m * n + m * m) + Points[3, 1] * t * t * t * 3 * ((-2) * m * n + m * m) +
                       Points[0, 2] * s * s * s * 3 * (m * 2 * n - n * n) + Points[1, 2] * 3 * (s * s * t) * 3 * (m * 2 * n - n * n) + Points[2, 2] * 3 * (t * t * s) * 3 * (m * 2 * n - n * n) + Points[3, 2] * t * t * t * 3 * (m * 2 * n - n * n) +
                       Points[0, 3] * s * s * s * 3 * n * n + Points[1, 3] * 3 * (s * s * t) * 3 * n * n + Points[2, 3] * 3 * (t * t * s) * 3 * n * n + Points[3, 3] * t * t * t * 3 * n * n;

            return new xyz(0, 0, 0);
        }
        /// <summary>
        /// Calculates the 3D-Points of a Bezier curve of degree 3 with controlpoints "Points".
        /// </summary>
        /// <param name="Points">Controlpoints. The demension has to be 4</param>
        /// <param name="t">Paramter from [0,1]</param>
        /// <returns></returns>
        public static xyz funBezier3D(xyz[] Points, double t)
        {

            int N = Points.Length;
            if (N == 4)
            {
                double s = 1 - t;
                xyz res;

                /* Note: a good optimizing compiler (such as gcc-3) reduces the
                   following to 16 multiplications, using common subexpression
                   elimination. */

                res.x = s * s * s * Points[0].x + 3 * (s * s * t) * Points[1].x + 3 * (t * t * s) * Points[2].x + t * t * t * Points[3].x;
                res.y = s * s * s * Points[0].y + 3 * (s * s * t) * Points[1].y + 3 * (t * t * s) * Points[2].y + t * t * t * Points[3].y;
                res.z = s * s * s * Points[0].z + 3 * (s * s * t) * Points[1].z + 3 * (t * t * s) * Points[2].z + t * t * t * Points[3].z;




                return res;


            }
            xyz result = new xyz(0, 0, 0);

            return result;

        }
        /// <summary>
        ///  retrieves the first derive of Bezierfunction at t.
        ///  The degree is 3;
        /// </summary>
        ///  <param name="Points"> controlpoints of the bezierfuntion. The count determines the degree </param>
        ///  <param name="t">is the value for which the calculation is given  </param>
        ///  <returns>value of the derivation of the Bezierfunction </returns>
        public static xy funBezierDerive(xy[] Points, double t)
        {

            return Points[0] * (-3 * (1 - t) * (1 - t)) + Points[1] * (-6 * (1 - t) * t + 3 * (1 - t) * (1 - t)) +
                 Points[2] * (6 * (1 - t) * t - 3 * t * t) +
                 Points[3] * (3 * t * t);

        }
        /// <summary>
        ///  retrieves the first derive of 3D- Bezier curve at t.
        ///  The degree is 3;
        /// </summary>
        ///  <param name="Points"> controlpoints of the bezierfuntion. The count determines the degree </param>
        ///  <param name="t">is the value for which the calculation is given  </param>
        ///  <returns>value of the derivation of the Bezierfunction </returns>
        public static xyz funBezierDerive3D(xyz[] Points, double t)
        {
            int N = Points.Length;
            xyz result = new xyz(0, 0, 0);
            for (int i = 0; i < N; i++)
                result = result + Points[i] * BernsteinDerive(N - 1, i, t);

            return result;
        }
        /// <summary>
        ///  retrieves the value of QSpline ( a quadratic weighted spline) at t.
        ///  He is given by two points A and B and a controlpoint P. Additionally a weight for the attraction
        ///  of the controlpoint can be set.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// <param name="Controlpoint">Controlpoint</param>
        /// <param name="weight">Attraction of the controlpoint. By default it is 1.</param>
        /// <param name="t">A paramter</param>
        /// <returns>Returns the functionvalue</returns>
        public static xy funQSpline(xy A, xy B, xy Controlpoint, double weight, double t)
        {
            xy zaehler;
            if (A.dist(B) < 0.001) return A;
            double nenner;
            zaehler = B * (t * t) + Controlpoint * (2 * weight * t * (1 - t)) + A * ((1 - t) * (1 - t));
            nenner = t * t + 2 * weight * t * (1 - t) + (1 - t) * (1 - t);
            if (!Utils.Equals(nenner, 0))
                return zaehler * (1 / nenner);
            else
                return new xy(0, 0);
        }
        /// <summary>
        ///  retrieves the value of 3D- QSpline ( a quadratic weighted spline) at t.
        ///  He is given by two points A and B and a controlpoint P. Additionally a weight for the attraction
        ///  of the controlpoint can be set.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// <param name="Controlpoint">Controlpoint</param>
        /// <param name="weight">Attraction of the controlpoint. By default it is 1.</param>
        /// <param name="t">A paramter</param>
        /// <returns>Returns the functionvalue</returns>
        public static xyz funQSpline3D(xyz A, xyz B, xyz Controlpoint, double weight, double t)
        {
            xyz zaehler;

            double nenner;
            zaehler = B * (t * t) + Controlpoint * (2 * weight * t * (1 - t)) + A * ((1 - t) * (1 - t));
            nenner = t * t + 2 * weight * t * (1 - t) + (1 - t) * (1 - t);
            if (!Utils.Equals(nenner, 0))
                return zaehler * (1 / nenner);
            else
                return new xyz(0, 0, 0);
        }
        /// <summary>
        /// Retrieves the first derivation of a Qspline. See <see cref="funQSpline"/>.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// <param name="Controlpoint">Controlpoint</param>
        /// <param name="weight">Attraction of the controlpoint. By default it is 1.</param>
        /// <param name="t">A paramter</param>		
        /// <returns>Value of the derivation</returns>
        public static xy funQSplineDerive(xy A, xy B, xy Controlpoint, double weight, double t)
        {
            xy u = B * (t * t) + Controlpoint * (2 * weight * t * (1 - t)) + A * ((1 - t) * (1 - t));
            xy u1 = B * (2 * t) + Controlpoint * (2 * weight * (1 - 2 * t)) - A * (2 * (1 - t));
            double v = t * t + 2 * weight * t * (1 - t) + (1 - t) * (1 - t);
            double v1 = 2 * t + 2 * weight * (1 - 2 * t) - 2 * (1 - t);
            return (u1 * v - u * v1) * (1 / (v * v) / 2);
        }
        /// <summary>
        /// Retrieves the first derivation of a 3D-Qspline. See <see cref="funQSpline3D"/>.
        /// </summary>
        /// <param name="A">Point A</param>
        /// <param name="B">Point B</param>
        /// <param name="Controlpoint">Controlpoint</param>
        /// <param name="weight">Attraction of the controlpoint. By default it is 1.</param>
        /// <param name="t">A paramter</param>		
        /// <returns>Value of the derivation</returns>
        public static xyz funQSplineDerive3D(xyz A, xyz B, xyz Controlpoint, double weight, double t)
        {
            xyz u = B * (t * t) + Controlpoint * (2 * weight * t * (1 - t)) + A * ((1 - t) * (1 - t));
            xyz u1 = B * (2 * t) + Controlpoint * (2 * weight * (1 - 2 * t)) - A * (2 * (1 - t));
            double v = t * t + 2 * weight * t * (1 - t) + (1 - t) * (1 - t);
            double v1 = 2 * t + 2 * weight * (1 - 2 * t) - 2 * (1 - t);
            return (u1 * v - u * v1) * (1 / (v * v) / 2);
        }
        /// <summary>
        /// Get a minimal rectangle, containing a given rectangle R and a point p.
        /// </summary>
        /// <param name="R">A rectangle</param>
        /// <param name="p">A point</param>
        /// <returns>minimal rectangle</returns>
        public static RectangleF GetMaxRectangle(RectangleF R, xy p)
        {
            double x1 = System.Math.Min(R.X, p.x);
            double y1 = System.Math.Min(R.Y, p.y);
            double x2 = System.Math.Max(R.X + R.Width, p.x);
            double y2 = System.Math.Max(R.Y + R.Height, p.y);
            return new RectangleF((float)System.Math.Round(x1, 6), (float)System.Math.Round(y1, 6), (float)System.Math.Round((x2 - x1), 6), (float)System.Math.Round((y2 - y1), 6));
        }
        /// <summary>
        /// Get a minimal rectangle containing a given rectangle R and a point p.
        /// </summary>
        /// <param name="R">A rectangle</param>
        /// <param name="p">A point</param>
        /// <returns>Minimal rectangle</returns>

        public static Rectangle GetMaxRectangle(Rectangle R, Point p)
        {

            int x1 = System.Math.Min(R.X, p.X);
            int y1 = System.Math.Min(R.Y, p.Y);
            int x2 = System.Math.Max(R.X + R.Width, p.X);
            int y2 = System.Math.Max(R.Y + R.Height, p.Y);
            return new Rectangle(x1, y1, (x2 - x1), (y2 - y1));
        }
        /// <summary>
        /// Calculates whether a point is inside a rectangle or not.
        /// 
        /// </summary>
        /// <param name="R">A rectangle</param>
        /// <param name="p">A point</param>
        /// <returns>True, if the point lies inside of the rectangle</returns>
        public static bool InsideRectangle(RectangleF R, xy p)
        {
            double l = System.Math.Min(R.Right, R.Left);
            double r = System.Math.Max(R.Right, R.Left);

            double u = System.Math.Min(R.Top, R.Bottom);
            double o = System.Math.Max(R.Top, R.Bottom);


            return ((p.x < r) && (p.x > l) &&
                (p.y > u) && (p.y < o));
        }
        /// <summary>
        /// Calculates wheter a point is inside a rectangle or not.
        /// 
        /// </summary>
        /// <param name="R">A rectangle</param>
        /// <param name="p">A point</param>
        /// <returns>True if the point lies inside of the rectangle</returns>
        public static bool InsideRectangle(Rectangle R, Point p)
        {
            int l = System.Math.Min(R.Right, R.Left);
            int r = System.Math.Max(R.Right, R.Left);

            int u = System.Math.Min(R.Top, R.Bottom);
            int o = System.Math.Max(R.Top, R.Bottom);


            return ((p.X < r) && (p.X > l) &&
                (p.Y > u) && (p.Y < o));
        }
        /// <summary>
        /// Rests a rectangle.
        /// </summary>
        /// <returns></returns>
        public static RectangleF Resetrectangle()
        {
            return new RectangleF(float.MaxValue / 2, float.MaxValue / 2, float.MinValue, float.MinValue);
        }




        /// <summary>
        ///  Calculates the points of a unit sphere, where Alpha indicates the geographical longitude and Phi the
        ///  latitude.
        ///  For the values Apha = 0 and Phi = 0 you get the Point(1, 0, 0).
        ///
        ///  For an arbitrary sphere you have to multiply with it´s Radius
        ///   
        /// </summary>
        ///  <param name="Alpha">Angle of the geographical longitude in radian </param>
        ///  <param name="Beta">Angle of the geographical latitude in radian  </param>
        ///  <returns>a point on a unitsphere </returns>
        /// 
        public static xyz PolarUnit(double Alpha, double Beta)
        {

            Matrix R = Matrix.Rotation(new LineType(new xyz(0, 0, 0), new xyz(1, 0, 0)), -Alpha);
            xyz Imagey = R * new xyz(0, 1, 0);
            Matrix T = Matrix.Rotation(new LineType(new xyz(0, 0, 0), Imagey), Beta);
            xyz result = (T * R) * new xyz(0, 0, 1);
            return result;

        }
        /// <summary>
        /// Calculates the polar angles of a Point
        /// </summary>
        /// <param name="P">A Point</param>
        /// <returns>Two angles</returns>
        public static xy UnitToPolar(xyz P)
        {
            double Beta = 0;
            if (new xy(P.y, P.z).length() > 0.000001)
                Beta = System.Math.PI / 2 - Utils.Angle(P, new xyz(1, 0, 0), new xyz(0, P.z, -P.y));
            else
            {
                if (P.z > 0) Beta = System.Math.PI / 2;
                else
                    Beta = -System.Math.PI / 2;
            }



            double Alfa = Utils.Angle(new xyz(0, 0, 1), new xyz(0, P.y, P.z), new xyz(-1, 0, 0));

            return new xy(Alfa, Beta);
        }


        /// <summary>
        ///  Calculates the Spat-product which is defined by (a X b). c, where "X" is the cross product and "." is the dot product.
        ///  It´s the same as the determinant of the Matrix, which has this vectors as columns.
        ///  </summary>
        ///  <param name="a">the first vector </param>
        ///  <param name="b">the second vector</param>
        ///  <param name="c">the third vector</param>
        ///  <returns>retrieves Calculates the Spat-product, which is defined by (a X b). c, where "X" is the vectorproduct and "." is the dotarproduct </returns>

        public static double Spat(xyz a, xyz b, xyz c)
        {
            return a.cross(b).Scalarproduct(c);
        }

        /// <summary>
        ///  The classical trunc-method which returns an integer. 
        ///  This integer is the greatest integer lower than "value".
        ///  
        ///  </summary>
        ///  <returns>gives the greatest integer, which is smaller then value</returns>


        public static int trunc(double value)
        {

            double d = System.Math.Floor(value);
            if ((d < int.MaxValue) && (d > int.MinValue))
                return Convert.ToInt32(d);
            if (d >= int.MaxValue)
                return int.MaxValue;
            if (d <= int.MinValue)
                return int.MinValue;
            return 0;
        }
        /// <summary>
        ///  The classical round-method which returns an integer. 
        ///  This integer is the nearest integer to "value".
        ///  
        ///  </summary>
        ///  <returns> gives the rounded value as integer</returns>

        public static int Round(double value)
        {
            try
            {
                double d = System.Math.Round(value);
                if ((d < int.MaxValue) && (d > int.MinValue))
                    return Convert.ToInt32(d);
                if (d >= int.MaxValue)
                    return int.MaxValue;
                if (d <= int.MinValue)
                    return int.MinValue;
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        ///  The constant epsilon is used in the methods less and equals. It means the tolerance of these functions.
        ///  The default value is 1e-20
        ///  </summary>
        public static double epsilon = 1e-5;

        /// <summary>
        ///  big is the same as double.MaxValue
        ///  </summary>

        public static double big = double.MaxValue;
        /// <summary>
        ///  small is the same as double.MinValue
        ///  </summary>

        public static double small = double.MinValue;
        /// <summary>
        ///  bigI is the same as int.MaxValue
        ///  </summary>

        public static int bigI = int.MaxValue;
        /// <summary>
        ///  smallI is the same as int.MinValue
        ///  </summary>

        public static int smallI = int.MinValue;

        /// <summary>
        ///  This function checks the equality between two values.
        ///  This is to avoid errors about computational accuracy with doubles.
        ///  If their distance is less then epsilon, then equality is done.
        ///  </summary>
        ///  <param name="a">The first value to check </param>
        ///  <param name="b">The second value to check</param>
        ///  <returns> Returns true, if the difference of these counts is smaller than epsilon</returns>

        public static bool Equals(xy a, xy b)
        {
            return (Equals(a.x, b.x) && Equals(a.y, b.y));

        }
        /// <summary>
        /// Gets true if the difference between a and b is &lt; <see cref="Utils.epsilon"/>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(double a, double b)
        {
            return (System.Math.Abs(a - b) < epsilon);
        }
        /// <summary>
        ///  This function controls, at which point a number is lower then an other, this means if
        ///  the difference b - a is less than epsilon, then a is really less than b.
        ///  </summary>
        ///  <param name="a">the first value to check </param>
        ///  <param name="b">the second value to check</param>
        ///  <returns> returns true a is really less then b</returns>

        public static bool Less(double a, double b)
        {

            return ((b - a) > epsilon);
        }
        /// <summary>
        ///  Converts Degree to Radian
        ///  </summary>
        ///  <param name="value">in degree ( from 0 to 360° ) </param>
        ///  <returns> returns value in radian (from 0 to 2*PI)</returns>


        public static double RadToDeg(double value)
        {
            return value * 180 / System.Math.PI;

        }
    }
    /// <summary>
    /// Is used in <see cref="Utils.funBSpline2d"/>
    /// </summary>
    [Serializable]

    public enum KnotType

    {
        /// <summary>
        /// Default
        /// </summary>
        None,
        /// <summary>
        /// the coefficients for the spline is calculated by <see cref="Utils.UniformCoeffSpline"/>
        /// </summary>
        Uniform,
        /// <summary>
        /// the coefficients for the spline is calculated by <see cref="Utils.UniformCoeffSpline"/>
        /// </summary>
        QuasiUniform,
        /// <summary>
        /// the coefficients for the spline is calculated by <see cref="Utils. CoeffSpline"/>
        /// </summary>
        PiecewiseBezier,
        /// <summary>
        /// the coefficients for the spline is calculated by <see cref="Utils.UniformCoeffSpline"/>
        /// </summary>
        Unspecified

    }
}
