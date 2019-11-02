using System.Drawing;
using System;
using System.Globalization;
using System.Collections;
using System.ComponentModel;

using System.Runtime.InteropServices;

using ClipperLib;

namespace Drawing3d
{
    /// <summary>
    /// This structure belongs to the geometrical object 2d-line. It contains two Points, P and Q and of course a direction, given by Q - P.
    /// Additional it contains a Valuefunction with returns for every parameter lam a point on the line.
    /// In case lam = 0 the value is P and in case lam = 1 the alue is Q.
    /// Sometimes the LineType2d is regarded as infinite line, somtimes as bounded line (bounded by P and Q).
    /// To avoid confusions with the Curveclass Line, it is named LineType2d.
    /// 
    /// </summary>

    [Serializable]
    public struct LineType2d
    {
        /// <summary>
        /// The starting point of the line
        /// </summary>
        public xy P;
        /// <summary>
        /// The endpoint of the line
        /// </summary>
        public xy Q
        {

            get { return P + Direction; }
            set { Direction = value - P; }
        }
        /// <summary>
        /// The direction of the line i.e Q - P
        /// </summary>

        public xy Direction;
        /// <summary>
        /// A valuator for the points on the line.
        /// The values lam between 0 and 1 give the points between P and Q.
        /// If it`s the linear function P + (Q - P)*lam.
        /// </summary>
        /// <param name="lam">the parameter for the linear function P + (Q - P)*lam</param>
        /// <returns></returns>
        public xy Value(double lam)
        {
            return P + (Q - P) * lam;
        }
        /// <summary>
        /// This constructor initializes the line by the startpoint <see cref="P"/> and the <see cref="Direction"/>.
        /// </summary>
        /// <param name="P">Starting point</param>
        /// <param name="Direction">Direction</param>
        public LineType2d(xy P, xy Direction)
        {
            this.P = P;
            this.Direction = Direction;
        }

        /// <summary>
        /// Transforms a LineType2d by a transformation M.
        /// <seealso cref="mul"/>
        /// </summary>
        ///
        ///<example>
        /// <code>
        /// Matrix M = Matrix.Translation( new xyz(3, 2, 0));
        /// LineType L = new LineType (new xyz(1, 1), new xyz (2, 1)).
        /// L = L * M;
        /// 
        /// // L.P is now ( 4, 3)
        /// // L.Direction is still (2, 1)
        /// 
        /// </code>
        /// </example> 
        /// <param name="L">LineType2d, which will transformed</param>
        /// <param name="M">Transformation</param>
        /// <returns></returns>
        public static LineType2d operator *(Matrix3x3 M, LineType2d L)
        {
            return L.mul(M);
        }

        /// <summary>
        /// This is the same as Operator "*" 
        /// </summary>
        /// <param name="M">This matrix is used for the transformation</param>
        /// <returns>Returns a LineType, which is transform by the Matrix M</returns>
        public LineType2d mul(Matrix3x3 M)
        {


            xy Q = M * (P + Direction);
            xy R = M * P;
            return new LineType2d(R, Q - R);
        }
        /// <summary>
        /// Checks, whether the LineType2d crosses an other LineType or not. In this method, bounded lines are 
        /// considered. If the cross point is between P and Q and also between the P and Q of the other line
        /// then the result is true, otherwise it would be false.
        /// </summary>
        /// <param name="L">An other LineType to check for crossing</param>
        /// <returns>true, if a cross point exists between P and Q, otherwise false is returned</returns>

        public bool CrossBounded(LineType2d L)
        {
            double lam, mue;
            bool result = Cross(L, out lam, out mue);
            if (!result) return result;
            if (Utils.Less(0, lam)) return false;
            if (Utils.Less(0, mue)) return false;
            if (Utils.Less(lam, 1)) return false;
            if (Utils.Less(mue, 1)) return false;
            return true;
        }
        /// <summary>
        /// Calculates the cross point with an other LineType L, if this exists.
        /// In this case the result is true. With the parameters <b>lam</b> and <b>mue</b> you
        /// can calculate the cross point by <b>Value(lam)</b> resp. <b>L.value(mue)</b>
        /// </summary>
        /// <param name="L">The other line to calculate the cross point</param>
        /// <param name="lam">Parameter to calculate the cross point with Value(lam)</param>
        /// <param name="mue">Parameter to calculate the cross point with L.Value(mue)</param>
        /// <returns></returns>

        public bool Cross(LineType2d L, out double lam, out double mue)
        {
            lam = -1;
            mue = -1;
            double d = Direction & L.Direction;
            if (!(System.Math.Abs(d) < 0.00000000001))
            {
                mue = (Direction & (Q - L.P)) / d;
                lam = (L.Direction & (P - L.Q)) / d;
                return true;
            }
            return false;
        }
        /// <summary>
        /// This method calculates the distance to a point Pt.
        /// The parameter Lam can be taken to calculate the nearest point of the LineType, which is 
        /// also returned by the outvalue Nearest
        /// </summary>
        /// <param name="Pt">Point to calculate the distance to the LineType</param>
        /// <param name="Lam">Parameter to calculate the nearest point</param>
        /// <param name="Nearest">The point on the line which has the smallest distance to Pt</param>
        /// <returns>Returns the distance from the line to the point Pt</returns>
        public double Distance(xy Pt, out double Lam, out xy Nearest)
        {
            Lam = Direction.normalize() * (Pt - P) / Direction.length();
            Nearest = P + Direction * Lam;
            return Nearest.dist(Pt);
        }
        /// <summary>
        /// This method calculates the distance to a point Pt <seealso cref="Distance(xy , out double, out xy)"/>.
        /// The difference to <see cref="Distance(xy , out double, out xy)"/> is:<br/>
        /// If the normalprojection from the Point Pt is outside of PQ then the distance to the
        /// Point P resp Q is taken and Lam is 0 resp 1.
        /// The parameter Lam can be taken to calculate the nearest point of the LineType, which is 
        /// also returned by the outvalue Nearest
        /// </summary>
        /// <param name="Pt">Point to calculate the distance to the LineType</param>
        /// <param name="Lam">Parameter to calculate the nearest point</param>
        /// <returns>Returns the distance from the line to the point Pt</returns>
        public double DistanceBounded(xy Pt, out double Lam)
        {
            xy Dummy = new xy(0, 0);
            double Result = Distance(Pt, out Lam, out Dummy);
            if ((!Utils.Less(Lam, 0)) && ((!Utils.Less(1, Lam))))
                return Result;
            double DiP = P.dist(Pt);
            double DiQ = Q.dist(Pt);
            if (DiP < DiQ)
            {
                Result = DiP;
                Lam = 0;
                return Result;
            }
            Result = DiQ;
            Lam = 1;
            return Result;



        }
        /// <summary>
        /// This method calculates the distance to a point only, if the distance is
        /// smaller then MaxDist, otherwise Utils.big will be returned.
        /// You can imagine a strip with width MaxDist along the line. 
        /// If a point is placed on this
        /// strip, it is accepted and the distance will be calculated and returned.
        /// If CheckP is true, additional to the strip a half circle with center P and radius Maxdist is
        /// considered and analogusly for CheckQ.
        /// </summary>
        /// <param name="p">The point to be tested</param>
        /// <param name="MaxDist">The maximal distance, for which a reasonable result can be provided.</param>
        /// <param name="CheckP">If CheckP is true: If the distance between p and <b>this.P</b> is less than MaxDist, it will be provided as a result. The value of Lam is zero.</param>
        /// <param name="CheckQ">If CheckQ is true: If the distance between p and <b>this.Q</b> is less than MaxDist, it will be provided as a result. The value of Lam is one. </param>
        /// <param name="Lam">The param Lam can be used to calculate the nearest point on the line by Value(Lam)</param>
        /// <returns>In case the distance of the line is smaller then Maxdist, the distance is returned else <see cref="Utils.big"/>
        /// </returns>
        public double Distance(xy p, double MaxDist, bool CheckP, bool CheckQ, out double Lam)
        {
            double dil = Utils.big;
            double dia = Utils.big;
            double dib = Utils.big;
            double result = Utils.big;
            xy Nearest;

            Lam = -1;
            double di = Distance(p, out Lam, out Nearest);
            if (!Utils.Less(MaxDist, di) && !Utils.Less(Lam, 0) && !Utils.Less(1, Lam)) dil = di;
            if (CheckP) dia = p.dist(P);
            if (CheckQ) dib = p.dist(Q);
            if (!Utils.Less(MaxDist, dia) && !Utils.Less(dil, dia) && !Utils.Less(dib, dia)) { Lam = 0; result = dia; }
            else
                if (!Utils.Less(MaxDist, dib) && !Utils.Less(dil, dib) && !Utils.Less(dia, dib)) { Lam = 1; result = dib; }
            else
                    if (!Utils.Less(MaxDist, dil) && Utils.Less(dil, dia) && Utils.Less(dil, dib)) { result = dil; }
            return result;
        }
        /// <summary>
        /// Checks whether a point lays on the line or not.
        /// </summary>
        /// <param name="pt">Point to be checked</param>
        /// <param name="param">A parameter for calculating the Point pt by <see cref="Value"/>, if the point lays on the line, else param = -1.</param> 
        /// <returns>Returns true, if the Point pt lays on the Line, otherwise false.</returns>


        public bool inLine(xy pt, out double param)
        {
            param = -1;
            bool result = (Utils.Equals(0, (pt - P) & (Q - P)));
            if (result)
                param = (pt - P) * (Direction).normalize() / Direction.length();
            return result;
        }
        /// <summary>
        /// Overrides the equals-method and returns true, if point P and the direction are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            LineType2d L = (LineType2d)obj;
            if ((Utils.Equals(L.Direction & Direction, 0) &&
                (Utils.Equals(Direction & (L.P - P), 0)))) return true;
            return false;

        }
        /// <summary>
        /// overrides the GetHashCode-method-
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.P.GetHashCode() + Q.GetHashCode();
        }
        /// <summary>
        /// Checks, if a point <b>pt</b> lies in a sector, which is given by the line (of <b>this</b>) and a second vector <b>direction1</b>.
        /// If you turn the direction of the line (<b>this</b>) in the counterclockwise sense to the other direction( <b>direction1</b>), you have
        /// a unique defined field. If the point is located inside this field, the result is true, otherwise it is false.
        /// By the parameter 'allowBorder' you can include or exclude the border of the sector.
        /// </summary>
        /// <param name="Direction1">Direction, which spans together with the point <b>this.P</b> 
        ///and the direction of
        /// the line (<b>this</b>) a sector. You get the valid area if you turn the direction of the line
        /// in counterclockwise sense to the direction1</param>
        /// <param name="pt">Point which will be tested</param>
        /// <param name="allowBorder">If it is true a point on border results true. </param>
        /// <returns>Returns true, if the point lies in the valid sector field</returns>
        public bool inSector(xy Direction1, xy pt, bool allowBorder)
        {
            double param;
            if ((allowBorder) && (inLine(pt, out param) || new LineType2d(P, Direction1).inLine(pt, out param)) && (!Utils.Less(param, 0)))
                return true;
            if (Utils.Less(0, (Direction & Direction1)))
                return (Utils.Less(0, (Direction & (pt - P))) && Utils.Less(0, ((pt - P) & Direction1)));
            else
                return !((Utils.Less(Direction & (pt - P), 0)) && Utils.Less((pt - P) & Direction1, 0));
        }

    }


}
