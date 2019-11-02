using System;
using System.Collections.Generic;




namespace Drawing3d
{  
    /// <summary>
    /// is a <see cref="Surface"/>, which rotates a 3D-curve aroun the z-axis.
    /// </summary>
    [Serializable]
    public class Curve3dRotator : Surface
    {
        /// <summary>
        /// is an empty constructor.
        /// </summary>
        public Curve3dRotator():base()
        {
            VPeriodicity = System.Math.PI * 2;
            VFactor = Math.PI * 2;
        }
        Curve3D _Curve = null;
        /// <summary>
        /// gets and sets the <see cref="Curve3D"/>, which will be rotated around the z-axis.
        /// </summary>
        public Curve3D Curve {
            get { return _Curve; } 
            set { _Curve = value; StartAngle = -1; Invalid = true; }
        }
        void RefreshStartAngle()
        {
            xyz A = Base.Relativ(Value(0, 0));
            if (Math.Abs(A.x) > 0.00001)
            {
                if (Value(0, 0).x > 0) StartAngle = 0;
                else StartAngle = Math.PI;

            }
            else
            {
                A = Base.Relativ(Value(0.5, 0));
                if (Math.Abs(A.x) > 0.00001)
                {
                    if (Value(0.5, 0).x > 0) StartAngle = 0;
                    else StartAngle = Math.PI;

                }
                else
                {
                    A = Base.Relativ(Value(1, 0));
                    if (Math.Abs(A.x) > 0.00001)
                    {
                        if (Value(1, 0).x > 0) StartAngle = 0;
                        else StartAngle = Math.PI;

                    }
                }

            }
        }
        /// <summary>
        /// overrides the <see cref="Surface.Value(double, double)"/> method.
        /// </summary>
        /// <param name="u">first paramter.</param>
        /// <param name="v">second paramter.</param>
        /// <returns>is the point in the room.</returns>
        public override xyz Value(double u, double v)
        {
            xyz P = Curve.Value(u);


            xyz Q = new xyz(System.Math.Cos(v*VFactor) * P.x-Math.Sin(v * VFactor) *(Curve.Value(u).y),
                           + Math.Sin(v * VFactor) *P.x+System.Math.Cos(v * VFactor) * (Curve.Value(u).y),
                            Curve.Value(u).z);
            if (ZHeight(u, v) > 0)
            {
                Q = Q + Normal(u, v) * ZHeight(u, v);
            }
            return this.Base.Absolut(Q);
        }
        /// <summary>
        /// overrides the <see cref="Surface.uDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first paramter.</param>
        /// <param name="v">second paramter.</param>
        /// <returns>is the partial u deriavation.</returns>
        public override xyz uDerivation(double u, double v)
        {
            xyz CurveDerive = Curve.Derivation(u);
            double x = System.Math.Cos(v * VFactor) * Curve.Derivation(u).x -Math.Sin(v * VFactor) * (Curve.Derivation(u).y);
            double y = Math.Sin(v * VFactor) * Curve.Derivation(u).x + System.Math.Cos(v * VFactor) * (Curve.Derivation(u).y);
            double z = Curve.Derivation(u).z;
            return Base.Absolut(new xyz(x,y,z)) - Base.BaseO;
        }
        /// <summary>
        /// overrides the <see cref="Surface.vDerivation(double, double)"/>.
        /// </summary>
        /// <param name="u">first paramter.</param>
        /// <param name="v">second paramter.</param>
        /// <returns>is the partial v deriavation.</returns>
        public override xyz vDerivation(double u, double v)
        {
            xyz P = Curve.Value(u);
            double x = -System.Math.Sin(v * VFactor) * VFactor * Curve.Value(u).x - Math.Cos(v * VFactor) * VFactor * (Curve.Value(u).y);
            double y = Math.Cos(v * VFactor) * VFactor * Curve.Value(u).x - System.Math.Sin(v * VFactor) * VFactor * Curve.Value(u).y;
            double z = 0;
            xyz Q = new xyz(x, y, z);
            return Base.Absolut(Q) - Base.BaseO;
        }
        double StartAngle = -1;
 
        /// <summary>
        /// overrides the <see cref="Surface.ProjectPoint(xyz)"/> method.
        /// </summary>
        /// <param name="Point">is the point, which will be projected.</param>
        /// <returns>u and v parameters for <see cref="Surface.Value(double, double)"/>.</returns>
        public override xy ProjectPoint(xyz Point)
        {
            if (StartAngle == -1)
                RefreshStartAngle();
           xyz Pt = Base.Relativ(Point);
            double v = StartAngle - System.Math.Atan2(Pt.y, Pt.x);
            xyz Q = Matrix.Rotation(new xyz(0, 0, 1), v) * Pt;
            xyzArray A = Curve.ToxyzArray();
            double Lam = -1;
            A.Distance(Q, 1e10, out Lam);
           xyz N = Value(Lam / Curve.Resolution, v);
         
            return new xy(Lam / Curve.Resolution, v);
           }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Curve3dRotator Result = base.Copy() as Curve3dRotator;
            Result.Curve = Curve.Clone() as Curve3D;
            Result.StartAngle = StartAngle;
          

            return Result;
        }
    }
}