using System;

namespace Drawing3d
{
    /// <summary>
    /// Implements a torus, which is given by <see cref="Torus.InnerRadius"/> and <see cref="Torus.OuterRadius"/>.
    /// </summary>
    [Serializable]
    public class Torus : PrimitiveSurface
    {
        /// <summary>
        /// Constructor without parameter. It initializes UPeriodicity
        /// </summary>
        public Torus():base()
        {
            UPeriodicity = System.Math.PI * 2;
            VPeriodicity = System.Math.PI * 2;
            UResolution = 32;
            VResolution = 32;
            UFactor = 2 * System.Math.PI;
            VFactor = 2 * System.Math.PI;
        
        }
        /// <summary>
        /// is a constructor with <b>Inner radius</b> and <b>outer radius</b>.
        /// </summary>
        /// <param name="InnerRadius">Inner radius</param>
        /// <param name="OuterRadius">outer radius</param>
        public Torus(double InnerRadius, double OuterRadius)
            : this(InnerRadius, OuterRadius, new xyz(0,0,0))
        {
        }
        /// <summary>
        /// is a constructor with <b>Inner radius</b>, <b>outer radius</b> and <b>center</b>.
        /// </summary>
        /// <param name="InnerRadius">Inner radius</param>
        /// <param name="OuterRadius">outer radius</param>
        /// <param name="Center">the center.</param>
        public Torus(double InnerRadius, double OuterRadius, xyz Center) : this()
            {
            
            this.InnerRadius = InnerRadius;
            this.OuterRadius = OuterRadius;
            this.Center = Center;
        
        }
        /// <summary>
        /// the center of the torus.
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
        /// gets the nearest point on the torus and return this as (u,v) parameters.
        /// </summary>
        /// <param name="Point">Specifies the point for which a nearst point shold be calculated.</param>
        /// <returns>Returns the (u,v) parameters of the nearest point.</returns>
        public override xy ProjectPoint(xyz Point)
        {
 
            xyz p = Base.Relativ(Point);
        
            double u = System.Math.Atan2(p.y, p.x);
            double v = System.Math.Atan2(p.z, p.x * System.Math.Cos(u) + p.y * System.Math.Sin(u) - OuterRadius);

            if (OuterRadius < InnerRadius)
            {
                double u1 = u + Math.PI;
                double v1 = System.Math.Atan2(p.z, p.x * System.Math.Cos(u1) + p.y * System.Math.Sin(u1) - OuterRadius);
                xyz Q = Value(u / UFactor, v / VFactor);

                xyz Q1 = Value(u1 / UFactor, v1 / VFactor);
 
                 if (Q1.dist(p)<Q.dist(p))
                return new xy(u1 / UFactor, v1 / VFactor);
            }
 
            return new xy(u / UFactor, v / VFactor);



        }
        private double _OuterRadius;
        /// <summary>
        /// is the outer radius of the torus.
        /// </summary>
        public double OuterRadius
        {
            get { return _OuterRadius; }
            set { _OuterRadius = value;
                   Invalid = true;
            }

        }
        private double _InnerRadius;

        /// <summary>
        /// is the inner radius of the torus.
        /// </summary>
        public double InnerRadius
        {
            get { return _InnerRadius; }
            set { _InnerRadius = value;
                Invalid = true;
            }

        }
        /// <summary>
        /// Calculates the circle in the center of the torus.
        /// </summary>
        /// <param name="u">Specifies a parameter in [0,1] for which a circle points will be calculated.</param>
        /// <returns>Returns a point on the center circle of the torus.</returns>
        public xyz CenterCircle(double u)
        {
            double x = OuterRadius * System.Math.Cos(u * UFactor);
            double y = OuterRadius * System.Math.Sin(u * UFactor);
            double z = 0;
            return Base.Absolut(new xyz(x, y, z));
            //  double dummy = x;
        }
        /// <summary>
        /// Calculates the value of the <see cref="Torus"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The Evaluation for the torus.</returns>
        public override xyz Value(double u, double v)
        {
           
            double IR = InnerRadius;
            double OR = OuterRadius;
            double x = (IR * System.Math.Cos(v * VFactor) + OR) * System.Math.Cos(u * UFactor);
            double y = (IR * System.Math.Cos(v * VFactor) + OR) * System.Math.Sin(u * UFactor);
            double z = IR * System.Math.Sin(v * VFactor);
            if (ZHeight(u, v) > 0)
                return Base.Absolut(new xyz(x, y, z) + Normal(u, v) * ZHeight(u, v));

            return Base.Absolut(new xyz(x, y, z));
          

        }
        
        /// <summary>
        /// Calculates partial uderivation  of the <see cref="Torus"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The partial u-derivation for the torus.</returns>
        public override xyz uDerivation(double u, double v)
        {
            double x = -(InnerRadius * System.Math.Cos(v * VFactor) + OuterRadius) * System.Math.Sin(u * UFactor) * UFactor;
            double y = (InnerRadius * System.Math.Cos(v * VFactor) + OuterRadius) * System.Math.Cos(u * UFactor) * UFactor;
            double z = 0;
            return (Base.Absolut(new xyz(x, y, z)) - Base.BaseO);
        }

        /// <summary>
        /// Calculates partial v-derivation  of the <see cref="Torus"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The partial v-derivation for the torus.</returns>
       public override xyz vDerivation(double u, double v)
        {
           
            double x = -InnerRadius * System.Math.Sin(v * VFactor) * VFactor * System.Math.Cos(u * UFactor);
            double y = -InnerRadius * System.Math.Sin(v * VFactor) * VFactor * System.Math.Sin(u * UFactor);
            double z = InnerRadius * System.Math.Cos(v * VFactor) * VFactor;
            return Base.Absolut(new xyz(x, y, z)) - Base.BaseO;
        }
        /// <summary>
        /// overrides the <see cref="Surface.Normal(double, double)"/> method.
        /// </summary>
        /// <param name="u">the parameter u.</param>
        /// <param name="v">the parameter v.</param>
        /// <returns>the normal vector.</returns>
        public override xyz Normal(double u, double v)
        {

            double x = (Math.Cos(v * VFactor) ) * Math.Cos(u * UFactor);
            double y = (System.Math.Cos(v * VFactor) ) * System.Math.Sin(u * UFactor);
            double z = System.Math.Sin(v * VFactor);
            {
                if (SameSense)
                    return (Base.Absolut(new xyz(x, y, z)) - Base.BaseO);
                else
                    return (Base.Absolut(new xyz(x, y, z)) - Base.BaseO) * (-1);
            }
 
        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            Torus Result = base.Copy() as Torus;
            Result.InnerRadius = InnerRadius;
            Result.OuterRadius = OuterRadius;

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
            return 2;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getUKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override xyz[,] getCtrlPoints()
        {
           
           
            double IRSqrt = InnerRadius;// / System.Math.Sqrt(2);
            xyz[,] CP = new xyz[,]
            {
               {
                   new xyz(OuterRadius+InnerRadius,0,0),
                   new xyz(OuterRadius+InnerRadius,0,InnerRadius),
                   new xyz(OuterRadius,0,InnerRadius),
                   new xyz(OuterRadius-InnerRadius,0,InnerRadius),
                   new xyz(OuterRadius-InnerRadius,0,0),
                   new xyz(OuterRadius-InnerRadius,0,-InnerRadius),
                   new xyz(OuterRadius,0,-InnerRadius),
                   new xyz(OuterRadius+InnerRadius,0,-InnerRadius),
                   new xyz(OuterRadius+InnerRadius,0,0),
               },

                {
                   new xyz(OuterRadius+InnerRadius,OuterRadius+InnerRadius,0),
                   new xyz(OuterRadius+InnerRadius,OuterRadius+InnerRadius,InnerRadius),
                   new xyz(OuterRadius,OuterRadius,InnerRadius),
                   new xyz(OuterRadius-InnerRadius,OuterRadius-InnerRadius,InnerRadius),
                   new xyz(OuterRadius-InnerRadius,OuterRadius-InnerRadius,0),
                   new xyz(OuterRadius-InnerRadius,OuterRadius-InnerRadius,-InnerRadius),
                   new xyz(OuterRadius,OuterRadius,-InnerRadius),
                   new xyz(OuterRadius+InnerRadius,OuterRadius+InnerRadius,-InnerRadius),
                   new xyz(OuterRadius+InnerRadius,OuterRadius+InnerRadius,0),

                },

                {
                new xyz(0,OuterRadius+InnerRadius,0),
                new xyz(0,OuterRadius+InnerRadius,InnerRadius),
                new xyz(0,OuterRadius,InnerRadius),
                new xyz(0,OuterRadius-InnerRadius,InnerRadius),
                new xyz(0,OuterRadius-InnerRadius,0),
                new xyz(0,OuterRadius-InnerRadius,-InnerRadius),
                new xyz(0,OuterRadius,-InnerRadius),
                new xyz(0,OuterRadius+InnerRadius,-InnerRadius),
                new xyz(0,OuterRadius+InnerRadius,0),

                },

                {
                new xyz(-(OuterRadius+InnerRadius),OuterRadius+InnerRadius,0),
                new xyz(-(OuterRadius+InnerRadius),OuterRadius+InnerRadius,InnerRadius),
                new xyz(-OuterRadius,OuterRadius,InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),OuterRadius-InnerRadius,InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),OuterRadius-InnerRadius,0),
                new xyz(-(OuterRadius-InnerRadius),OuterRadius-InnerRadius,-InnerRadius),
                new xyz(-OuterRadius,OuterRadius,-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),OuterRadius+InnerRadius,-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),OuterRadius+InnerRadius,0),
                },

                {
                new xyz(-(OuterRadius+InnerRadius),0,0),
                new xyz(-(OuterRadius+InnerRadius),0,InnerRadius),
                new xyz(-OuterRadius,0,InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),0,InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),0,0),
                new xyz(-(OuterRadius-InnerRadius),0,-InnerRadius),
                new xyz(-OuterRadius,0,-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),0,-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),0,0),
                },

                {
                new xyz(-(OuterRadius+InnerRadius),-(OuterRadius+InnerRadius),0),
                new xyz(-(OuterRadius+InnerRadius),-(OuterRadius+InnerRadius),InnerRadius),
                new xyz(-OuterRadius,-(OuterRadius),InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),-(OuterRadius-InnerRadius),InnerRadius),
                new xyz(-(OuterRadius-InnerRadius),-(OuterRadius-InnerRadius),0),
                new xyz(-(OuterRadius-InnerRadius),-(OuterRadius-InnerRadius),-InnerRadius),
                new xyz(-OuterRadius,-OuterRadius,-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),-(OuterRadius+InnerRadius),-InnerRadius),
                new xyz(-(OuterRadius+InnerRadius),-(OuterRadius+InnerRadius),0),
                },

                {
                new xyz(0,-(OuterRadius+InnerRadius),0),
                new xyz(0,-(OuterRadius+InnerRadius),InnerRadius),
                new xyz(0,-OuterRadius,InnerRadius),
                new xyz(0,-(OuterRadius-InnerRadius),InnerRadius),
                new xyz(0,-(OuterRadius-InnerRadius),0),new xyz(0,-(OuterRadius-InnerRadius),-InnerRadius),
                new xyz(0,-OuterRadius,-InnerRadius),
                new xyz(0,-(OuterRadius+InnerRadius),-InnerRadius),
                new xyz(0,-(OuterRadius+InnerRadius),0),
                },

                {
                new xyz(OuterRadius+InnerRadius,-(OuterRadius+InnerRadius),0),
                new xyz(OuterRadius+InnerRadius,-(OuterRadius+InnerRadius),InnerRadius),
                new xyz(OuterRadius,-OuterRadius,InnerRadius),
                new xyz(OuterRadius-InnerRadius,-(OuterRadius-InnerRadius),InnerRadius),
                new xyz(OuterRadius-InnerRadius,-(OuterRadius-InnerRadius),0),
                new xyz(OuterRadius-InnerRadius,-(OuterRadius-InnerRadius),-InnerRadius),
                new xyz(OuterRadius,-OuterRadius,-InnerRadius),
                new xyz(OuterRadius+InnerRadius,-(OuterRadius+InnerRadius),-InnerRadius),
                new xyz(OuterRadius+InnerRadius,-(OuterRadius+InnerRadius),0),
                },

                {
                new xyz(OuterRadius+InnerRadius,0,0),
                new xyz(OuterRadius+InnerRadius,0,InnerRadius),
                new xyz(OuterRadius,0,InnerRadius),
                new xyz(OuterRadius-InnerRadius,0,InnerRadius),
                new xyz(OuterRadius-InnerRadius,0,0),
                new xyz(OuterRadius-InnerRadius,0,-InnerRadius),
                new xyz(OuterRadius,0,-InnerRadius),
                new xyz(OuterRadius+InnerRadius,0,-InnerRadius),
                new xyz(OuterRadius+InnerRadius,0,0),
                },
            };


            for (int i = 0; i < CP.GetLength(0); i++)
                for (int j = 0; j < CP.GetLength(1); j++)
                        CP[i, j] = Base.Absolut(CP[i, j]);
           
          
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
            UKnots[3] = UKnots[4] = 1d / 4d * System.Math.PI * 2 / UFactor;
            UKnots[5] = UKnots[6] = 1d / 2d * System.Math.PI * 2 / UFactor;
            UKnots[7] = UKnots[8] = 3d / 4d * System.Math.PI * 2 / UFactor;
            UKnots[9] = UKnots[10] = UKnots[11] = 1 * System.Math.PI * 2 / UFactor;
            return UKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getVKnots"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[] getVKnots()
        {
            double[] VKnots = new double[12];
            VKnots[0] = VKnots[1] = VKnots[2] = 0;
            VKnots[3] = VKnots[4] = 1d / 4d * System.Math.PI * 2 / VFactor; ;
            VKnots[5] = VKnots[6] = 1d / 2d * System.Math.PI * 2 / VFactor;
            VKnots[7] = VKnots[8] = 3d / 4d * System.Math.PI * 2 / VFactor;
            VKnots[9] = VKnots[10] = VKnots[11] = 1 * System.Math.PI * 2 / VFactor;
            return VKnots;
        }
        /// <summary>
        /// implements the <see cref="INurbs3d.getWeights"/> method.
        /// </summary>
        /// <returns></returns>
        public override double[,] getWeights()
        {
            double[,] Weights = new double[9, 9];
            double ausgleich = 2;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if ((i) / 2 * 2 != i)
                        Weights[i, j] = System.Math.Sqrt(2) / ausgleich;//  System.Math.Sqrt(2) / 2d;
                    else
                        Weights[i, j] = 1;

                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                   if ((j) / 2 * 2 != j)
                    {
                        if (Weights[i, j] != 1) Weights[i, j] = 0.5;
                        else
                            Weights[i, j] = System.Math.Sqrt(2) / ausgleich;
                    }

                }
            }
            return Weights;
        }
    }

}