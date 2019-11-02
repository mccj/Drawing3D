using System;
using System.Drawing;
namespace Drawing3d
{
    /// <summary>
    /// This class implements the a cone. It is inherited from <see cref="PrimitiveSurface"/>. It is defined by a  <see cref="Radius"/>/> the
    /// <see cref="Height"/>. You can define also the <see cref="HalfAngle"/> on the top, so the Height is calculated.
    /// </summary>
    [Serializable]
    public class Cone : PrimitiveSurface
    {
         RectangleF Env = new RectangleF();
        /// <summary>
        /// it overrides <see cref="Surface.SetBoundedCurves(Loca)"/>.
        /// </summary>
        /// <param name="value">the bounding curve.</param>
        protected override void SetBoundedCurves(Loca value)
        {
          
            base.SetBoundedCurves(value);
            if (value != null)
                Env = value[0].MaxRect;
        }
        /// <summary>
        /// gets and sets the center of the cone.
        /// </summary>
        public xyz Center
        {
            get { return BasePoint; }
            set
            {
                BasePoint = value;
                RefreshEnvBox();
                Invalid = true;
            }
        }
 
        /// <summary>
        /// overrides <see cref="Surface.CheckPeriodic"/>.
        /// </summary>
        protected override void CheckPeriodic()
        {
            UpPol = new Pol(true, Height);
        }
 
        /// <summary>
        /// Is only implemented, if the paramcurve is a 2D - Line. In case that this line
        /// is parallel to the x-axis a Circle3d will be returned, which belongs to the paramcurve.
        /// If it is parallel to y-axis a Line3d on the Cone, which belongs to the paramcurve will be returned
        /// </summary>
        /// <param name="ParamCurve">Specifies the 2d-paramcurve, which will be lifted to a 3d-Curve </param>
        /// <returns>Returns a 3d-Curve.</returns>
        public override Curve3D To3dCurve(Curve ParamCurve)
        {
            if (ParamCurve is Line)
            {
                if (System.Math.Abs(ParamCurve.Atang.y) < 0.0001)
                {
                    Circle3D C = new Circle3D();

                    {
                        C.Radius = (Value(0, ParamCurve.A.y) - Value(0.5, ParamCurve.A.y)).length() / 2;
                        Base B = Base;
                        xyz T = B.BaseZ * (B.BaseZ * (Value(ParamCurve.A.x, ParamCurve.A.y) - Base.BaseO));
                        B.BaseO = B.BaseO + T;
                        C.Base = B;
                        return C;
                    }
                }
                if (System.Math.Abs(ParamCurve.Atang.x) < 0.0001)
                {
                    Line3D L = new Line3D();
                    L.A = Value(ParamCurve.A.x, ParamCurve.A.y);
                    L.B = Value(ParamCurve.B.x, ParamCurve.B.y);
                    return L;
                }
            }
            MappedCurve C1 = new MappedCurve();
            C1.Curve2d = ParamCurve;
            C1.Mapper = this;

            return C1;
        }
        private double _Halfangle = System.Math.PI / 4;
        /// <summary>
        /// Gets or sets the half angle on the top.
        /// </summary>
        public double HalfAngle
        {
            get { return _Halfangle; }
            set { _Halfangle = value;
                Invalid = true;
            }
        }


        private double _Radius;
        /// <summary>
        /// Sets or gets the base radius of the cone.
        /// </summary>
        public double Radius
        {
            get { return _Radius; }
            set
            {
                _Radius = value;
                Invalid = true;
            }
        }
       
        /// <summary>
        /// Constuructor without parameters. It sets the <see cref="Surface.UPeriodicity"/> and the 
        /// <see cref="Surface.UFactor"/> to 2PI.
        /// </summary>
        public Cone()
        {
            SameSense = true;
            UPeriodicity = System.Math.PI * 2;
            UFactor = System.Math.PI * 2;
            UResolution = 32;
            VResolution = 1;
        }
        /// <summary>
        /// is a constructor with <b>Center</b>, <b>Radius</b>, <b>Height</b> and <b>HalfAngle</b>.
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="Radius"></param>
        /// <param name="Height"></param>
        /// <param name="HalfAngle"></param>
        public Cone(xyz Center, double Radius, double Height,double HalfAngle)
        {
            this.Center = Center;
            this.Radius = Radius;
            this.HalfAngle = HalfAngle;
            this.Height = Height;
            SameSense = true;
            UPeriodicity = System.Math.PI * 2;
            UFactor = System.Math.PI * 2;
            UResolution = 32;
            VResolution = 1;
        }
        /// <summary>
        /// is a constructor with <b>Radius</b>, <b>Height</b> and <b>HalfAngle</b>.
        /// For the center is 0,0,0 is taken.
        /// </summary>
        /// <param name="Radius">base radius.</param>
        /// <param name="Height">height.</param>
        public Cone(double Radius,double Height):this(new xyz(0,0,0),Radius,Height)
        {
           
        }
        /// <summary>
        /// is a constructor with <b>Center</b>, <b>Radius</b>, <b>Height</b>. The <b>halfangle</b> is so calculated, the a cone has a point as top.
        /// </summary>
        /// <param name="Center"></param>
        /// <param name="Radius"></param>
        /// <param name="Height"></param>
        public Cone(xyz Center,double Radius, double Height) : this(Center,Radius,Height, System.Math.Atan2(Radius, Height))
        {

        }
        /// <summary>
        /// overrides this method and calculates a point on the cone in u, v parameters. This point is
        /// nearest to the given <b>Point</b>.
        /// </summary>
        /// <param name="Point"> specifies a 3D-Points, for which a near point on the cone will be calculated and
        /// whose u and v value will be returned.</param>
        /// <returns>Resturns the u and v parameter of a near point on the Cone</returns>
        public override xy ProjectPoint(xyz Point)
        {
            xyz p = Base.Relativ(Point);
           double u1 = System.Math.Atan2(p.y, p.x);
            Double v = p.z;
            xyz Test = Value(u1 / UFactor, v/VFactor);
            if (Test.dist(Point)>0.05)
            { }
            return new xy(u1 / UFactor, v / VFactor);
       }

        /// <summary>
        /// Overrides the Value function and implements the cone calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz Value(double u, double v)
        {

            double Factor = Radius - v * VFactor * System.Math.Tan(HalfAngle);
            double x = System.Math.Cos(u * UFactor) * Factor;
            double y = System.Math.Sin(u * UFactor) * Factor;
            double z = v * VFactor;
            if (ZHeight(u, v) != 0)

                return Base.Absolut(new xyz(x, y, z) + Normal(u, v) * (ZHeight(u, v)));
            else
                return Base.Absolut(new xyz(x, y, z));

        }
        /// <summary>
        /// Overrides the vDerivation function and implements the cone calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz vDerivation(double u, double v)
        {
            double Factor = Radius - v * System.Math.Tan(Math.Abs(HalfAngle));
            double x = System.Math.Cos(u * UFactor) * (-VFactor * System.Math.Tan(HalfAngle)); ;
            double y = System.Math.Sin(u * UFactor) * (-VFactor * System.Math.Tan(HalfAngle)); ;

            return this.Base.Absolut(new xyz(x, y, VFactor)) - Base.BaseO;
        }

        /// <summary>
        /// Overrides the uDerivation function and implements the cone calulations.
        /// </summary>
        /// <param name="u">Specifies the u parameter int [0,1]</param>
        /// <param name="v">Specifies the v parameter int [0,1]</param>
        /// <returns>returns a xyz Point</returns>
        public override xyz uDerivation(double u, double v)
        {

            double Factor = Radius - v * System.Math.Tan(Math.Abs(HalfAngle)); ;
            if (Factor == 0) Factor = 1;
            double x = -System.Math.Sin(u * UFactor) * UFactor * Factor;
            double y = System.Math.Cos(u * UFactor) * UFactor * Factor;
            double z = 0;

            return this.Base.Absolut(new xyz(x, y, z)) - Base.BaseO;
       }


       
        /// <summary>
        /// Sets or gets the height of the cone by setting the <see cref="Surface.VFactor"/>
        /// </summary>
        public double Height
        {
            get
            {

                return VFactor;
            }


            set
            {
                Invalid = true;
                VFactor = value;
            }
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Cone Result = base.Copy() as Cone;
            Result.Radius = Radius;
            Result.HalfAngle = HalfAngle;
            Result.Height = Height;
            return Result;
        }
        bool schnittkegel(LineType L,out xyz Pt, out xy uv)
        {
            uv = new xy(0, 0);
            Pt = new xyz(0, 0, 0);
            double Rdh = Radius / Height;
            xyz U = L.P + L.Direction;
            L.P.z = Height - L.P.z;
            U.z = Height - U.z;
            L.Direction = U - L.P;
            double A = (L.Direction.x * L.Direction.x) + (L.Direction.y * L.Direction.y) - Rdh * Rdh * L.Direction.z * L.Direction.z;
            double B = 2 * L.P.x * L.Direction.x + 2 * L.P.y * L.Direction.y - 2 * Rdh * Rdh * L.P.z * L.Direction.z;
            double C = L.P.x * L.P.x + L.P.y * L.P.y - Rdh * Rdh * L.P.z * L.P.z;
            double Diskriminante = Math.Sqrt(B * B - 4 * A * C);
            if (Diskriminante < 0) return false;
            double t1 = (-B + Diskriminante) / (2 * A);
            double t2 = (-B - Diskriminante) / (2 * A);
            xyz P1 = L.P + L.Direction * t1;
            P1.z = Height - P1.z;
            xyz P2 = L.P + L.Direction * t2;
            P2.z = Height - P2.z;

            if (P1.dist(L.P) < P2.dist(L.P))
            {
                Pt = P1;
              uv=  ProjectPoint(Pt);
            }
            else
            {
                Pt = P2;
                uv = ProjectPoint(Pt);
             
            }

            return true;
        }
        /// <summary>
        /// overrides the <see cref="Surface.getCross(LineType, ref double, ref double)"/> method and calculates the cross point.
        /// </summary>
        /// <param name="L">is the crossing line.</param>
        /// <param name="u">is th u parapeter for <see cref="Value(double, double)"/></param>
        /// <param name="v">is th v parapeter for <see cref="Value(double, double)"/></param>
        /// <returns></returns>
        public override bool getCross(LineType L, ref double u, ref double v)
        {
            xyz Pt = new xyz(0, 0, 0);
            xy uv = new xy(0, 0);
            bool b= schnittkegel(L, out Pt, out uv);
            if (!b) return false;
            u = uv.x;
            v = uv.y;
            return true;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getCtrlPoints"/> method.
        /// </summary>
        /// <returns></returns>   
        public override xyz[,] getCtrlPoints()
        {
            if (BoundedCurves != null)
                Env = BoundedCurves[0].MaxRect;
            else
                Env = new RectangleF(new PointF(0, 0), new SizeF((float)Radius, (float)Height));
            double Max = Env.Y + Env.Height;
            double Min = Env.Y;
            double BaseRadius = Radius;
            double TopR =Radius- Height* System.Math.Tan(HalfAngle);

            TopR = Radius - (VFactor * (System.Math.Tan(HalfAngle) ));
            BaseRadius = Radius - (VFactor * (System.Math.Tan(HalfAngle) * (Min)));
            xyz[,] CP = new xyz[,]
          {
            {new xyz(BaseRadius,0,Min), new xyz(TopR,0,Max)},
            {new xyz(BaseRadius,BaseRadius,Min),   new xyz(TopR,TopR,Max)},
            {new xyz(0,BaseRadius,Min),             new xyz(0,TopR,Max)},
            {new xyz(-BaseRadius,BaseRadius,Min),    new xyz(-TopR,TopR,Max)},
            {new xyz(-BaseRadius,0,Min),            new xyz(-TopR,0,Max)},
            {new xyz(-BaseRadius,-BaseRadius,Min), new xyz(-TopR,-TopR,Max)},
            {new xyz(0,-BaseRadius,Min),              new xyz(0,-TopR,Max)},
            {new xyz(BaseRadius,-BaseRadius,Min),   new xyz(TopR,-TopR,Max)},
            {new xyz(BaseRadius,0,Min),               new xyz(TopR,0,Max)},
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
        public override double[] getUKnots()
        {
            double[] UKnots = new double[12];
            UKnots[0] = UKnots[1] = UKnots[2] = 0; ;
            UKnots[3] = UKnots[4] = 1f / 4f ;
            UKnots[5] = UKnots[6] = 1f / 2f ;
            UKnots[7] = UKnots[8] = 3f / 4f  ;
            UKnots[9] = UKnots[10] = UKnots[11] = 1  ;
          
            return UKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getVKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[] getVKnots()
        {
           
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