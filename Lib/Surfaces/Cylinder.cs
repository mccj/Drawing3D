using System;
using System.Drawing;
namespace Drawing3d
{
    /// <summary>
    /// A cylinder with <see cref="Radius"/>  and <see cref="Height"/>.
    /// </summary>
    [Serializable]
    public class Cylinder : PrimitiveSurface
    {
        /// <summary>
        /// the center of the cylinder.
        /// </summary>
        public xyz Center
        {
            get { return BasePoint; }
            set
            {
                Invalid = true;
                BasePoint = value;
                RefreshEnvBox();
            }


        }
        bool Schittpunkt(xyz Straightpkt, xyz StraightDir, xyz AxisPoint, xyz AxisDir,ref xyz Pt1,ref xyz Pt2, double Radius)
        {
            xyz a = Straightpkt - AxisPoint;
            xyz e = StraightDir - AxisDir * ((StraightDir * AxisDir) / (AxisDir * AxisDir));
            xyz f = a - AxisDir * ((a * AxisDir) / (AxisDir * AxisDir));
            double P = 2 * (e * f) / (e * e);
            double Q = ((f * f) - Radius * Radius) / (e * e);
            if (P * P / 4 - Q < 0) return false;
            double l1 = -P / 2 + Math.Sqrt(P * P / 4 - Q);
            Pt1 = a + StraightDir * (-P / 2 + Math.Sqrt(P * P / 4 - Q));
            Pt2 = a + StraightDir * (-P / 2 - Math.Sqrt(P * P / 4 - Q));
            return true;
        }
        /// <summary>
        /// overrides <see cref="Surface.getCross(LineType, ref double, ref double)"/>.
        /// </summary>
        /// <param name="L">is a crossing line.</param>
        /// <param name="u">parameter u. See also <see cref="Value(double, double)"/> </param>
        /// <param name="v">parameter v. See also <see cref="Value(double, double)"/></param>
        /// <returns></returns>
        public override bool getCross(LineType L, ref double u, ref double v)
        {
            xyz Pt1 = new xyz(0, 0, 0);
            xyz Pt2 = new xyz(0, 0, 0);
            bool Cross= Schittpunkt(L.P, L.Direction, Center, new xyz(0, 0, 1), ref Pt1, ref Pt2, Radius);
            if (!Cross) return false;
            if ((Pt1.z > Height + 0.00000001) || (Pt2.z > Height + 0.00000001)) return false;
            xy uv = new xy(0, 0);
            if (L.P.dist(Pt1)< L.P.dist(Pt2))
               uv= ProjectPoint(Pt1);
               else
                uv = ProjectPoint(Pt2);
            
            u = uv.x;
            v = uv.y;
            return true;
        }
        RectangleF Env = new RectangleF();
        /// <summary>
        /// overrides <see cref="Surface.SetBoundedCurves(Loca)"/>.
        /// </summary>
        /// <param name="value">the bounded curves.</param>
        protected override void SetBoundedCurves(Loca value)
        {
            base.SetBoundedCurves(value);
            if (value != null)
                Env = value[0].MaxRect;
        }
 
        double _Height = 1;
       /// <summary>
       /// is the height of the cylinder,
       /// </summary>
        public double Height
        {
            get { return _Height; }
            set { _Height = value;
                   Invalid = true;
                }
        }
        /// <summary>
        /// A constructor without parameters. It sets the VResolution to 1 and Uperiodicity and UFactor to 2PI.
        /// 
        /// </summary>
        public Cylinder()
        {
            VResolution = 1;
            UResolution = 32;
            UPeriodicity = System.Math.PI * 2;
            UFactor = 2 * System.Math.PI;
            VPeriodicity = 1e10;
        }
        /// <summary>
        /// is a constructor with <b>radius</b> and <b>height</b>.
        /// </summary>
        /// <param name="Radius">the radius.</param>
        /// <param name="Height">the height.</param>
        public Cylinder(double Radius,double Height):this()
        {
            this.Radius = Radius;
            this.Height = Height;
        }
        private double _Radius;
        /// <summary>
        /// Sets and gets the radius of the cylinder.
        /// </summary>
        public double Radius
        {
            get { return _Radius; }
            set { _Radius = value;
                   Invalid = true;
                }
        }

        /// <summary>
        /// Overrides the <see cref="Surface.ProjectPoint"/> method and calculates a near point on the cylinder.
        /// </summary>
        /// <param name="Point">The u and v relative to the cylinder parameters of a near point.</param>
        /// <returns></returns>
        public override xy ProjectPoint(xyz Point)
        {
           
            xyz p = Base.Relativ(Point);
            xy Result = new xy(Math.Atan2(p.y, p.x)/UFactor, p.z/Height);
            return Result;

        }
  
        /// <summary>
        /// Overrides the Value function and implements the Cylinder calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz Value(double u, double v)
        {
            double x = Radius * System.Math.Cos(u * UFactor);
            double y = Radius * System.Math.Sin(u * UFactor);
            double z = Height* v;
            if (ZHeight(u, v) > 0)
                return Base.Absolut(new xyz(x, y, z) + Normal(u, v) * ZHeight(u, v));

            return Base.Absolut(new xyz(x, y, z));

        }
        /// <summary>
        /// Overrides the uDerivation function and implements the cone calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz uDerivation(double u, double v)
        {
            double x = -Radius * System.Math.Sin(u * UFactor) * UFactor;
            double y = Radius * System.Math.Cos(u * UFactor) * UFactor;
            double z = 0;
            return Base.Absolut(new xyz(x, y, z)) - Base.BaseO;
        }

        /// <summary>
        /// Overrides the vDerivation function and implements the cone calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz vDerivation(double u, double v)
        {
            return Base.Absolut(new xyz(0, 0, 1)) - Base.BaseO;
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Cylinder Result = base.Copy() as Cylinder;
            Result.Radius = Radius;
            Result.Height = Height;


            return Result;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUDegree"/> method.
        /// </summary>
        /// <returns></returns>   

        public override int getUDegree()
        {
            return 2;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getVDegree"/> method.
        /// </summary>
        /// <returns></returns>
        public override int getVDegree()
        {
            return 1;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override xyz[,] getCtrlPoints()
        {

            RefreshEnvBox();
            double BaseRadius = Radius;
            double Max = Env.Y + Env.Height;
            double Min = Env.Y;
            xyz[,] CP = new xyz[,]
            {
            {new xyz(BaseRadius,0,0), new xyz(BaseRadius,0,Height)},
            {new xyz(BaseRadius,BaseRadius,0),new xyz(BaseRadius,BaseRadius,Height)},
            {new xyz(0,BaseRadius,0), new xyz(0,BaseRadius,Height)},
            {new xyz(-BaseRadius,BaseRadius,0), new xyz(-BaseRadius,BaseRadius,Height)},
            {new xyz(-BaseRadius,0,0),new xyz(-BaseRadius,0,Height)},
            {new xyz(-BaseRadius,-BaseRadius,0), new xyz(-BaseRadius,-BaseRadius,Height)},
            {new xyz(0,-BaseRadius,0), new xyz(0,-BaseRadius,Height)},
            {new xyz(BaseRadius,-BaseRadius,0), new xyz(BaseRadius,-BaseRadius,Height)},
            {new xyz(BaseRadius,0,0), new xyz(BaseRadius,0,Height)
             },
     
             };
           
            for (int i = 0; i < CP.GetLength(0); i++)
            {
                for (int j = 0; j < CP.GetLength(1); j++)
                {
                    CP[i, j] = Base.Absolut(CP[i, j]);
                }
            }
            return CP;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[] getUKnots()
        {
            double[] UKnots = new double[12];
            UKnots[0] = UKnots[1] = UKnots[2] = 0;
            UKnots[3] = UKnots[4] = 1f / 4f;
            UKnots[5] = UKnots[6] = 1f / 2f;
            UKnots[7] = UKnots[8] = 3f / 4f;
            UKnots[9] = UKnots[10] = UKnots[11] = 1;
            //for (int i = 0; i < UKnots.Length; i++)
            //{
            //    UKnots[i] *= System.Math.PI * 2;
            //}
            return UKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getVKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[] getVKnots()
        {
            if ((BoundedCurves != null) && (BoundedCurves.Count > 0))
                Env = BoundedCurves[0].MaxRect;
            else
                Env = new RectangleF(new PointF(0, 0), new SizeF((float)Radius, (float)Height));
            // return Utils.StandardKnots(2, 1, Env.Y, Env.Y + Env.Height);

            return Utils.StandardKnots(2, 1, 0, 1);
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getWeights"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[,] getWeights()
        {
            double[,] Weights = new double[9, 2];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Weights[i, j] = 1;

                    if ((i) / 2 * 2 != i)
                        Weights[i, j] = System.Math.Sqrt(2) / 2f;
                    else
                        Weights[i, j] = 1;

                }
            }
            return Weights;
        }
    }
}