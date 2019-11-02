using System;

namespace Drawing3d
{
    /// <summary>
    /// This class inherits from <see cref="BezierSurface"/>  uses its <see cref="BezierSurface.ControlPoints"/>
    /// and additionally the <b>Knots</b> to evaluate a 3D BSpline. <see cref="BezierSurface"/> has a degree for
    /// u and v parameters: <see cref="UDegree"/> and <see cref="VDegree"/>
    /// The default is 2.
    /// <remarks>Usually: the Number of Controlpoints plus the degree plus 1 gives the number of Knots
    /// </remarks>
    /// </summary>
    [Serializable]
    public class BSplineSurface : BezierSurface, INurbs3d
    {
        /// <summary>
        /// overrides the <see cref="Surface.RefreshEnvBox"/> for the calculation of the enclosing <see cref="Surface.EnvBox"/>.
        /// </summary>
        protected override void RefreshEnvBox()
        {
            xyz[] P = new xyz[ControlPoints.GetLength(0) * ControlPoints.GetLength(1)];
            int id = 0;
            for (int i = 0; i < ControlPoints.GetLength(0); i++)
            {
                for (int j = 0; j < ControlPoints.GetLength(1); j++)
                {
                    P[id] = ControlPoints[i, j];
                    id++;
                }
            }
            EnvBox = Box.GetEnvBox(P);
        }

        /// <summary>
        /// overrides the <see cref="Surface.CheckPeriodic"/> method and set the <see cref="Surface.UPeriodicity"/> and the <see cref="Surface.VPeriodicity"/>.
        /// </summary>
        protected override void CheckPeriodic()
        {
            double Fac = 0.000001;
            Rectangled R = DefinitionDomain;
            try
            {

                if (Value(R.X, R.Y).dist(Value(R.X + R.Width, R.Y)) < 20 * Fac)
                    UPeriodicity = R.Width;
                else

                    UPeriodicity = 1000000;
            }
            catch (Exception)
            {


            }

            if (Value(R.X + R.Width, R.Y).dist(Value(R.X + R.Width, R.Y + R.Height)) < 20 * Fac)
            {
                VPeriodicity = R.Height;
            }
            else
                VPeriodicity = 1000000;

            if (Value(R.X, R.Y).dist(Value(R.X + R.Width / 4, R.Y)) < 20 * Fac)
            {
                DownPol = new Pol(true, R.Y);
            }
            else
                DownPol = new Pol(false, -1);
            if (Value(R.X, R.Y + R.Height).dist(Value(R.X + R.Width / 4, R.Y)) < 20 * Fac)
            {
                UpPol = new Pol(true, R.Y + R.Height);
            }
            else
                UpPol = new Pol(false, -1);
            if (Value(R.X, R.Y).dist(Value(R.X, R.Y + R.Height)) < 20 * Fac)
            {
                LeftPol = new Pol(true, R.Y);
            }
            else
                LeftPol = new Pol(false, -1);
            if (Value(R.X + R.Width, R.Y).dist(Value(R.X + R.Width, R.Y + R.Height)) < 20 * Fac)
            {
                RightPol = new Pol(true, R.X + R.Width);
            }
            else
                RightPol = new Pol(false, -1);
            if (UpPol.Enable & DownPol.Enable & LeftPol.Enable & RightPol.Enable)
            {
            }

        }
        
        double[] NormalizeKnots(double[] value)
        {
            if (value.Length == 0) return value;
            double[] Result = new double[value.Length];
            double smallest = value[0];
            double with = value[value.Length - 1] - smallest;
            for (int i = 0; i < value.Length; i++)
                Result[i] = (value[i] - smallest) / with;
           return Result;
        }


       
        double[][] U_Coeff = null;
        double[][] V_Coeff = null;
        void ReCalculateBuffer()
        {
            Rectangled R = DefinitionDomain;
            U_Coeff = new double[100][];
            double Step = (float)R.Width / 100f;
            for (int i = 0; i < 100; i++)
            {
               

                U_Coeff[i] = Utils.CoeffSpline(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity, R.Left + i * Step);

            }

            V_Coeff = new double[100][];
            Step = (float)R.Height / 100f;
            for (int i = 0; i < 100; i++)
            {
               

                V_Coeff[i] = Utils.CoeffSpline(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, R.Bottom + i * Step);

            }
           
        }

   
        private int _UDegree = 2;
        /// <summary>
        /// The degree related to the parameter u. The default is 2.
        /// </summary>
        public int UDegree
        {
            get
            {
                 return _UDegree;
            }
            set
            {
                Invalid = true;
                _UDegree = value;
            }
        }
        private int _VDegree = 2;
        /// <summary>
        /// The degree related to the parameter v. The default is 2.
        /// </summary>
        public int VDegree
        {
            get { return _VDegree; }
            set
            {
                Invalid = true;
                _VDegree = value;
             
            }
        }
        private Double[] _UKnots;
        /// <summary>
        /// Defines the knots related to the parameter u.
        /// </summary>
        public Double[] UKnots
        {
            get { return _UKnots; }
            set {
                 Invalid = true;
                _UKnots = NormalizeKnots(value); }
        }
        private Double[] _VKnots;
        /// <summary>
        /// Defines the knots related to the parameter v.
        /// </summary>
        public Double[] VKnots
        {
            get { return _VKnots; }
            set {
                Invalid = true;
                _VKnots = NormalizeKnots(value);
               
                }
        }

        
        /// <summary>
        /// sets the <see cref="Utils.DefaultKnots(int, int)"/>.
        /// </summary>
        public void SetDefaultKnots()
        {
            UKnots = Utils.DefaultKnots(ControlPoints.GetLength(0), UDegree);
            VKnots = Utils.DefaultKnots(ControlPoints.GetLength(1), VDegree);
        }
        /// <summary>
        /// Calculates the value of the <see cref="BSplineSurface"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The Evaluation of the bspline.</returns>
        public override xyz Value(double u, double v)
        {
            try
            {
            if (UKnots==null)
                UKnots =Utils.StandardKnots(ControlPoints.GetLength(0), UDegree);
            if (VKnots == null)
                VKnots = Utils.StandardKnots(ControlPoints.GetLength(1), VDegree);
            Rectangled R = DefinitionDomain;
         
            xyz Result = new xyz(0, 0, 0);
        
            int UCount = ControlPoints.GetLength(0);
            int VCount = ControlPoints.GetLength(1);
            double[] UCoeff = Utils.CoeffSpline(UKnots, UCount, UDegree, UPeriodicity, v);
            double[] VCoeff = Utils.CoeffSpline(VKnots, VCount, VDegree, VPeriodicity, u);
            int FirstUCoeffNonZero = 0;
            for (int i = 0; i < UCoeff.Length; i++)
                if (UCoeff[i] != 0)
                {
                    FirstUCoeffNonZero = i;
                    break;
                }

            int FirstVCoeffNonZero = 0;
            for (int i = 0; i < VCoeff.Length; i++)
                if (VCoeff[i] != 0)
                {
                    FirstVCoeffNonZero = i;
                    break;
                }
            int UntilU = Math.Min(FirstUCoeffNonZero + UDegree, UCoeff.Length - 1);
            int UntilV = Math.Min(FirstVCoeffNonZero + VDegree, VCoeff.Length - 1);
                
                for (int i = 0; i < UCoeff.Length; i++)
                    for (int k = 0; k < VCoeff.Length; k++)

                    {

                        Result = Result + ControlPoints[i, k] * (UCoeff[i] * VCoeff[k]);

                    }
                if (ZHeight(u,v)!=0)
                return Base.Absolut(Result+Normal(u,v)*ZHeight(u,v));
                else
                    return Base.Absolut(Result);
            }
            catch (Exception )
            {
                if ((ControlPoints.GetLength(0) + UDegree + 1) != UKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the UKnots is wrong. . It must be UKnots.Length=ControlPoints.GetLength(0) + UDegree + 1");
                else
                if ((ControlPoints.GetLength(1) + VDegree + 1) != VKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the VKnots is wrong. It must be VKnots.Length=ControlPoints.GetLength(1) + VDegree + 1");
 
                return new xyz(0, 0, 0);
            }
        }
       
    Rectangled DefinitionDomain = new Rectangled(0, 0, 1, 1);
        /// <summary>
        /// Calculates the partial u derivation of the <see cref="BSplineSurface"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The partial u derivation.</returns>
        public override xyz uDerivation(double u, double v)
        {
            try
            {
           xyz Result = new xyz(0, 0, 0);
            double[] VCoeff = Utils.CoeffSpline(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, u);
            Utils.SplineExValue[] UCoeffDerived = Utils.CoeffSplineEx(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity,v);


                




                int FirstUCoeffNonZero = 0;
            for (int i = 0; i < UCoeffDerived.Length; i++)
                if (UCoeffDerived[i].Value != 0)
                {
                    FirstUCoeffNonZero = i;
                    break;
                }

            int FirstVCoeffNonZero = 0;
            for (int i = 0; i < VCoeff.Length; i++)
                if (VCoeff[i] != 0)
                {
                    FirstVCoeffNonZero = i;
                    break;
                }

                int UntilU = Math.Min(FirstUCoeffNonZero + UDegree, UCoeffDerived.Length - 1);
                int UntilV = Math.Min(FirstVCoeffNonZero + VDegree, VCoeff.Length - 1);
                // Derivation without Weigths
                for (int i = FirstUCoeffNonZero; i<= UntilU; i++)
                for (int k = FirstVCoeffNonZero; k <= UntilV; k++)

                {
                   
                    Result = Result + ControlPoints[i, k] * (UCoeffDerived[i].Derivation * VCoeff[k]);

            }

            // Derivation width Weigth

            // noch machen
          
            return (Base.Absolut(Result) - Base.BaseO);
            }
           
                 catch (Exception)
            {
                if ((ControlPoints.GetLength(0) + UDegree + 1) != UKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the UKnots is wrong.");
                else
                if ((ControlPoints.GetLength(1) + VDegree + 1) != VKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the VKnots is wrong.");
                System.Windows.Forms.Application.Exit();
                return new xyz(0, 0, 0);
            }
           
        }
        /// <summary>
        /// Calculates the partial v derivation of the <see cref="BSplineSurface"/>.
        /// </summary>
        /// <param name="u">Parameter u.</param>
        /// <param name="v">Parameter v.</param>
        /// <returns>The partial v derivation.</returns>     
        public override xyz vDerivation(double u, double v)
        {
            try
            {
            xyz Result = new xyz(0, 0, 0);
            double[] UCoeff = Utils.CoeffSpline(UKnots, ControlPoints.GetLength(0), UDegree, UPeriodicity, v);
            double[] VCoeff = Utils.CoeffSpline(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, u);
            Utils.SplineExValue[] VCoeffDerived = Utils.CoeffSplineEx(VKnots, ControlPoints.GetLength(1), VDegree, VPeriodicity, u);

            int FirstUCoeffNonZero = 0;
            for (int i = 0; i < UCoeff.Length; i++)
                if (UCoeff[i] != 0)
                {
                    FirstUCoeffNonZero = i;
                    break;
                }

            int FirstVCoeffNonZero = 0;
            for (int i = 0; i < VCoeffDerived.Length; i++)
                if (VCoeffDerived[i].Value != 0)
                {
                    FirstVCoeffNonZero = i;
                    break;
                }
                int UntilU = Math.Min(FirstUCoeffNonZero + UDegree, UCoeff.Length - 1);
                int UntilV = Math.Min(FirstVCoeffNonZero + VDegree, VCoeffDerived.Length - 1);
                // Derivation without Weigths
                for (int i = FirstUCoeffNonZero; i <= UntilU; i++)
                    for (int k = FirstVCoeffNonZero; k <= UntilV; k++)
                       
                {
                    
                    Result = Result + ControlPoints[i, k] * (VCoeffDerived[k].Derivation * UCoeff[i]);

            }

            // Derivation width Weigth

            // noch machen
          
            return Base.Absolut(Result) - Base.BaseO;
            }
            catch (Exception)
            {
                if ((ControlPoints.GetLength(0) + UDegree + 1) != UKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the UKnots is wrong.");
                else
                  if ((ControlPoints.GetLength(1) + VDegree + 1) != VKnots.Length)
                    System.Windows.Forms.MessageBox.Show("the definition of the VKnots is wrong.");
                System.Windows.Forms.Application.Exit();
                return new xyz(0, 0, 0);
            }

        }
        /// <summary>
        /// overrides the <see cref="Surface.Copy"/> method and sets the belonging fields.
        /// </summary>
        /// <returns>the copied surface</returns>
        public override Surface Copy()
        {
            BSplineSurface Result = base.Copy() as BSplineSurface;
            Result.UKnots =UKnots.Clone() as double[];
            Result.VKnots = VKnots.Clone() as double[];
            Result.UDegree = UDegree;
            Result.VDegree = VDegree;
            return Result;
        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns><see cref="UDegree"/></returns>
        public int getUDegree()
        {
            return UDegree;
        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns><see cref="VDegree"/></returns>
        public int getVDegree()
        {
            return VDegree;

        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns><see cref="BezierSurface.ControlPoints"/></returns>
        public xyz[,] getCtrlPoints()
        {
            return this.ControlPoints;
        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns><see cref="UKnots"/></returns>
        public double[] getUKnots()
        {
            return this.UKnots;

        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns><see cref="VKnots"/></returns>
        public double[] getVKnots()
        {
            return this.VKnots;

        }
        /// <summary>
        /// for the definistion of <see cref="INurbs3d"/>
        /// </summary>
        /// <returns>constant 1.</returns>
        double[,] INurbs3d.getWeights()
        {
            long Acount = ControlPoints.GetLongLength(0);
            long BCount = ControlPoints.GetLongLength(1);
            double[,] result = new double[Acount, BCount];
            for (int i = 0; i < Acount; i++)
            {
                for (int j = 0; j < BCount; j++)
                {
                    result[i, j] = 1;
                }
            }
            return result;
        }
      
    }
}