using System;
using System.Collections.Generic;
using System.Text;

namespace Drawing3d
{
    /// <summary>
    /// is a <see cref="Surface"/>which rotates a curve around the z-axis and has a <see cref="FromAngle"/> and a <see cref="ToAngle"/>.
    /// </summary>
    [Serializable]
    public class Curve2dRotator : Surface
    {
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public Curve2dRotator() : base()
        {
            VPeriodicity = System.Math.PI * 2;
            VFactor = Math.PI * 2;
        }
        void CheckAngles()
        {
            while (_FromAngle < 0) _FromAngle += Math.PI * 2;
            while (_FromAngle > Math.PI * 2) _FromAngle -= Math.PI * 2;
            while (_ToAngle < 0) _ToAngle += Math.PI * 2;
            while (_ToAngle > Math.PI * 2) _ToAngle -= Math.PI * 2;

            if (_FromAngle > _ToAngle) _ToAngle += Math.PI * 2;


        }
        double _FromAngle = 0;
        /// <summary>
        /// FromAngle is relative to the x-axis
        /// </summary>
        public double FromAngle
        {
            get { return _FromAngle; }
            set { _FromAngle = value;
                CheckAngles();
                Invalid = true;
            }
        }
        double _ToAngle = Math.PI * 2;
        /// <summary>
        /// ToAngle is the end angle, relatively to the x-axis.
        /// </summary>
        public double ToAngle
        {
            get { return _ToAngle; }
            set { _ToAngle = value;
                CheckAngles();
                Invalid = true;
            }
        }
       
        Curve _Curve = null;
        /// <summary>
        /// is the curve which will be rotated.
        /// </summary>
        public Curve Curve
        {
            get { return _Curve; }
            set { _Curve = value;
                Invalid = true;
            }
        }
        /// <summary>
        /// overrides <see cref="Surface.Value(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>is the calculated point on the surface.</returns>
        public override xyz Value(double u, double v)
        {
 
            double x = System.Math.Cos(v*(ToAngle / VFactor + (1 - v) * FromAngle / VFactor)*( Math.PI*2)) * Curve.Value(u).x;
            double y = System.Math.Sin(v * (ToAngle / VFactor + (1 - v) * FromAngle / VFactor) * (Math.PI * 2)) * Curve.Value(u).x;
            double z = Curve.Value(u).y;
            xyz R = new xyz(x, y, z);
            if (ZHeight(u,v)>0)
            {
                R = R + Normal(u, v) * ZHeight(u, v);
             }
           
            return this.Base.Absolut(R);
        }
        /// <summary>
        /// overrides <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>is the partial u deriavation.</returns>
        public override xyz uDerivation(double u, double v)
        {
            double x = System.Math.Cos(v*Math.PI*2 * (ToAngle / VFactor + (1 - v) * FromAngle / VFactor)) * Curve.Derivation(u).x;
            double y = System.Math.Sin(v * Math.PI * 2 * (ToAngle / VFactor + (1 - v) * FromAngle / VFactor)) * Curve.Derivation(u).x;
            double z = Curve.Derivation(u).y;


            xyz Q = new xyz(x, y, z);
            return Base.Absolut(Q) - Base.BaseO;
        }
        /// <summary>
        /// overrides <see cref="Surface.vDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first parameter.</param>
        /// <param name="v">second parameter.</param>
        /// <returns>is the partial v deriavation.</returns>
        public override xyz vDerivation(double u, double v)
        {
            double x = -System.Math.Sin(v * Math.PI * 2 * (ToAngle / VFactor + (1 - v) * FromAngle / VFactor)) * Curve.Value(u).x;
            double y = System.Math.Cos(v * Math.PI * 2 * (ToAngle / VFactor + (1 - v) * FromAngle / VFactor)) * Curve.Value(u).x;
            double z = 0;
            xyz Q = new xyz(x, y, z);
            return Base.Absolut(Q) - Base.BaseO;
        }
        /// <summary>
        /// overrides <see cref="Surface.ProjectPoint(xyz)"/> to get the parameters u and v.
        /// </summary>
        /// <param name="Point">is the 3D point.</param>
        /// <returns></returns>
        public override xy ProjectPoint(xyz Point)
        {
            double alfa = Math.Atan2(Point.z, Point.x);
            xyz PP = Matrix.Rotation(new LineType(new xyz(0, 0, 0), new xyz(0, 1, 0)), alfa) * Point;
            double Param = -1;
            double d = Curve.Distance(new LineType(PP, new xyz(0, 0, 1)), 1e10, out Param);
            return new xy(Param, alfa / VFactor);
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Curve2dRotator Result = base.Copy() as Curve2dRotator;
            Result.Curve = Curve.Clone() as Curve;
            Result.FromAngle = FromAngle;
            Result.ToAngle = ToAngle;
          
            return Result;
        }
    }
}
