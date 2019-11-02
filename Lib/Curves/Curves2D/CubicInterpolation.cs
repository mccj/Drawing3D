using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d
{  
    /// <summary>
    /// in herits from <see cref="Bezier"/> and has a degree 3. The <see cref="Nurbs.ControlPoints"/> are empty.
    /// </summary>
    [Serializable]
    public class CubicInterpolation : Bezier
    {
        /// <summary>
        /// overrides <see cref="Bezier.GetDegree"/> and returns 3.
        /// </summary>
        /// <returns></returns>
        public override int GetDegree()
        {
            return 3;
        }
        /// <summary>
        /// constructor, which initialize the  <see cref="CubicInterpolation"/> with points, which will be interpolated.
        /// </summary>
        /// <param name="InterpolationPoints">the points, which will be interpolated.</param>
        public CubicInterpolation(xy[] InterpolationPoints)
        {
            SetInterPolationPoints(InterpolationPoints);
        }
        bool Closed(xy[] Array)
        {
            if (Array.Length > 0)
                return Array[0].dist(Array[Array.Length - 1]) < 0.000001;
            else
                return false;
        }
        /// <summary>
        /// gets and sets the InterpolationPoints.
        /// </summary>
        public xy[] InterpolationPoints
        {
            get { return GetInterPolationPoints(); }
            set { SetInterPolationPoints(value); }
        }
        xy[] GetInterPolationPoints()
        {
            xy[] Result = new xy[(Points.Length) / 3 + 1];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i] = Points[3 * i];
            }
            Result[Result.Length - 1] = Points[Points.Length - 1];
            return Result;
        }

        void SetInterPolationPoints(xy[] InterPolationPoints)
        {
 
            Points = new xy[3 * (InterPolationPoints.Length - 1) + 1];

            for (int i = 0; i < InterPolationPoints.Length; i++)
            {
                int v = i - 1; if (v < 0)
                    if (Closed(InterPolationPoints))
                        v = InterPolationPoints.Length - 2;
                int n = i + 1; if (n >= InterPolationPoints.Length)
                    if (Closed(InterPolationPoints)) n = 1; else n = -1;

                Points[3 * i] = InterPolationPoints[i];
                if ((v >= 0) && (n >= 0))
                {
                    int a = 3 * i + 1;
                    if (a >= Points.Length) a -= Points.Length - 1;
                    Points[a] = InterPolationPoints[i] + (InterPolationPoints[n] - InterPolationPoints[v]) * (1f / 5f);


                    int b = 3 * v + 2;
                    if (b >= Points.Length) b -= Points.Length - 1;

                    Points[b] = InterPolationPoints[i] - (InterPolationPoints[n] - InterPolationPoints[v]) * (1f / 5f);
                }
                else
                {
                    if (v >= 0)
                        Points[3 * v + 2] = InterPolationPoints[i];
                    else
                        Points[3 * i + 1] = InterPolationPoints[i];
                }

            }
            Dirty = true;
        }
        /// <summary>
        /// overrides <see cref="Nurbs.getKnots"/> and returns <see cref="Utils.StandardKnots(int, int)"/>.
        /// </summary>
        /// <returns>returns <see cref="Utils.StandardKnots(int, int)"/>.</returns>
        public override double[] getKnots()
        {
            double[] k = Utils.StandardKnots(Points.Length, 2);
            return k;
        }


        /// <summary>
        /// overrides <see cref="Nurbs.getWeights"/> and set it all to 1.
        /// </summary>
        /// <returns></returns>
        public override double[] getWeights()
        {
            double[] W = new double[getCtrlPoints().Length];
            for (int i = 0; i < W.Length; i++)
            {
                W[i] = 1;
            }
            return W;
        }

        /// <summary>
        /// Empty constructor and sets the resolution to 20.
        /// </summary>
        public CubicInterpolation()
        {
            Resolution = 20;
        
        }


       
        xy[] Points = new xy[0];
        double getLocal(double t, xy[] P)
        {
            t = t * 0.999999;
            int StartId = (int)(t * (Points.Length - 1) / 3f);


            t = t * (Points.Length - 1) / 3f - StartId;


            P[0] = Points[StartId * 3];
            P[1] = Points[StartId * 3 + 1];
            P[2] = Points[StartId * 3 + 2];

            P[3] = Points[StartId * 3 + 3];
            return t;
        }
        /// <summary>
        /// Overrides the abstract Value function <see cref="Curve.Value"/>of the curve class and retrieves the Bezierfunction of t depending on the
        /// control points <see cref="Points"/>.
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>returns the bezierfunction</returns>
        public override xy Value(double t)
        {
            xy[] P = new xy[4];
            t = getLocal(t, P);
            return Utils.funBezier(P, t);
        }
        /// <summary>
        /// Overrides the abstract Derivationfunction <see cref="Curve.Derivation"/> of the curve class and retrieves the 
        /// derivation of the Bezierfunction of t, 
        /// depending on the controlpoints <see cref="Points"/>
        /// 
        /// </summary>
        /// <param name="t">parameter</param>
        /// <returns>Derivation of the bezierfunction</returns>
        public override xy Derivation(double t)
        {
            xy[] P = new xy[4];
            t = getLocal(t, P);
            return Utils.funBezierDerive(P, t);
        }

        /// <summary>
        /// Overrides the method <see cref="Curve.setA"/> by setting the value of the Point[0];
        /// </summary>
        /// <returns>Value of A</returns>
        protected override void setA(xy value)
        {
            xy save = Atang;
            Points[0] = value;
            Atang = save;
            Dirty = true;
        }

        ///// <summary>
        ///// Overrides the <see cref="Curve.Slice"/>-method.
        ///// </summary>
        //public override void Slice(double from, double to)
        //{  
        //    this.fromParam = from;
        //    this.toParam = to;
        //    Dirty = true;

        //}
 

        /// <summary>
        /// Overrides the <see cref="Curve.setAtang"/>-method
        /// </summary>
        /// <param name="value">Starttangent</param>
        protected override void setAtang(xy value)
        {
            if (Points.Length > 1)
                Points[1] = Points[0] + value;
            else
                base.setAtang(value);
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.Transform"/>-method, whitch transforms the Control<see cref="Points"/>.
        /// </summary>
        /// <param name="m">Matrix which will transform.</param>
        public override void Transform(Matrix3x3 m)
        {
            for (int i = 0; i < Points.Length; i++)
                Points[i] = Points[i].mul(m);
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.setB"/>-method.
        /// </summary>
        /// <param name="value">Endpoint</param>
        protected override void setB(xy value)
        {
            xy save = Btang;
            if (Points.Length > 1)
                Points[Points.Length - 1] = value;
            base.setB(value);
            Btang = save;
            Dirty = true;
        }

        /// <summary>
        /// Overrides the <see cref="Curve.setBtang"/> by setting Points[2] to Points[3] - value;
        /// </summary>
        /// <param name="value">Endtangent</param>
        protected override void setBtang(xy value)
        {
            if (Points.Length > 1)
                Points[Points.Length - 2] = Points[Points.Length - 1] - value;
            Dirty = true;
        }
        /// <summary>
        /// overrides the <see cref="Bezier.Invert"/> method.
        /// </summary>
        public override void Invert()
        {
            xy[] Save = new xy[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                Save[Points.Length - i-1] = Points[i];
            }
            Points = Save;
            //double d = fromParam;
            //fromParam = toParam;
            //toParam = d;
        }
   }
  
}
