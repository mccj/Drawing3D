using System;
using System.Collections.Generic;
using System.Drawing;


namespace Drawing3d
{
    /// <summary>
    /// This class holds a triangle with the three points A, B, C. Their type is <see cref="xyzf"/>.
    /// </summary>
    [System.Serializable]
    public class TriangleF
    {
        /// <summary>
        /// Point of the Triangle
        /// </summary>
        public xyzf A;
        /// <summary>
        /// Point of the Triangle
        /// </summary>
       public xyzf B;
        /// <summary>
        ///  Point of the Triangle
        /// </summary>
       public xyzf C;

        /// <summary>
        /// The constructor initializes the class with the values A, B, C
        /// </summary>
        /// <param name="A">1. point</param>
        /// <param name="B">2. point</param>
        /// <param name="C">3. point</param>
        public TriangleF(xyzf A, xyzf B, xyzf C)
        {
            this.A = A;
            this.B = B;
            this.C = C;

        }
        /// <summary>
        /// crosses two triangles <b>t1</b> and <b>t2</b>
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="Lam"></param>
        /// <param name="Mue"></param>
        /// <param name="P"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        public static bool Cross(TriangleF t1, TriangleF t2, out double Lam, out double Mue, out xyz P, out xyz Q)
        {
            xyzArray A = new xyzArray();
            xyzArray B = new xyzArray();
            A.data = new xyz[] { t1.A.Toxyz(), t1.B.Toxyz(), t1.C.Toxyz(), t1.A.Toxyz() };
            B.data = new xyz[] { t2.A.Toxyz(), t2.B.Toxyz(), t2.C.Toxyz(), t2.A.Toxyz() };
            Lam = -1;
            Mue = -1;
            P = new Drawing3d.xyz(0, 0, 0);
            Q = new Drawing3d.xyz(0, 0, 0);

            double d = (A.Distance(B, 1e10, out Lam, out Mue));
            if (d < 2)

            {
                P = A.Value(Lam);
                Q = B.Value(Mue);
                return true;

            }
            return false;
        }
        /// <summary>
        /// crosses the triangle with a line. It returns true if the cross point is inside the triangle. In this case <b>Lam</b> and <b>Point</b> will be calculated.
        /// You get the cross point <b>Pt</b> by L.Value(Lam).
        /// </summary>
        /// <param name="L">is the line</param>
        /// <param name="Pt">is the cross point</param>
        /// <param name="Lam">is the parameter. You get the cross point <b>Pt</b> by L.Value(Lam).</param>
        /// <returns></returns>
        public bool Cross(LineType L, ref xyzf Pt, ref double Lam)
        {

            Plane P1 = new Plane(A, B, C);
            xyz P = new xyz(0, 0, 0);
            Lam = -1;
            if (P1.Cross(L, out Lam, out P))
            {
                xyz PP = L.Value(Lam);
                Pt = new Drawing3d.xyzf((float)P.x, (float)P.y, (float)P.z);
                if (Inside(P.toXYZF()))
                {
                    Pt = P.toXYZF();
                    return true;
                }

            }
            return false;
        }
        /// <summary>
        /// crosses two triangles. If they have crosspoints the result is true and <b>Pt1</b> and <b>Pt2</b> are the cross points.
        /// </summary>
        /// <param name="T1">is the first triangle.</param>
        /// <param name="T2">is the second triangle.</param>
        /// <param name="Pt1">is one crosspoint.</param>
        /// <param name="Pt2">is the other crosspoint.</param>
        /// <returns>true, if the triangles crosses each other.</returns>
        public static bool Cross(TriangleF T1, TriangleF T2, /*LineType ViewLine,*/ ref xyzf Pt1, ref xyzf Pt2)
        {
            int PtCount = 0;
            Plane P1 = new Plane(T1.A, T1.B, T1.C);
            Plane P2 = new Plane(T2.A, T2.B, T2.C);
            //if ((P1.NormalUnit * ViewLine.Direction < 0)
            //               || (P2.NormalUnit * ViewLine.Direction < 0)
            //               ) return false;
            Check(P1, T1, ref PtCount, T2.A, T2.B, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;
            Check(P1, T1, ref PtCount, T2.B, T2.C, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;
            Check(P1, T1, ref PtCount, T2.C, T2.A, ref Pt1, ref Pt2);
            if (PtCount == 2) return true;
            Check(P2, T2, ref PtCount, T1.A, T1.B, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;
            Check(P2, T2, ref PtCount, T1.B, T1.C, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;
            Check(P2, T2, ref PtCount, T1.C, T1.A, ref Pt1, ref Pt2);
            if (PtCount == 2)
                return true;

            return false;

        }
        static void Check(Plane P1, TriangleF T, ref int PtCount, xyzf A, xyzf B, ref xyzf Pt1, ref xyzf Pt2)
        {

            double Lam = -1;
            xyz Pt = new xyz(0, 0, 0);

            if (P1.Cross(new LineType(A.Toxyz(), (B.Toxyz() - A.Toxyz())), out Lam, out Pt))
            {
                xyz N = A.Toxyz() + (B.Toxyz() - A.Toxyz()) * Lam;

                if ((Lam >= -0.000000001) && (Lam < 1.000000001))
                {
                    if (PtCount == 0)
                    {
                        Pt1 = Pt.toXYZF();
                        PtCount = 1;
                    }
                    else
                if (PtCount == 1)
                        if (Pt1.dist(Pt.toXYZF()) > 0.0001)
                        {
                            Pt2 = Pt.toXYZF();
                            PtCount++;
                        }


                }

            }
        }
        /// <summary>
        /// checks, whether the point <b>P</b> is inside the triangle.
        /// </summary>
        /// <param name="P">is the point to check</param>
        /// <returns>true if the point <b>P</b> is inside the triangle</returns>
        public bool Inside(xyzf P)
        {

            xyzf N = ((B - A) & (C - A)).normalized();
            return (((((A - P) & (B - P)) * N > -0.1)
                  && (((B - P) & (C - P)) * N > -0.01)
                && (((C - P) & (A - P)) * N > -0.01)
                )
                ||
                ((((A - P) & (B - P)) * N < 0.01)
                  && (((B - P) & (C - P)) * N < 0.01)
                && (((C - P) & (A - P)) * N < 0.01)
                ));
       }
    }
}
