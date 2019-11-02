using System;
using System.Collections.Generic;




namespace Drawing3d
{
    /// <summary>
    /// Sphere is a surface, which implements a sphere with the origin as center and 
    /// a <see cref="Radius"/>.
    /// Sphere overrides the abstract functions <see cref="Surface.Value(double, double)"/>, <see cref="Surface.uDerivation(double, double)"/> and <see cref="Surface.Value(double, double)"/>, <see cref="Surface.uDerivation(double, double)"/><br/>
    /// </summary>
    [Serializable]
    public class Sphere : PrimitiveSurface
    {
        /// <summary>
        /// Constructor without parameter. I initialize UFactor with 2*PI and VFactor with PI.
        /// </summary>
        public Sphere()
        {
            UFactor = 2 * System.Math.PI;
            VFactor = System.Math.PI;
            UPeriodicity = 2 * System.Math.PI;
            VPeriodicity = 2 * System.Math.PI;
            UResolution = 32;
            VResolution = 16;
            Radius = 1;
         
        }
        /// <summary>
        /// is a constructor with the <b>Radius</b>. The center is 0,0,0.
        /// </summary>
        /// <param name="Radius">the radius.</param>
         public Sphere(double Radius):this()
        {
            this.Radius = Radius;
            Invalid = true;
        }
        /// <summary>
        /// is a constructor with the <b>Center</b> and the <b>Radius</b>
        /// </summary>
        /// <param name="Radius">the radius.</param>
        /// <param name="Center">the center.</param>
        public Sphere(xyz Center,double Radius) : this()
        {
            this.Center = Center;
            this.Radius = Radius;
        }
   /// <summary>
   /// overrides <see cref="Surface.ProjectPoint(xyz)"/>
   /// </summary>
   /// <param name="Point">u and v parameters for <see cref="Surface.Value(double, double)"/>.</param>
   /// <returns></returns>
        public override xy ProjectPoint(xyz Point)
        {

            xyz p = Base.Relativ(Point);
            double u = ((System.Math.Atan2(p.Y, p.x))  - 0.5 * System.Math.PI) / UFactor;
            double v = System.Math.Atan2(System.Math.Sqrt(p.x * p.x + p.y * p.y), -p.z) / VFactor;
            
            if (u <0)
            { u = u + 1; }
           
            return new xy(u, v);
      }
  
        private double FRadius;
        /// <summary>
        /// retrieves or writes the radius of the sphere
        /// </summary>
        public double Radius
        {
            get { return FRadius; }
            set
            {
                FRadius = value;
                RefreshEnvBox();
            }


        }
        /// <summary>
        /// gets and sets the center of the sphere.
        /// </summary>
        public xyz Center
        {
            get { return Base.BaseO; }
            set
            {
                Base B = this.Base;
                B.BaseO = value;
                Base = B;
            
                RefreshEnvBox();
                Invalid = true;

            }
        }
        /// <summary>
        /// overrides the <see cref="Surface.OnDraw(OpenGlDevice)"/> method.
        /// </summary>
        /// <param name="Device"></param>
        protected override void OnDraw(OpenGlDevice Device)
        {
            //if ((BoundedCurves == null) || (BoundedCurves.Count == 0))
            //    Device.drawSphere(Center, Radius, UResolution, VResolution);
            //else
                base.OnDraw(Device);
        }
        /// <summary>
        /// overides the <see cref="Surface.RefreshEnvBox"/> method.
        /// </summary>
        protected override void RefreshEnvBox()
        {
            EnvBox = new Box(new xyz(-Radius + Center.x, -Radius + Center.y, -Radius + Center.z),
                 new xyz(2 * Radius, 2 * Radius, 2 * Radius)
                      );
         }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Sphere Sphere = base.Copy() as Sphere;
            Sphere.Radius = Radius;
          
            return Sphere;
        }
        /// <summary>
        /// paramtisizes the sphere for the to parameters u and v which are given in the interval [0, 1]
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>Spherecoordinate</returns>
        public override xyz Value(double u, double v)
        {

            double x = Radius * System.Math.Cos((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Sin(v * VFactor);
            double y = Radius * System.Math.Sin((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Sin(v * VFactor);
            double z = -Radius * System.Math.Cos(v * VFactor);
            if (ZHeight(u, v) > 0)
                return Base.Absolut(new xyz(x, y, z) +
               new xyz(
               System.Math.Cos((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Sin(v * VFactor),
               System.Math.Sin((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Sin(v * VFactor),
              -System.Math.Cos(v * VFactor))*ZHeight(u,v));
            return Base.Absolut(new xyz(x, y, z));

        }
        /// <summary>
        /// Calculates the partial uDerivation
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>value of the partial uDerivation</returns>
        public override xyz uDerivation(double u, double v)
        {


            double x = -Radius * System.Math.Sin((u) * UFactor + 0.25 * System.Math.PI * 2) * UFactor * System.Math.Sin(v * VFactor);
            double y = Radius * System.Math.Cos((u) * UFactor + 0.25 * System.Math.PI * 2) * UFactor * System.Math.Sin(v * VFactor);
            double z = 0;
            return Base.Absolut(new xyz(x, y, z)) - Base.BaseO;



        }
        /// <summary>
        /// Calculates the partial vDerivation
        /// </summary>
        /// <param name="u">first parameter</param>
        /// <param name="v">second parameter</param>
        /// <returns>value of the partial vDerivation</returns>

        public override xyz vDerivation(double u, double v)
        {


            double x = Radius * System.Math.Cos((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Cos(v * VFactor);
            double y = Radius * System.Math.Sin((u) * UFactor + 0.25 * System.Math.PI * 2) * System.Math.Cos(v * VFactor);
            double z = Radius * System.Math.Sin(v * VFactor);
            return Base.Absolut(new xyz(x, y, z)) - Base.BaseO;
           

        }
        /// <summary>
        /// Return the normalvector of a sphere
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public override xyz Normal(double u, double v)
        {
            if (SameSense)
                return (Value(u, v)) - Base.BaseO;
            else
                return (Value(u, v) - Base.BaseO
                    ) * (-1);
        }
        /// <summary>
        /// overrides <see cref="Surface.GetPointAndNormal(double, double, ref xyzf, ref xyzf)"/>
        /// </summary>
        /// <param name="x">first parameter.</param>
        /// <param name="y">second parameter.</param>
        /// <param name="Point">point on the sphere.</param>
        /// <param name="Normal">normal vector of the sphere.</param>
        public override void GetPointAndNormal(double x, double y, ref xyzf Point, ref xyzf Normal)
        {
            xyz _Point = Value(x, y);
            if (!SameSense)
                Normal = ((_Point - Base.BaseO).normalized() * (-1)).toXYZF();
            else
                Normal = (_Point - Base.BaseO).normalized().toXYZF();
            Point = _Point.toXYZF();
        }
        /// <summary>
        /// Overrides the method <see cref="Surface.getCross"/> and implements a method for get the crosspoint with a Line <b>L</b>, which is
        /// nearer to L.Q.
        /// </summary>
        /// <param name="L">The line, which will be crossed with the sphere</param>
        /// <param name="u">gets the u parameter of the cross point</param>
        /// <param name="v">gets the v parameter of the cross point</param>
        /// <returns>false if there is no cross point.</returns>
        public override bool getCross(LineType L, ref double u, ref double v)
        {

            xyz Direction = L.Direction.normalized();
            xyz P = Base.Relativ(L.P);
            xyz NDirection = ((Direction & P) & (Direction));

            double bb = NDirection.length();
            xyz S = new xyz(0, 0, 0);
            if (Radius > bb)
                S = NDirection - Direction * System.Math.Sqrt(Radius * Radius - bb * bb);
            else
                return false;
            xy Param = this.ProjectPoint(Base.Absolut(S));
            u = Param.x;
            v = Param.Y;
            return true;
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
            return 2;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUKnots"/> method.
        /// </summary>
        /// <returns></returns>

        public override xyz[,] getCtrlPoints()
        {
            double Factor = 0.0000;
            xyz[,] ControlPoints = new xyz[,]  {

             {new xyz(0,Factor,-Radius),new xyz(0,Radius,-Radius),new xyz(0,Radius,0),new xyz(0,Radius,Radius),new xyz(0,Factor,Radius)},
             {new xyz(-Factor,Factor,-Radius),new xyz(-Radius,Radius,-Radius),new xyz(-Radius,Radius,0),new xyz(-Radius,Radius,Radius),new xyz(-Factor,Factor,Radius)},
             {new xyz(-Factor,0,-Radius),new xyz(-Radius,0,-Radius),new xyz(-Radius,0,0),new xyz(-Radius,0,Radius),new xyz(-Factor,0,Radius)},
             {new xyz(-Factor,-Factor,-Radius),new xyz(-Radius,-Radius,-Radius),new xyz(-Radius,-Radius,0),new xyz(-Radius,-Radius,Radius),new xyz(-Factor,-Factor,Radius)},
             {new xyz(0,-Factor,-Radius),new xyz(0,-Radius,-Radius),new xyz(0,-Radius,0),new xyz(0,-Radius,Radius),new xyz(0,-Factor,Radius)},
             {new xyz(Factor,-Factor,-Radius),new xyz(Radius,-Radius,-Radius),new xyz(Radius,-Radius,0),new xyz(Radius,-Radius,Radius),new xyz(Factor,-Factor,Radius)},
             {new xyz(Factor,0,-Radius),new xyz(Radius,0,-Radius),new xyz(Radius,0,0),new xyz(Radius,0,Radius),new xyz(Factor,0,Radius)},
             {new xyz(Factor,Factor,-Radius),new xyz(Radius,Radius,-Radius),new xyz(Radius,Radius,0),new xyz(Radius,Radius,Radius),new xyz(Factor,Factor,Radius)},
             {new xyz(0,Factor,-Radius),new xyz(0,Radius,-Radius),new xyz(0,Radius,0),new xyz(0,Radius,Radius),new xyz(0,Factor,Radius)}
            };
            for (int i = 0; i < ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < ControlPoints.GetLength(1); j++)
                {
                    ControlPoints[i, j] = Base.Absolut(ControlPoints[i, j]);
                }
            }
          
            return ControlPoints;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUKnots"/> method.
        /// </summary>
        /// <returns></returns>

        public override double[] getUKnots()
        {
            double[] UKnots = new double[12];
            UKnots[0] = UKnots[1] = UKnots[2] = 0;
            UKnots[3] = UKnots[4] = 1f / 4f * System.Math.PI * 2 / UFactor;// *0.995; ;
            UKnots[5] = UKnots[6] = 1f / 2f * System.Math.PI * 2 / UFactor; ;
            UKnots[7] = UKnots[8] = 3f / 4f * System.Math.PI * 2 / UFactor;// * 1.005; ;
            UKnots[9] = UKnots[10] = UKnots[11] = 1f * System.Math.PI * 2 / UFactor; ;

            return UKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getVKnots"/> method.
        /// </summary>
        /// <returns></returns>

        public override double[] getVKnots()
        {
            double[] VKnots = new double[8];
            VKnots[0] = VKnots[1] = VKnots[2] = 0.0;
            VKnots[3] = VKnots[4] = 0.5 * System.Math.PI / VFactor;
            VKnots[5] = VKnots[6] = VKnots[7] = 1 * System.Math.PI / VFactor;

            return VKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getWeights"/> method.
        /// </summary>
        /// <returns></returns>

        public override double[,] getWeights()
        {
            double[,] Weights = new double[9, 5];
            double Sqrt2div2 = 1 / System.Math.Sqrt(2);
        

            for (int i = 0; i < 9; i++)
            {
                Weights[i, 0] = 1;//0/0.00001/-40}	Drawing3d.Math.xyz
                Weights[i, 1] = Sqrt2div2;//0/40/-40}	Drawing3d.Math.xyz
                Weights[i, 2] = 1;//0/40/0}	Drawing3d.Math.xyz
                Weights[i, 3] = Sqrt2div2;//0/40/40}	Drawing3d.Math.xyz
                Weights[i, 4] = 1;//0/0.00001/40}	Drawing3d.Math.xyz
                if (i + 1 < 9)
                {
                    Weights[i + 1, 0] = Sqrt2div2;//-0.00001/0.00001/-40}	Drawing3d.Math.xyz
                    Weights[i + 1, 1] = 0.5;//-40/40/-40}	Drawing3d.Math.xyz
                    Weights[i + 1, 2] = Sqrt2div2;//-40/40/0}	Drawing3d.Math.xyz
                    Weights[i + 1, 3] = 0.5;//-40/40/40}	Drawing3d.Math.xyz
                    Weights[i + 1, 4] = Sqrt2div2;
                }
                i++;
            }

          return Weights;
        }
        

    }
}